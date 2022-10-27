using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatCharacterController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheckLeft;                       // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_GroundCheckRight;                       // A position marking where to check if the player is grounded.
	[SerializeField] private Animator m_animator; 								// The animator for this character
	[SerializeField] private float maxAngle; 								    // The maximum angle at which this character can rotate.


	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
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
 /*
		if(m_Grounded) {
			transform.rotation.Set(0f,0f,0f,0f);
		} else {
			Vector2 velocity = m_Rigidbody2D.velocity;
			if(velocity.x < 0f) {
				// Make sure it's facing the right direction.
				if (m_FacingRight) {
					Flip();
				}

			
			} else if (velocity.x > 0f) {
				// Make sure it's facing the right direction.
				if (!m_FacingRight) {
					Flip();
				}


			}

			if(velocity.x != 0f) {
				Quaternion rotation = Quaternion.LookRotation(Vector3.forward, velocity);
				Vector3 angles = rotation.eulerAngles;
				Debug.Log(angles);
				
				if (angles.z > maxAngle) {
					rotation.eulerAngles = new Vector3(0f,0f,maxAngle);
				} else if (angles.z < -maxAngle) {
					rotation.eulerAngles = new Vector3(0f,0f,-maxAngle);
				}

				transform.rotation = rotation;
			}
		}
		*/

		m_animator.SetBool("grounded", m_Grounded);
	}


	public void Move(float move, bool jump)
	{

		//only control the player if grounded
		if (m_Grounded)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

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
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	public void Jump(Vector2 jumpDirection, float jumpForce) {
		jumpDirection = jumpDirection.normalized;
		m_Rigidbody2D.AddForce(jumpDirection * (jumpForce * m_JumpForce));
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
}
