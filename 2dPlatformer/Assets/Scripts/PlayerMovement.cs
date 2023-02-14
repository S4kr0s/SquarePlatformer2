using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private ParticleSystem jumpParticles;
	[SerializeField] private ParticleSystem dashParticles;
	[SerializeField] private AudioSource jumpAudio;
	[SerializeField] private AudioSource dashAudio;
	[SerializeField] private AudioSource deathAudio;
	[SerializeField] private Animator animator;
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	[SerializeField] private float m_moveSpeedMultiplier = 2f;                // A collider that will be disabled when crouching
	[SerializeField] public int m_midAirJumps = 1;                // A collider that will be disabled when crouching
	[SerializeField] public int m_currentMidAirJumps = 0;                // A collider that will be disabled when crouching
	[SerializeField] private float m_dashForce = 2f;                // A collider that will be disabled when crouching
	[SerializeField] public int m_midAirDashs = 1;                // A collider that will be disabled when crouching
	[SerializeField] public int m_currentMidAirDashs = 0;                // A collider that will be disabled when crouching
	[SerializeField] private bool allowedToJump = true;
	[SerializeField] private bool allowedToDash = true;
	[SerializeField] private bool stoppedJumping = true;
	[Range(0, 2)] [SerializeField] private float jumpTime = 0.75f;
	[SerializeField] private float holdingJumpMultiplier = 0.1f;
	[SerializeField] private float cameraSpeed = 5f;
	[SerializeField] private Vector3 _velocity = Vector3.zero;
	[SerializeField] private bool moveCamera = false;
	[SerializeField] private float gravityPullStrength = -5f;

	[Space]
	[SerializeField] const float k_GroundedRadius = .25f; // Radius of the overlap circle to determine if grounded
	[SerializeField] private bool m_Grounded;            // Whether or not the player is grounded.
	[SerializeField] private Rigidbody2D m_Rigidbody2D;
	[SerializeField] private Vector3 m_Velocity = Vector3.zero;
	[SerializeField] private float jumpTimeCounter = 0.75f;

	[Space]
	[SerializeField] private static bool hasMoved = false;
	[SerializeField] private bool enableAnimation = false;
	public static bool HasMoved => hasMoved;

	private float keyMove = 0f;
	private bool keyJump = false;
	private bool keyDash = false;
	private bool keyHoldJump = false;
	private bool animationSquash = false;

	public int MaxMidAirJumps => m_midAirJumps;
	public int CurrentMidAirJumps => m_currentMidAirJumps;
	public int MaxMidAirDashs => m_midAirDashs;
	public int CurrentMidAirDashs => m_currentMidAirDashs;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		hasMoved = false;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		if (keyMove != 0)
			hasMoved = true;

		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
				}
			}
		}

		Move(keyMove, keyJump, keyDash, keyHoldJump);

		if (keyJump)
			keyJump = false;

		if (keyDash)
			keyDash = false;
	}

    private void Update()
	{
		keyMove = Input.GetAxis("Horizontal") * m_moveSpeedMultiplier;

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
			keyJump = true;

		if (Input.GetKeyDown(KeyCode.LeftShift))
			keyDash = true;

		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
			keyHoldJump = true;
		else
			keyHoldJump = false;


		if (moveCamera)
		{
			Camera.main.gameObject.transform.position = Vector3.SmoothDamp(Camera.main.gameObject.transform.position, new Vector3(this.gameObject.transform.position.x,
				Camera.main.gameObject.transform.position.y, Camera.main.gameObject.transform.position.z), ref _velocity, cameraSpeed * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.R))
        {
			Time.timeScale = 1f;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
			Application.targetFrameRate = 240;

		if (Input.GetKeyDown(KeyCode.Alpha2))
			Application.targetFrameRate = 60;

		if (Input.GetKeyDown(KeyCode.Alpha3))
			Application.targetFrameRate = 30;

		if (Input.GetKeyDown(KeyCode.Alpha4))
			Application.targetFrameRate = 10;
	}

    public void Move(float move, bool jump, bool dash, bool holdJump)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}

		if (!holdJump)
			stoppedJumping = true;

		if (jump && allowedToJump)
			Jump();

        if (dash && allowedToDash)
			Dash();

		if (!m_Grounded && !stoppedJumping && jumpTimeCounter > 0)
		{
			m_Rigidbody2D.velocity += (Vector2.up * m_JumpForce * holdingJumpMultiplier * Time.deltaTime);
			jumpTimeCounter -= Time.deltaTime;
		}

		if (!m_Grounded && (stoppedJumping || jumpTimeCounter <= 0) && m_Rigidbody2D.velocity.y <= 0)
		{
			m_Rigidbody2D.velocity -= (Vector2.up * gravityPullStrength * Time.deltaTime);

			if (animator != null && !animationSquash && enableAnimation)
			{
				animator.ResetTrigger("stretch");
				animator.SetTrigger("normal");
				animator.SetTrigger("squash");
				animationSquash = true;
			}
		}
	}

	private void Jump()
    {
		if (m_Grounded)
		{
			stoppedJumping = false;
			ResetJumpTimer();
			m_Grounded = false;
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0) + (Vector2.up * m_JumpForce) * Time.deltaTime;

			if (jumpParticles != null)
			{
				jumpParticles.transform.position = m_GroundCheck.position;
				jumpParticles.Play();
			}

			if (jumpAudio != null)
			{
				jumpAudio.Play();
			}

			if (animator != null && enableAnimation)
			{
				animator.SetTrigger("stretch");
				animationSquash = false;
            }

			ResetMidAirJumps();
			ResetMidAirDashs();
		}
		else if (!m_Grounded && m_currentMidAirJumps > 0)
		{
			stoppedJumping = false;
			ResetJumpTimer();
			m_currentMidAirJumps--;

			if (jumpParticles != null)
			{
				jumpParticles.transform.position = m_GroundCheck.position;
				jumpParticles.Play();
			}

			if (jumpAudio != null)
			{
				jumpAudio.Play();
			}

			if (animator != null && enableAnimation)
			{
				animator.SetTrigger("stretch");
				animationSquash = false;
			}

			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0) + (Vector2.up * m_JumpForce) * Time.deltaTime;
		}
	}

	private void Dash()
	{
		float dashDirection = Input.GetAxis("Horizontal");

		if (dashDirection == 0)
			return;

		dashDirection = (dashDirection > 0) ? 1 : -1; 

		if (m_Grounded)
		{
			if (dashParticles != null)
			{
				dashParticles.startSpeed = 5 * -dashDirection;
				dashParticles.transform.position = transform.position;
				dashParticles.Play();
			}

			if (jumpAudio != null)
			{
				dashAudio.Play();
			}

			if (animator != null && enableAnimation)
			{
				animator.SetTrigger("normal");
				animationSquash = false;
			}

			//Destroy(dashParticlesInstance.gameObject, 2f);
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.velocity = transform.right * dashDirection * m_dashForce * Time.deltaTime;
			ResetMidAirJumps();
			ResetMidAirDashs();
		}
		else if (!m_Grounded && m_currentMidAirDashs > 0)
		{
			m_currentMidAirDashs--;

			if (dashParticles != null)
			{
				dashParticles.startSpeed = 5 * -dashDirection;
				dashParticles.transform.position = transform.position;
				dashParticles.Play();
			}

			if (jumpAudio != null)
			{
				dashAudio.Play();
			}

			if (animator != null && enableAnimation)
			{
				animator.SetTrigger("normal");
				animationSquash = false;
			}

			//Destroy(dashParticlesInstance.gameObject, 2f);
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.velocity = transform.right * dashDirection * m_dashForce * Time.deltaTime;
		}

	}

	public void SetToNormal()
    {
		if (animator != null && enableAnimation)
		{
			animator.ResetTrigger("stretch");
			animator.ResetTrigger("squash");
			animator.SetTrigger("normal");
		}
	}

	public void ResetMidAirJumps()
	{
		m_currentMidAirJumps = m_midAirJumps;
	}

	public void ResetMidAirDashs()
    {
		m_currentMidAirDashs = m_midAirDashs;
    }

    public void ResetJumpTimer()
    {
		jumpTimeCounter = jumpTime;
    }

    public void AddMidAirJumps(int count)
    {
		m_currentMidAirJumps += count;
	}

	public void AddMidAirDashs(int count)
	{
		m_currentMidAirDashs += count;
	}
}
