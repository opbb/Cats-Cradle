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
    private LayerMask grabbableSolid;
    private bool isGrabbing;
    private bool isGrabbingSolid;

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

        isGrabbing = false;

        grabbable = skeletonController.getGrabbable();
        grabbableSolid = skeletonController.getGrabbableSolid();
    }

    // Update is called once per frame
    void Update()
    {
        FixedJoint2D holdJoint = GetComponent<FixedJoint2D>();

        isGrabbing = holdJoint != null;

        if (isGrabbing) {
            if (holdJoint.connectedBody == null) {
                isGrabbingSolid = true;
            } else {
                int grabbedLayer = holdJoint.connectedBody.gameObject.layer;
                isGrabbingSolid = isLayerInMask(grabbedLayer, grabbableSolid);
            }
        } else {
            isGrabbingSolid = false;
        }

        if (skeletonController.getIsActive()) {
            hold = Input.GetMouseButton(mouseButton);
            if(!hold) {
                Destroy(GetComponent<FixedJoint2D>());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        int layer = col.gameObject.layer; // The layer this collision is with.
        if (hold && isLayerInMask(layer, grabbable) /* checks if the layer is in the layermask somehow, idk */) {
            Rigidbody2D rb = col.transform.GetComponent<Rigidbody2D>();
            if (rb == null) {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
            } else {
                FixedJoint2D fj = transform.gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                fj.connectedBody = rb;
            }
        }
    }

    private bool isLayerInMask(int layer, LayerMask layerMask) {
        return layerMask == (layerMask | (1 << layer));
    }

    public bool Grabbing() {
        return isGrabbing;
    }

    public bool GrabbingSolid() {
        return isGrabbingSolid;
    }
}
