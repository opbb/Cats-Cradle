using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullCharacterController : MonoBehaviour {

[SerializeField] new private Rigidbody2D rigidbody;
[SerializeField] private float speed = 0f;

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

            float torque = horizontal * speed * -1f;

            rigidbody.AddTorque(torque, ForceMode2D.Force); 
        }
    }

    public void switchActive() {
        isActive = !isActive;
    }
}
