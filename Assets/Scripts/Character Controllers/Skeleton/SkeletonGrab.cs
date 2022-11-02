using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SkeletonGrab : MonoBehaviour
{

    private bool hold;
    [SerializeField] private int mouseButton;
    [SerializeField] private FullSkeletonController fullSkeleton;
    [SerializeField] private SkeletonSansLegsController sansLegs;

    private SkeletonController skeletonController;
    private LayerMask grabbable;

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

        grabbable = skeletonController.getGrabbable();
    }

    // Update is called once per frame
    void Update()
    {
        if (skeletonController.getIsActive()) {
            hold = Input.GetMouseButton(mouseButton);
            if(!hold) {
                Destroy(GetComponent<FixedJoint2D>());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("In On Trigger Enter");
        int layer = col.gameObject.layer; // The layer this collision is with.
        if (hold && (grabbable == (grabbable | (1 << layer))) /* checks if the layer is in the layermask somehow, idk */) {

            Debug.Log("Registered as grabbable");
            Rigidbody2D rb = col.transform.GetComponent<Rigidbody2D>();
            if (rb == null) {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb;
            } else {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
            }
        }
    }
}
