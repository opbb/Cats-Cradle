using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSansLegsController : MonoBehaviour, SkeletonController
{

    [SerializeField] private Rigidbody2D torso;
    [SerializeField] private float verticalForce;
    [SerializeField] private float horizontalForce;
    [SerializeField] private float floatDistance;
    [SerializeField] LayerMask grabbable;
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
        RaycastHit2D[] colliders = Physics2D.CircleCastAll(torso.transform.position, floatDistance * torso.transform.lossyScale.x, Vector2.down, .01f, grabbable, -1f, 1f);
    
        if (colliders.Length > 0) {
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
}
