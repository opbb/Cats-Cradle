using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCharacterMovement : MonoBehaviour
{
    [SerializeField] private CatCharacterController controller;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Animator animator;
    [SerializeField] private new Camera camera;

    // Sound Emitters
    [SerializeField] FMODUnity.StudioEventEmitter footstepsEmitter;
    [SerializeField] FMODUnity.StudioEventEmitter swatEmitter;

    //Tuning Variables
    [SerializeField] private float speed = 0f;
    [SerializeField] private float maxJumpDist;
    [SerializeField] private float jumpDelay;
    [SerializeField] private int swatCooldown = 0;

    [HideInInspector] public bool isActive = true;
    private float horizontal = 0f;
    private float vertical = 0f;
    private bool leftMouse = false;
    private bool leftMouseWasDown = false;
    private bool rightMouse = false;
    private bool shiftDown = false;
    private float horizontalMove = 0f;
    private int swatCooldownTimer = 0;

    // Start is called before the first frame update.
    void Start()
    {
        
    }

    // Update is called once per frame.
    void Update()
    {
        if (isActive && !controller.Climbing()) {
            //INPUTS:
            //--------------------------------------------------------------------------------------------

            // These 2 variables get the player input horizontally and vertically.
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            // These 2 variables get the player's clicks
            leftMouse = Input.GetButton("Fire1");

            rightMouse = Input.GetButton("Fire2");

            //These variables get the player's other inputs
            
            shiftDown = Input.GetButtonDown("Fire3");
        }
    }

    // FixedUpdate is called independently of frames, and so is used for physics calculations.
    void FixedUpdate()
    {
        //--------------------------------------------------------------------------------------------
        //AUDIO & ANIMATION:
        //--------------------------------------------------------------------------------------------

        float speedParam = Mathf.Abs(horizontal);
        animator.SetFloat("speed", speedParam);
        if(speedParam > 0 && !footstepsEmitter.IsPlaying()) {
            footstepsEmitter.Play();
        } else if (speedParam < .01f && footstepsEmitter.IsPlaying()) {
            footstepsEmitter.Stop();
        }


        //--------------------------------------------------------------------------------------------
        //MOVEMENT:
        //--------------------------------------------------------------------------------------------
        
        horizontalMove = horizontal * speed;

        controller.Move(horizontalMove * Time.fixedDeltaTime);
        
        // Swatting Section
        if(controller.Grounded() && rightMouse && swatCooldownTimer > swatCooldown) {
            // Cancel Jump
            leftMouse = false;
            leftMouseWasDown = false;

            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDirection = (Vector2)(mousePos - transform.position);
            mouseDirection = mouseDirection.normalized;

            controller.FaceDirection(mouseDirection);

            animator.Play("cat_swat");
            swatEmitter.Play();

            controller.Swat(mouseDirection);

            swatCooldownTimer = 0;
        } else {
            swatCooldownTimer++;
        }

        // Jumping Section
        if (controller.Grounded() && (leftMouse || leftMouseWasDown) && isActive) {
            //Find difference between mouse position (in worldspace) and cat position
            Vector3 mousePos = Input.mousePosition;
            Vector3 catPos = camera.WorldToScreenPoint(transform.position);
            Vector3 catToMouse = mousePos - catPos;
            float arbitraryValue = 100f;
            catToMouse.x = catToMouse.x / Screen.width * arbitraryValue;
            catToMouse.y = catToMouse.y / Screen.height * arbitraryValue;
            catToMouse.z = 0f;

            // Limit jump strength
            catToMouse = Vector3.ClampMagnitude(catToMouse, maxJumpDist);
            
            // Face towards mouse
            if (speedParam < .01f) {
                controller.FaceDirection(catToMouse);
            }
            
            if (!leftMouse) {
                // Jump
                animator.SetBool("readyJump", false);
                controller.HideJumpGuides();
                controller.Jump(catToMouse, catToMouse.magnitude);
            } else {
                if (!animator.GetBool("readyJump") && speedParam < .01f) {
                    animator.SetBool("readyJump", true);
                }
                controller.RenderJumpGuides(catToMouse, catToMouse.magnitude);
            }
        } else {
            animator.SetBool("readyJump", false);
            controller.HideJumpGuides();
        }

        //--------------------------------------------------------------------------------------------

        // Final Variable Updates
        leftMouseWasDown = leftMouse;
    }

    public void switchActive() {
        isActive = !isActive;
        horizontal = 0f;
        vertical = 0f;
        leftMouse = false;
        rightMouse = false;
        shiftDown = false;
        leftMouseWasDown = false;
    }

    // LateUpdate is called once per frame after every Update() method.
    void LateUpdate()
    {
        
    }
}