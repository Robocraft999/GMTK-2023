using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_WallCheck;							    // A position marking where to check for ceilings
	[SerializeField] private AudioClip pickupItem;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_WallRadius = .2f;     // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public Item ItemTriggered { get; set; }
	private Transform m_Item = null;

	private Animator m_Animator;
	private AudioSource m_AudioSource;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Animator = GetComponent<Animator>();
		m_AudioSource = GetComponent<AudioSource>();
		m_FacingRight = Math.Sign(m_Rigidbody2D.gravityScale) > 0;

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	public void Update()
	{
		float move = Input.GetAxis("Horizontal") * transform.right.x;
		bool jump = Input.GetKey(KeyCode.Space);
		Move(move, jump);
		
		m_Animator.SetBool("IsWalking", Math.Abs(m_Rigidbody2D.velocity.x) > 0.8f);
		m_Animator.SetBool("IsJumping", !m_Grounded && m_Rigidbody2D.velocity.y > 0);
		m_Animator.SetBool("IsFalling", !m_Grounded && m_Rigidbody2D.velocity.y < 0);

		CheckForItem();
	}


	public void Move(float move, bool jump)
	{
		bool inWall = false;
		if (Physics2D.OverlapCircle(m_WallCheck.position, k_WallRadius, m_WhatIsGround))
		{
			inWall = true;
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity;
			//if (!m_Grounded)
				if (inWall)
				{
					targetVelocity = new Vector2(0, m_Rigidbody2D.velocity.y);
				}
				/*else
				{
					targetVelocity = new Vector2(move * 5f, m_Rigidbody2D.velocity.y);
				}*/
			//else
				targetVelocity = new Vector2(move * 8f, m_Rigidbody2D.velocity.y);
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
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * Math.Sign(m_Rigidbody2D.gravityScale)));
		}
	}

	private void CheckForItem()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			if (!m_Item && ItemTriggered)
			{
				m_Item = ItemTriggered.gameObject.transform;
				m_Item.SetParent(transform);
				m_Item.localPosition = new Vector3(0.4f,0,0);
				m_Item.localRotation = Quaternion.identity;
				m_Item.GetComponent<Rigidbody2D>().simulated = false;
				m_AudioSource.PlayOneShot(pickupItem);
			}
			else if (m_Item)
			{
				m_Item.SetParent(null);
				m_Item.GetComponent<Rigidbody2D>().simulated = true;
				m_Item = null;
			}
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
}
