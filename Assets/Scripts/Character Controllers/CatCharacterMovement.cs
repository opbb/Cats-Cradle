using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCharacterMovement : MonoBehaviour
{
    [SerializeField] private CatCharacterController controller;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Animator animator;
    [SerializeField] private new Camera camera;
    [SerializeField] FMODUnity.StudioEventEmitter footstepsEmitter;

    //Tuning Variables
    [SerializeField] private float speed = 0f;
    [SerializeField] private float maxJumpDist;

    [HideInInspector] public bool isActive = true;
    private float horizontal = 0f;
    private float vertical = 0f;
    private bool leftMouse = false;
    private bool leftMouseWasDown = false;
    private bool rightMouse = false;
    private bool shiftDown = false;
    private float horizontalMove = 0f;

    // Start is called before the first frame update.
    void Start()
    {
        
    }

    // Update is called once per frame.
    void Update()
    {
        if (isActive) {

            //TESTING:
            //--------------------------------------------------------------------------------------------

            if (rightMouse) {
                dialogueManager.triggerDialogue(3);
            }

            //--------------------------------------------------------------------------------------------
        }
    }

    // FixedUpdate is called independently of frames, and so is used for physics calculations.
    void FixedUpdate()
    {
        if (isActive) {
            //INPUTS:
            //--------------------------------------------------------------------------------------------

            // These 2 variables get the player input horizontally and vertically.
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            // These 2 variables get the player's clicks
            leftMouse = Input.GetButton("Fire1");

            rightMouse = Input.GetButtonDown("Fire2");

            //These variables get the player's other inputs
            
            shiftDown = Input.GetButton("Fire3");

        }

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
        
        if (controller.Grounded() && (leftMouse || leftMouseWasDown)) {
            //Find difference between mouse position (in worldspace) and cat position
            Vector3 mousePos = Input.mousePosition;
            Vector3 catPos = camera.WorldToScreenPoint(transform.position);
            Vector3 catToMouse = mousePos - catPos;
            float arbitraryValue = 100f;
            catToMouse.x = catToMouse.x / Screen.width * arbitraryValue;
            catToMouse.y = catToMouse.y / Screen.height * arbitraryValue;
            catToMouse.z = 0f;
            // Limit jump strength
            Debug.Log("Before: " + catToMouse);
            catToMouse = Vector3.ClampMagnitude(catToMouse, maxJumpDist);
            Debug.Log("After: " + catToMouse);
            
            // Face towards mouse
            controller.FaceDirection(catToMouse);
            
            if (!leftMouse) {
                // Jump
                controller.Jump(catToMouse, catToMouse.magnitude);
            }
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