using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSkeletonBalance : MonoBehaviour
{

    [SerializeField] private float targetRotation;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float force;

    // Update is called once per frame
    void Update()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.fixedDeltaTime));
    }
}
