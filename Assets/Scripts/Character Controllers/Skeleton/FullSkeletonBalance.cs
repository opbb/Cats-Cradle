using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSkeletonBalance : MonoBehaviour
{

    [SerializeField] private float targetRotation;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float force;
    [SerializeField] private FullSkeletonController fullSkeleton;
    [SerializeField] private SkeletonSansLegsController sansLegs;
    
    private SkeletonController controller;

    void Start()
    {
        if (fullSkeleton != null) {
            controller = (SkeletonController) fullSkeleton;
        } else if (sansLegs != null) {
            controller = (SkeletonController) sansLegs;
        } else {
            throw new Exception("No skeleton controller assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.getRagdoll()) {
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.fixedDeltaTime));
        }
    }
}
