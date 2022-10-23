using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCharacterMovement : MonoBehaviour
{

    [SerializeField] private Transform trans;
    [SerializeField] private CatCharacterControler controler;

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

        //INPUTS:
        //--------------------------------------------------------------------------------------------

        // These 2 variables get the player input horizontally and vertically.
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // These 2 variables get the player's clicks
        leftMouse = Input.GetButton("Fire1");
        rightMouse = Input.GetButton("Fire2");

        //These variables get the player's other inputs
        spaceDown = Input.GetButton("Jump");
        shiftDown = Input.GetButton("Fire3");

        //--------------------------------------------------------------------------------------------


        //MOVEMENT:
        //--------------------------------------------------------------------------------------------

        //if (shiftDown) { speed = normSpeed * speedFactor; } else { speed = normSpeed; }

        horizontalMove = horizontal * speed;

        //--------------------------------------------------------------------------------------------

    }

    // FixedUpdate is called independently of frames, and so is used for physics calculations.
    void FixedUpdate()
    {
        controler.Move(horizontalMove * Time.fixedDeltaTime, shiftDown, spaceDown);
    }

    // LateUpdate is called once per frame after every Update() method.
    void LateUpdate()
    {
        
    }
}