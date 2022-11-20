using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkullCharacterController : MonoBehaviour, SkeletonController {

    [SerializeField] new private Rigidbody2D rigidbody;
    [SerializeField] private FMODUnity.StudioEventEmitter rollingSound;
    [SerializeField] private FMODUnity.StudioEventEmitter rollingCollision;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float soundSpeed;

    private int collidersTouching = 0;
    [HideInInspector] public bool isActive = false;
    private float horizontal = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called a set number of times per second
    void FixedUpdate()
    {
        if (isActive) {
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        float torque = horizontal * speed * -1f;

        rigidbody.AddTorque(torque, ForceMode2D.Force); 

        if(rigidbody.angularVelocity > soundSpeed && collidersTouching > 0) {
            rollingSound.Play();
        } else {
            rollingSound.Stop();
        }
    }

    
    void OnCollisionEnter2D(Collision2D col) {
        collidersTouching++;
        rollingCollision.Play();
    }

    void OnCollisionExit2D(Collision2D col) {
        collidersTouching--;
    }

    public void switchActive() {
        isActive = !isActive;
        horizontal = 0f;
    }

    public bool getIsActive() {
        return isActive;
    }

    public LayerMask getGrabbable() {
        return -1;
    }
    public LayerMask getGrabbableSolid() {
        return -1;
    }

    public bool getRagdoll() {
        return false;
    }
}
