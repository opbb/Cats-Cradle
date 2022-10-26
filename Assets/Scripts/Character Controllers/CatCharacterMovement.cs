using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCharacterMovement : MonoBehaviour
{

    [SerializeField] private Transform trans;
    [SerializeField] private CatCharacterController controller;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private Animator animator;

    //Tuning Variables
    [SerializeField] private float speed = 0f;

    private float horizontal = 0f;
    private float vertical = 0f;
    private bool leftMouse = false;
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
            dialogueController.Speak("This is a test phrase.");
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

        animator.SetFloat("speed", Mathf.Abs(horizontal));
        horizontalMove = horizontal * speed;

        //--------------------------------------------------------------------------------------------


        controller.Move(horizontalMove * Time.fixedDeltaTime, spaceDown);
    }

    // LateUpdate is called once per frame after every Update() method.
    void LateUpdate()
    {
        
    }
}