using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSkeletonController : MonoBehaviour, SkeletonController
{

    [SerializeField] private Animator animator;
    [SerializeField] private float jumpForce;
    [SerializeField] private float playerSpeed;
    [SerializeField] private Vector2 jumpHeight;
    [SerializeField] private bool grounded;
    [SerializeField] private float positionRadius;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] LayerMask grabbable;
    [SerializeField] LayerMask grabbableSolid;
    [SerializeField] Collider2D catCollider;
    
    private bool isActive = false;
    private float horizontal = 0f;
    private float vertical = 0f;
    private bool isRagdoll;

    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < colliders.Length; i++) {
            for (int j = i + 1; j < colliders.Length; j++) {
                Physics2D.IgnoreCollision(colliders[i], colliders[j]);
            }
            if (!(colliders[i].tag == "CatGrabTrigger")) {
                Physics2D.IgnoreCollision(colliders[i], catCollider);
                Physics2D.IgnoreCollision(catCollider, colliders[i]);
            }
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

        isRagdoll = vertical == -1f;

        if (!isRagdoll && horizontal != 0f) {
            if (horizontal > 0f) {
                animator.Play("fullskeleton_walkRight");
                rb.AddForce(Vector2.right * playerSpeed * Time.fixedDeltaTime);
            } else {
                animator.Play("fullskeleton_walkLeft");
                rb.AddForce(Vector2.left * playerSpeed * Time.fixedDeltaTime);
            }
        } else {
            animator.Play("fullskeleton_idle");
        }
    }

    public void switchActive() {
        isActive = !isActive;
        horizontal = 0f;
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
        return isRagdoll;
    }
}
