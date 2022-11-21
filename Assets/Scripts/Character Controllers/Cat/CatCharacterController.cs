using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatCharacterController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private LayerMask swattable;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheckLeft;                       // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_GroundCheckRight;                       // A position marking where to check if the player is grounded.
	[SerializeField] private Animator m_animator; 								// The animator for this character
	[SerializeField] private GameObject guidePrefab; 
	[SerializeField] private SkullCatGrab skullCatGrab;							// The script that manages the skull sprite while the cat is grabbing the skeleton 
	[SerializeField] private int numJumpGuides;
	[SerializeField] private float guidesTimeOffset;
	[SerializeField] private float jumpVelocityTransfer; 						// The amount of prior velocity that remains when jumping.
	[SerializeField] private float swatSize; 
	[SerializeField] private float swatForce; 
	[SerializeField] private float swatDist; 
	[SerializeField] private float grabVelDeadzone; 
	[SerializeField] private float grabTeleportHeight; 
	[SerializeField] private float grabLockLength;
	

	private GameObject[] jumpGuides;
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private bool m_Climbing; 			// Whether or not the player is climbing.
	private Collider2D m_Collider; 		// The cat's collider
	private Collider2D m_LedgeCheck;
	private float defaultGravity; 		// The cat's deafault gravity value
	private bool m_isGrabSkeleton = false;		// Whether the cat is currently on the skeleton.
	private bool m_canGrabSkeleton = true;		// Whether or not the cat is currently able to grab the skeleton.

	[Header("Events")]
	[Space]

	public UnityEvent OnJumpEvent;
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Collider = (Collider2D)GetComponent<CapsuleCollider2D>();
		m_LedgeCheck = (Collider2D)GetComponentInChildren<BoxCollider2D>();
		m_Climbing = false;
		defaultGravity = m_Rigidbody2D.gravityScale;

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnJumpEvent == null)
			OnJumpEvent = new UnityEvent();

		jumpGuides = new GameObject[numJumpGuides];

		for (int i = 0; i < numJumpGuides; i++) {
			jumpGuides[i] = Instantiate(guidePrefab, transform.position, Quaternion.identity);
		}

	}

	private void FixedUpdate()
	{
		if (!m_Climbing && !m_isGrabSkeleton) {
			bool wasGrounded = m_Grounded;
			m_Grounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapAreaAll(m_GroundCheckLeft.position, m_GroundCheckRight.position, m_WhatIsGround, -1f, 1f);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					m_Grounded = true;
					if (!wasGrounded)
						OnLandEvent.Invoke();
				}
			}

			m_animator.SetFloat("vertSpeed", m_Rigidbody2D.velocity.y);
			m_animator.SetBool("grounded", m_Grounded);
		} else if (m_isGrabSkeleton) {
			transform.position = skullCatGrab.transform.position;
		}
	}


	public void Move(float move)
	{

		//only control the player if grounded
		if (m_Grounded && !m_Climbing && !m_isGrabSkeleton)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			if ((move > .01f || move < -.01f) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("cat_run") && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("cat_swat")) {
				m_animator.Play("cat_run");
			}

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	}

	public void Swat(Vector2 swatDirection) {
		swatDirection = swatDirection.normalized;
		RaycastHit2D[] swattedObjects = Physics2D.CircleCastAll(transform.position, swatSize, swatDirection, swatDist, swattable, -1f, 1f);
		for (int i = 0; i < swattedObjects.Length; i++) {
			swattedObjects[i].rigidbody.AddForce(swatDirection * swatForce);
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	// Face (only in the X axis) towards the given vector
	public void FaceDirection(Vector3 direction) {
		if((direction.x > 0 && !m_FacingRight) || (direction.x < 0 && m_FacingRight)) {
			Flip();
		}
	}

	// Get m_Grounded variable
	public bool Grounded() {
		return m_Grounded;
	}

	// Get m_Climbing variable
	public bool Climbing() {
		return m_Climbing;
	}

	// Get m_isGrabSkeleton variable
	public bool isGrabSkeleton() {
		return m_isGrabSkeleton;
	}


	private Vector3 calculateJump(Vector2 jumpDirection, float jumpForce) {
		jumpDirection = jumpDirection.normalized;
		Vector2 initialVelPred = m_Rigidbody2D.velocity * jumpVelocityTransfer;
		initialVelPred += jumpDirection * (jumpForce * m_JumpForce);
		return (Vector3)(initialVelPred);
	}

	public void Jump(Vector2 jumpDirection, float jumpForce) {
		if (m_isGrabSkeleton) {
			ungrabSkeleton();
		}
		m_Rigidbody2D.velocity = calculateJump(jumpDirection, jumpForce);
		OnJumpEvent.Invoke();
	}

	public void RenderJumpGuides(Vector2 jumpDirection, float jumpForce) {
		Vector3 initialVelPred = calculateJump(jumpDirection, jumpForce);

		for (int i = 0; i < jumpGuides.Length; i++) {
			jumpGuides[i].transform.position = pointPos(initialVelPred, (i + 1) * guidesTimeOffset);
			((SpriteRenderer)(jumpGuides[i].GetComponent(typeof(SpriteRenderer)))).color = Color.white; // Set spirite renderer to show
		}
	}

	private Vector2 pointPos(Vector3 initialVelocity, float time) {
		return transform.position + initialVelocity * time + (Vector3)(Physics2D.gravity) * (time * time * 0.5f) * defaultGravity;
	}

	public void HideJumpGuides() {
		for (int i = 0; i < jumpGuides.Length; i++) {
			((SpriteRenderer)(jumpGuides[i].GetComponent(typeof(SpriteRenderer)))).color = Color.clear; // Set sprite renderer to transparent
		}
	}


	// Ledge grab section
	// Conditions: Grab ledge IF
	//      - You are not grounded
	//      - You are not moving away from the ledge
	//      - You your right/left ledge grab zone has a left/right ledge in it
	//
	// Actions: A complete ledgegrab DOES
	//      - Marks m_Climbing as true
	//      - Disables collisions
	//      - Snaps you to the ledge object
	//      - Kills your velocity and turns off gravity
	//      - Checks to make sure you are facing the right direction
	//      - Plays ledge climb animation
	//      - After animation, teleports cat to above the ledge
	//      - Re-enables collisions and gravity
	//      - Marks m_Climbing as false
	public void GrabLedge(bool isRight, Transform ledge) {
		
		// If we are not grounded and not already climbing
		if (!m_Grounded && !m_Climbing) {
			m_Climbing = true;
			m_Grounded = true;
			m_animator.SetBool("grounded", true);
			if (isRight == m_FacingRight) {Flip();} // If we're facing away from the ledge then flip
			disableCatInteractions();
			transform.position = ledge.position;
			StartCoroutine(waitForClimbAnim(ledge));
		}
	}

	public IEnumerator waitForClimbAnim(Transform ledge) {
		m_animator.Play("cat_ledge_getup");
		yield return new WaitForSeconds(grabLockLength);
		transform.position = ledge.position + new Vector3(0f, grabTeleportHeight, 0f);
		enableCatInteractions();
		m_Climbing = false;
	}

	private void grabSkeleton() 
	{
		// Grab the skeleton if is grounded and not currently grabbing the skeleton and if can grab skeleton
		if (!m_Grounded && !m_isGrabSkeleton && m_canGrabSkeleton) {
			m_isGrabSkeleton = true; // is now grabbing the skeleton
			m_canGrabSkeleton = false; // Cant grab again until later
			m_Grounded = true;

			// To enter the "grabbing skeleton state" we will turn off collisions, gravity, sprite renderer(cat is in skeleton sprite)
			disableCatInteractions();
			this.SendMessage("clearInputs"); // Tell CatCharacterMovement to clear its input values
		}
	}

	private void ungrabSkeleton() {
		m_isGrabSkeleton = false; // is not grabbing the skeleton
		enableCatInteractions();
		m_Grounded = false;
	}

	private void disableCatInteractions() {
		m_Collider.enabled = false;
		m_LedgeCheck.enabled = false;
		m_Rigidbody2D.gravityScale = 0f;
		m_Rigidbody2D.velocity = Vector3.zero;
	}

	private void enableCatInteractions() {
		m_Collider.enabled = true;
		m_LedgeCheck.enabled = true;
		m_Rigidbody2D.gravityScale = defaultGravity;
	}

	private void resetCanGrabSkeleton() {
		if (!m_isGrabSkeleton) {
			m_canGrabSkeleton = true;
		}
	}
}
