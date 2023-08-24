using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum ePlayerState : int { 
    
        WALK = 0,
        RUN,
        JUMP,
        CLIMB,
    }

    public CharacterController2D controller;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    public float speed = 25f;
    public float climbSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float finalSpeed;

        finalSpeed = speed;

        if (Input.GetButtonDown("Climb"))
            Global.currState = Global.ePlayerState.CLIMB;

        if (Input.GetButtonUp("Climb"))
            Global.currState = Global.ePlayerState.WALK;

        if (Input.GetButtonDown("Jump"))
        {
            Global.currState = Global.ePlayerState.JUMP;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = Global.defGravityScale;
        }

        if (Input.GetButtonUp("Run") && Global.isRunning())
            Global.currState = Global.ePlayerState.WALK;

        if (Input.GetButtonDown("Run") && !Global.isClimbing())
        {
            Global.currState = Global.ePlayerState.RUN;
            finalSpeed *= 1.5f;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * finalSpeed;
        verticalMove   = Input.GetAxisRaw("Vertical") * climbSpeed;
    }

    void FixedUpdate ()
    {
        if (!Global.isClimbing())
            controller.Move(horizontalMove * Time.fixedDeltaTime);
        else
            controller.Move(verticalMove * Time.fixedDeltaTime);

        if (Global.isJumping())
            Global.currState = Global.ePlayerState.WALK;

        if (controller.IsGrounded())
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = Global.defGravityScale;
            Global.currState = Global.ePlayerState.WALK;
        }
    }
}
