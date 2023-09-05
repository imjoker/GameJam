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
    private Animator PlayerAnimator;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    public float speed = 25f;
    public float climbSpeed = 1f;
    public bool IsClimbingAbilityUnlocked = false;

    public string FootStepName;
    const string SoundPath= "event:/Player/";


    //Code added by Joe to enable camera following character
    public Camera mianCamera;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float finalSpeed;

        finalSpeed = speed;

        if (IsClimbingAbilityUnlocked && Input.GetButton("Climb") && controller.IsNearAnyPillar())
        {
            // TODO: Add transition to Monkey here.

            // Stops the player from being affected by gravity while on ladder
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            Global.currState = Global.ePlayerState.CLIMB;
            PlayerAnimator.SetTrigger("Climb");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/climb");
        }

        if (!Input.GetButton("Climb") && Global.isClimbing())
        {
            // TODO: Add transition back to player here.

            // reset gravity
            gameObject.GetComponent<Rigidbody2D>().gravityScale = Global.defGravityScale;
            Global.currState = Global.ePlayerState.WALK;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/land");
        }

        if (Input.GetButtonUp("Run") && Global.isRunning())
            Global.currState = Global.ePlayerState.WALK;

        if (Input.GetButtonDown("Run") && !Global.isClimbing())
        {
            Global.currState = Global.ePlayerState.RUN;
            finalSpeed *= 1.5f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Global.currState = Global.ePlayerState.JUMP;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = Global.defGravityScale;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/jump");
            PlayerAnimator.SetTrigger("Jump");
        }


    SetSpeed:

        horizontalMove = Input.GetAxisRaw("Horizontal") * finalSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
        PlayerAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        //Code added by Joe to enable camera following character
        mianCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);

        controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);

        //if (Global.isJumping())
        //    Global.currState = Global.ePlayerState.WALK;

        if ((controller.IsGrounded() && !Global.isClimbing()) || !controller.m_IsNearPillar)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = Global.defGravityScale;
            Global.currState = Global.ePlayerState.WALK;
        }
    }

    public void PlayFootStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SoundPath+ FootStepName);
    }


    void FixedUpdate()
    {

    }
}
