using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SkeletonArms : MonoBehaviour
{

    [SerializeField] private int speed;
    [SerializeField] private KeyCode mouseButton;
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rotationZOffset;
    [SerializeField] private FullSkeletonController fullSkeleton;
    [SerializeField] private SkeletonSansLegsController sansLegs;

    private SkeletonController skeletonController;

    // Start is called before the first frame update
    void Start()
    {
        if (fullSkeleton != null) {
            skeletonController = (SkeletonController) fullSkeleton;
        } else if (sansLegs != null) {
            skeletonController = (SkeletonController) sansLegs;
        } else {
            throw new Exception("No skeleton controller assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (skeletonController.getIsActive()) {

            Vector3 playerPos = cam.ScreenToWorldPoint(Input.mousePosition);
            playerPos.z = 0f;

            Vector3 difference = playerPos - transform.position;

            float rotationZ = Mathf.Atan2(difference.x, difference.y) * -Mathf.Rad2Deg + rotationZOffset;

            if(Input.GetKey(mouseButton)) {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.fixedDeltaTime));
            }
        }
    }
}
