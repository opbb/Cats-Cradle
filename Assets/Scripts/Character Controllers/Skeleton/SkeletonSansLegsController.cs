using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSansLegsController : MonoBehaviour, SkeletonController
{

    [SerializeField] private Rigidbody2D torso;
    [SerializeField] private float verticalForce;
    [SerializeField] private float horizontalForce;
    [SerializeField] LayerMask grabbable;
    [SerializeField] LayerMask grabbableSolid;
    [SerializeField] SkeletonGrab leftHand;
    [SerializeField] SkeletonGrab rightHand;
    [SerializeField] Collider2D catCollider;
    private bool isActive = false;
    private float horizontal = 0f;
    private float vertical = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < colliders.Length; i++) {
            for (int j = i + 1; j < colliders.Length; j++) {
                Physics2D.IgnoreCollision(colliders[i], colliders[j]);
            }
            Physics2D.IgnoreCollision(colliders[i], catCollider);
            Physics2D.IgnoreCollision(catCollider, colliders[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get inputs only if this is the active player
        if(isActive) {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
    }

    void FixedUpdate()
    {
        if (leftHand.GrabbingSolid() || rightHand.GrabbingSolid()) {
            torso.AddForce(new Vector2(horizontal * horizontalForce, vertical * verticalForce));
        } 
    }

    public void switchActive() {
        isActive = !isActive;
        horizontal = 0f;
        vertical = 0f;
    }

    public bool getIsActive() {
        return isActive;
    }

    public LayerMask getGrabbable() {
        return grabbable;
    }

    public LayerMask getGrabbableSolid() {
        return grabbableSolid;
    }

    public bool getRagdoll() {
        return false;
    }
}
