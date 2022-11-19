using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabCheck : MonoBehaviour
{
    [SerializeField] CatCharacterController controller;

    void OnTriggerEnter2D(Collider2D col) {
        maybeGrabLedge(col);
    }

    void OnTriggerStay2D(Collider2D col) {
        maybeGrabLedge(col);
    }

    private void maybeGrabLedge(Collider2D col) {

        // If the cat's right side is touching a leftward facing ledge, then we climb
        if(col.tag == "LeftLedge") {
            controller.GrabLedge(false, col.transform);
        } else if (col.tag == "RightLedge") {
            controller.GrabLedge(true, col.transform);
        }
    }
}
