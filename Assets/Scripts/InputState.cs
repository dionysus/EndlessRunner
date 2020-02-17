using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//* Determine physical state of the player
//* Absolute x/y, velocity, isStanding, user Input (jump)
public class InputState : MonoBehaviour
{

    public bool actionButton;
    public float absVelX = 0f; 
    public float absVelY = 0f;
    public bool standing;
    public float standingThreshold = 1;

    private Rigidbody2D body2d;

    void Awake()
    {
        body2d = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        actionButton = Input.anyKeyDown; // Simple implementation - ANY key
    }

    void FixedUpdate() // physics calculations go in fixedUpdate
    {
        absVelX = System.Math.Abs(body2d.velocity.x);
        absVelY = System.Math.Abs(body2d.velocity.y);

        standing = absVelY <= standingThreshold;
        //normally, would use collider to test player collision with floor
    }
}
