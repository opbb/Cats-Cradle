using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCharacterMovement : MonoBehaviour
{

    [SerializeField] private Transform trans;
    [SerializeField] private CatCharacterController controller;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Animator animator;
    [SerializeField] private new Camera camera;
    [SerializeField] FMODUnity.StudioEventEmitter footsteps;

    //Tuning Variables
    [SerializeField] private float speed = 0f;
    [SerializeField] private float maxJump;

    private float horizontal = 0f;
    private float vertical = 0f;
    private bool leftMouse = false;
    private bool leftMouseWasDown = false;
    private bool rightMouse = false;
    private bool spaceDown = false;
    private bool shiftDown = false;
    private float horizontalMove = 0f;

    public 

    // Start is called before the first frame update.
    void Start()
    {
        
    }

    // Update is called once per frame.
    void Update()
    {




        //TESTING:
        //--------------------------------------------------------------------------------------------

        if (rightMouse) {
            dialogueManager.triggerDialogue(3);
        }

        //--------------------------------------------------------------------------------------------
    }

    // FixedUpdate is called independently of frames, and so is used for physics calculations.
    void FixedUpdate()
    {

        //INPUTS:
        //--------------------------------------------------------------------------------------------

        // These 2 variables get the player input horizontally and vertically.
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // These 2 variables get the player's clicks
        leftMouse = Input.GetButton("Fire1");

        rightMouse = Input.GetButtonDown("Fire2");

        //These variables get the player's other inputs
        spaceDown = Input.GetButton("Jump");
        shiftDown = Input.GetButton("Fire3");

        //--------------------------------------------------------------------------------------------


        //MOVEMENT:
        //--------------------------------------------------------------------------------------------

        //if (shiftDown) { speed = normSpeed * speedFactor; } else { speed = normSpeed; }

        // TODO: MAke an audio manager, this system is messy
        float speedParam = Mathf.Abs(horizontal);
        animator.SetFloat("speed", speedParam);
        if(speedParam > 0 && !footsteps.IsPlaying()) {
            footsteps.Play();
        } else if (speedParam < .01f && footsteps.IsPlaying()) {
            footsteps.Stop();
        }
        
        horizontalMove = horizontal * speed;

        controller.Move(horizontalMove * Time.fixedDeltaTime);
        
        if (controller.Grounded() && (leftMouse || leftMouseWasDown)) {
            //Find difference between mouse position (in worldspace) and cat position
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 catToMouse = mousePos - transform.position;
        
            // Limit jump strength
            catToMouse = Vector3.ClampMagnitude(catToMouse, maxJump);
            
            // Face towards mouse
            controller.FaceDirection(catToMouse);
            
            if (!leftMouse) {
                // zero velocity then add old velocity to this then clamp to make run jumping less effective?
                //Vector3 newVelocity = catToMouse + Rigidbody2D.velocity

                // Jump
                controller.Jump(catToMouse, catToMouse.magnitude);
            }
        }

        //--------------------------------------------------------------------------------------------

        // Final Variable Updates
        leftMouseWasDown = leftMouse;
    }

    // LateUpdate is called once per frame after every Update() method.
    void LateUpdate()
    {
        
    }
}