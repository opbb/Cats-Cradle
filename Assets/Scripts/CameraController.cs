using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform catTransform;
    [SerializeField] private Transform skeletonTransform;
    [SerializeField] private Transform listener;
    [SerializeField] private float cameraDistance;
    public float verticalOffset;
    private Transform activeTransform;
    //private bool catActive;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation.Set(0f,0f,0f,0f);
        activeTransform = catTransform;
        moveToActive();
    }

    // Update is called once per frame
    void Update()
    {
        moveToActive();
    }

    private void moveToActive() {

        Vector3 parentPos = activeTransform.position;
        Vector3 thisPos = new Vector3(parentPos.x, parentPos.y + verticalOffset, cameraDistance);
        transform.position = thisPos;
        listener.position = activeTransform.position;
    }
}
