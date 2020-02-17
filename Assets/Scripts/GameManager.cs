using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    
    private GameObject player;
    private GameObject floor;
    private Spawner spawner;

    void Awake(){
        floor = GameObject.Find("Foreground");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
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
        ResetGame();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerKilled(){
        spawner.active = false;
            // Destroy Player
        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();
        //! When player is killed, the delegate is unlinked
        playerDestroyScript.DestroyCallback -= OnPlayerKilled;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
    }

    void ResetGame(){
        spawner.active = true;

        // Create Player object
        player = GameObjectUtil.Instantiate(
            playerPrefab, 
            new Vector3(0, (Screen.height/PixelPerfectCamera.pixelsToUnits)/2, 0)
        );

        // Destroy Player
        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();
        
        //! when OnDestroyCallback called, OnPlayerKilled is called
        playerDestroyScript.DestroyCallback += OnPlayerKilled; 
    }


}
