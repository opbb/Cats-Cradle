using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] new private Camera camera;
    [SerializeField] private Transform catTransform;
    [SerializeField] private Transform skeletonTransform;

    [Header("Camera Positioning Settings")]
    [SerializeField] private float cameraDistance;
    public float verticalOffset;
    [SerializeField] private float maxCamDistance;

    [Header("Camera Movement Settings")]
    [SerializeField] private float camMoveWithMouseFactor;
    [SerializeField] private float camSmoothTime;
    private Vector3 currentCamVelocity = Vector3.zero; // Used by Vector3.SmoothDamp. Please don't touch unless you know what you're doing.
    
    
    private bool catActive;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation.Set(0f,0f,0f,0f);
        catActive = true;
        setCameraPos(getParentPos());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = getNewPos();
        setCameraPos(newPos);
    }

    private Vector3 getParentPos() {
        Vector3 activePos;
        if(catActive) {
            activePos = catTransform.position;
        } else {
            activePos = skeletonTransform.position;
        }

        activePos.y += verticalOffset;

        return activePos;
    }

    private Vector3 getTargetPos() {
        Vector3 parentPos = getParentPos();
        Vector3 activePos = parentPos;

        Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 activeToMouse = mousePos - activePos;

        activeToMouse.x = activeToMouse.x / Screen.width * camMoveWithMouseFactor;
        activeToMouse.y = activeToMouse.y / Screen.height * camMoveWithMouseFactor;
        activeToMouse.z = 0f;

        // Limit camera movement
        activeToMouse = Vector3.ClampMagnitude(activeToMouse, maxCamDistance);

        return new Vector3(parentPos.x + activeToMouse.x, parentPos.y + activeToMouse.y, cameraDistance);
    }

    private Vector3 getNewPos() {
        Vector3 targetPos = getTargetPos();
        return Vector3.SmoothDamp(transform.position, targetPos, ref currentCamVelocity, camSmoothTime);
    }

    private void setCameraPos(Vector3 pos) {
        pos.z = cameraDistance;

        transform.position = pos;
    }
    
    public void switchActive() {
        catActive = !catActive;
    }
    
}
