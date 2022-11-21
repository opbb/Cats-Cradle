using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalUtils;

public class SkeletonCatGrab : MonoBehaviour
{

    [SerializeField] LayerMask catLayer;

    void OnTriggerEnter2D(Collider2D col) {
        maybeGrabCat(col);
    }

    void OnTriggerStay2D(Collider2D col) {
        maybeGrabCat(col);
    }

    void OnTriggerExit2D(Collider2D col) {
        int colLayer = col.gameObject.layer;
        if (GlobalUtils.isLayerInMask(colLayer, catLayer)) {
            col.SendMessage("resetCanGrabSkeleton");
        }
    }

    private void maybeGrabCat(Collider2D col) {
        int colLayer = col.gameObject.layer;
        if (GlobalUtils.isLayerInMask(colLayer, catLayer)) {
            col.SendMessage("grabSkeleton");
        }
    }
}
