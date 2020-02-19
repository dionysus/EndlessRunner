using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public Text continueText;
    public Text scoreText;

    private float timeElapsed = 0f;
    private float bestTime = 0f;
    private float blinkTime = 0f;
    private bool blink;
    private bool gameStarted;
    private TimeManager timeManager;
    private GameObject player;
    private GameObject floor;
    private Spawner spawner;
    private bool beatBestTime;

    void Awake(){
        floor = GameObject.Find("Foreground");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        timeManager = GetComponent<TimeManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // Re-position the floor to near the bottom of the screen
        var floorHeight = floor.transform.localScale.y;
        var pos = floor.transform.position;

        pos.x = 0;
        pos.y = -((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2) + (floorHeight / 2);
        floor.transform.position = pos;
        spawner.active = false;

        // Start Game
        Time.timeScale = 0;
        continueText.text = "PRESS ANY BUTTON TO START!";

    }


    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Time.timeScale == 0) {
            if(Input.anyKeyDown){
                timeManager.ManipulateTime(1, 1f);
                ResetGame();
            }
        }
    
        if (!gameStarted) {
            blinkTime++;
            if (blinkTime % 40 == 0) {
                blink = !blink;
            }

            continueText.canvasRenderer.SetAlpha(blink ? 0 : 1);

            var textColor = beatBestTime ? "#FF0" : "#FFF";

            scoreText.text = ( 
                "TIME: " + FormatTime(timeElapsed) + 
                "\n<color="+textColor+">BEST: " + FormatTime(bestTime) + "</color>"
                );
        
        } else {
            timeElapsed += Time.deltaTime;
            scoreText.text = (
                "TIME: " + FormatTime(timeElapsed)
                );
        }
    
    }

    void OnPlayerKilled(){
        spawner.active = false;
            // Destroy Player
        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();
        //! When player is killed, the delegate is unlinked
        playerDestroyScript.DestroyCallback -= OnPlayerKilled;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timeManager.ManipulateTime(0, 5.5f);

        gameStarted = false;

        continueText.text = "PRESS ANY BUTTON TO RESTART!";

        if (timeElapsed > bestTime) {
            bestTime = timeElapsed;
            PlayerPrefs.SetFloat("BestTime", bestTime); //! PPrefs to store
            beatBestTime = true;
        }
    }

    void ResetGame(){
        spawner.active = true;

        // Create Player object
        player = GameObjectUtil.Instantiate(
            playerPrefab, 
            new Vector3(0, (Screen.height/PixelPerfectCamera.pixelsToUnits)/2 + 100, 0)
        );

        // Destroy Player
        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();
        //! when OnDestroyCallback called, OnPlayerKilled is called
        playerDestroyScript.DestroyCallback += OnPlayerKilled;

        gameStarted = true;
         
        continueText.canvasRenderer.SetAlpha(0);
        timeElapsed = 0;
        beatBestTime = false;
    }

    string FormatTime(float value) {

        TimeSpan t = TimeSpan.FromSeconds (value);
        return string.Format(
            "{0:D2}:{1:D2}:{2:D2}", 
            t.Minutes, t.Seconds, t.Milliseconds
            );
    }

}
