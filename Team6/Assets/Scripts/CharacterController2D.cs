using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_LeftPillarCheck;                       // A position marking where to check if the player is near a pillar on the left side.
	[SerializeField] private Transform m_RightPillarCheck;                      // A position marking where to check if the player is near a pillar on the right side.
	[SerializeField] private LayerMask m_WhatIsPillar;                          // A mask determining what is ground to the character

	const float k_PillarSearchRadius = .2f;                                     // Radius of the overlap circle to determine if the player is near a pillar.
	const float k_GroundedRadius = .2f;                                         // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;                                                    // Whether or not the player is grounded.
	public bool m_IsNearPillar;                                                // Whether or not the player is near a pillar.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;                                          // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	// need these to move player along with platform
    [SerializeField] private LayerMask platformLayer;
    private Transform platform = null;
    private bool isOnThePlatform = false;

	public Transform savedCheckPoint;

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

        savedCheckPoint = transform;
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
			if (colliders[i].gameObject != gameObject && colliders[i].gameObject.tag == "Ground")
			{
				m_Grounded = true;

				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		IsNearAnyPillar();
    }

	private void Update ()
	{
        if (platform != null)
            transform.parent = platform;
        else
            transform.parent = null;

        isOnThePlatform = isOnPlatform();
    }

	public void Move(float HorizontalMove, float VerticalMove)
	{

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			if (!Global.isClimbing()) // Walk / Jump
			{
				float verticalVelocity = Global.isJumping() ? VerticalMove : m_Rigidbody2D.velocity.y;

				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(HorizontalMove * 10f, verticalVelocity);

				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (HorizontalMove > 0 && !m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (HorizontalMove < 0 && m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
			}
			else
			{
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, VerticalMove * 10f);

				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			}

		}

		// If the player should jump...
		if ((m_Grounded || m_IsNearPillar) && Global.isJumping())
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

			if (Global.isJumping())
				Global.currState = Global.ePlayerState.WALK;
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

	public bool IsGrounded ()
	{
		return m_Grounded;
	}

    public bool IsNearToAPillar(Transform pPillarCheck)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pPillarCheck.position, k_PillarSearchRadius, m_WhatIsPillar);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && colliders[i].gameObject.tag == "Pillar")
            {
                //gameObject.transform.position = colliders[i].transform.position;

                m_IsNearPillar = true;
                return m_IsNearPillar;
            }
        }

		m_IsNearPillar = false;
        return m_IsNearPillar;
    }

    public bool IsNearAnyPillar ()
	{
		if (IsNearToAPillar(m_LeftPillarCheck) && m_FacingRight)
		{
            //Flip();
			return m_IsNearPillar;
        }

        //if (IsNearToAPillar(m_RightPillarCheck) && !m_FacingRight)
          //  Flip();

		IsNearToAPillar(m_RightPillarCheck);

        return m_IsNearPillar;
    }

    private bool isOnPlatform()
    {
        RaycastHit2D hit = Physics2D.CircleCast(m_GroundCheck.position, k_GroundedRadius, Vector2.down, 0.1f, platformLayer);

        if (hit)
			isOnThePlatform = true;
        
		if (hit)
			this.platform = hit.collider.gameObject.transform;
        else
			this.platform = null;
        
		return hit.collider != null;
    }

	public void ReSpawnAtSavedCheckPoint ()
	{
		transform.position = savedCheckPoint.position;
    }
}
