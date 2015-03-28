using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour 
{
	CharacterController m_Controller;
	
	Vector3 m_Velocity = Vector3.zero;

    bool m_IsGroundedThisFrame;
    bool m_CheckedGroundedThisFrame = false;
    const float GROUNDED_RAYCAST_DISTANCE = 0.217f;
    LayerMask m_GroundedRaycastIgnoreMask;

    public enum States
    {
        Grounded,
        Airborne
    };

    States m_State = States.Grounded;
	public States State
    {
        get { return m_State; }
        set { setState(value); }
    }

    #region Movement Variables
    public float GROUNDED_MOVE_SPEED = 4.0f;
    public float AIRBORNE_MOVE_SPEED = 2.5f;

    public float INITIAL_JUMP_SPEED = 0.155f;
    public float GRAVITY = 0.6f;

    public float INITIAL_FLOAT_POWER = 0.2f;
    public float FLOAT_POWER_LOSS = 0.14f;
    float m_FloatPower = 0.0f;

    public float JUMP_FORGIVNESS = 0.15f;
    float m_JumpForgivnessTimer = 0.0f;

	bool m_CanAirJump = false;
    bool m_IsHodlingJump = false;

	bool m_IsAllowedToMove = true;
	public bool IsAllowedToMove
	{
		get { return m_IsAllowedToMove; }
        set { m_IsAllowedToMove = value; }
	}

	bool m_FullStop = false;
    public bool FullStop
    {
        get { return m_FullStop; }
        set { m_FullStop = value; }
    }

    #endregion

    Stamina m_Stamina;

    // Use this for initialization
	void Start () 
	{
		m_Controller = gameObject.GetComponent<CharacterController> ();
		m_GroundedRaycastIgnoreMask = LayerMask.GetMask("Player", "Ignore Raycast");
        m_Stamina = gameObject.GetComponent<Stamina>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (m_FullStop)
			return;

        switch (m_State)
        {
            case States.Airborne:
                airborne();
                break;
            case States.Grounded:
                grounded();
                break;
        };

		m_Controller.Move (m_Velocity);
        transform.forward = new Vector3(m_Velocity.x, 0.0f, (m_Velocity.x != 0.0f) ? 0.0f : -1.0f);
	}

    void LateUpdate()
    {
        m_CheckedGroundedThisFrame = false;
    }

    void setState(States state)
    {
        if(m_State != state)

        switch (m_State)
        {
            case States.Airborne:
                exitAirborne();
                break;
            case States.Grounded: 
                exitGrounded(); 
                break;
        };

        m_State = state;

        switch (m_State)
        {
            case States.Airborne:
                onAirborne();
                break;
            case States.Grounded:
                onGrounded();
                break;
        };
    }

    void onGrounded()
    {

    }

    void grounded()
    {
        if(!getIsGrounded())
        {
            m_JumpForgivnessTimer = JUMP_FORGIVNESS;
            State = States.Airborne;
            return;
        }

        if( m_Stamina.stamina > Constants.JUMP_COST && InputManager.getJumpDown())
        {
            m_JumpForgivnessTimer = JUMP_FORGIVNESS;
            State = States.Airborne;
            jump();
        }

		m_Velocity.y -= Time.deltaTime * GRAVITY;

		if(m_IsAllowedToMove)
        	m_Velocity.x = GROUNDED_MOVE_SPEED * InputManager.getMovement() * Time.deltaTime;
		else
			m_Velocity.x = 0.0f;
    }

    void exitGrounded()
    {

    }

    void onAirborne()
    {
		m_CanAirJump = true;
    }

    void airborne()
    {
        if(getIsGrounded())
        {
            State = States.Grounded;
			return;
        }

		if(m_JumpForgivnessTimer > 0.0f)
		{
			m_JumpForgivnessTimer -= Time.deltaTime;

            if (m_Stamina.stamina > Constants.JUMP_COST && InputManager.getJumpDown())
            {
                jump();
                m_JumpForgivnessTimer = 0.0f;
            }
		}
        else if(m_CanAirJump)
        {
            if (m_Stamina.stamina > Constants.DOUBLE_JUMP_COST && InputManager.getJumpDown())
            {
				m_CanAirJump = false;
                doubleJump();
            }
        }

        if(m_IsHodlingJump)
        {
            if(InputManager.getJumpUp() || m_FloatPower <= 0.0f)
            {
                m_IsHodlingJump = false;
                m_FloatPower = 0.0f;
            }
            else
            {
                m_Velocity.y += Time.deltaTime * m_FloatPower;
                m_FloatPower -= Time.deltaTime * FLOAT_POWER_LOSS;
            }
        }

        m_Velocity.y -= Time.deltaTime * GRAVITY;

		if(m_IsAllowedToMove)
        	m_Velocity.x = AIRBORNE_MOVE_SPEED * InputManager.getMovement() * Time.deltaTime;
		else
			m_Velocity.x = 0.0f;
    }

    void exitAirborne()
    {

    }

    void jump()
    {
        m_Velocity.y = INITIAL_JUMP_SPEED;
        m_FloatPower = INITIAL_FLOAT_POWER;
        m_IsHodlingJump = true;
        m_Stamina.stamina -= Constants.JUMP_COST;
    }

    void doubleJump()
    {
        m_Velocity.y = INITIAL_JUMP_SPEED;
        m_FloatPower = INITIAL_FLOAT_POWER;
        m_IsHodlingJump = true;
        m_Stamina.stamina -= Constants.DOUBLE_JUMP_COST;
    }

    public bool getIsGrounded()
    {
        if(!m_CheckedGroundedThisFrame)
        {
            m_CheckedGroundedThisFrame = true;
#if DEBUG || UNITY_EDITOR
            RaycastHit hitInfo;
#endif
            if (m_Controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, out hitInfo, GROUNDED_RAYCAST_DISTANCE, ~m_GroundedRaycastIgnoreMask.value))
            {
				m_IsGroundedThisFrame = true;
				m_Velocity.y = 0.0f;
            }
			else
			{
				m_IsGroundedThisFrame = false;
			}
            //Debug.Log(m_IsGroundedThisFrame);

            //Debug.Log((hitInfo.collider != null) ? hitInfo.collider.name : "controller");
        }
        return m_IsGroundedThisFrame;
    }

	public void knockback(Vector3 knockbackSpeed)
	{
		m_Velocity = knockbackSpeed;
	}
}
