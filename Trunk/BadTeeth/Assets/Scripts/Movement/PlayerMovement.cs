using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour 
{
#if DEBUG || UNITY_EDITOR
    public bool i_DrawRays = true;
#endif
	CharacterController m_Controller;
	
	Vector3 m_Velocity = Vector3.zero;

    bool m_IsGroundedThisFrame;
    bool m_CheckedGroundedThisFrame = false;
    const float GROUNDED_RAYCAST_DISTANCE = 0.217f;

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
    const float GROUNDED_MOVE_SPEED = 4.0f;
    const float AIRBORNE_MOVE_SPEED = 2.5f;

    const float INITIAL_JUMP_SPEED = 0.155f;
    const float GRAVITY = 0.6f;

    const float INITIAL_FLOAT_POWER = 0.2f;
    const float FLOAT_POWER_LOSS = 0.14f;
    float m_FloatPower = 0.0f;

    const float JUMP_FORGIVNESS = 0.15f;
    float m_JumpForgivnessTimer = 0.0f;

	bool m_CanAirJump = false;
    bool m_IsHodlingJump = false;

    #endregion

    // Use this for initialization
	void Start () 
	{
		m_Controller = gameObject.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
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

        if(InputManager.getJumpDown())
        {
            m_JumpForgivnessTimer = JUMP_FORGIVNESS;
            State = States.Airborne;
            jump();
        }

        m_Velocity.x = GROUNDED_MOVE_SPEED * InputManager.getMovement() * Time.deltaTime;
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

            if(InputManager.getJumpDown())
            {
                jump();
                m_JumpForgivnessTimer = 0.0f;
            }
		}
        else if(m_CanAirJump)
        {
            if (InputManager.getJumpDown())
            {
				m_CanAirJump = false;
                jump();
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

        m_Velocity.x = AIRBORNE_MOVE_SPEED * InputManager.getMovement() * Time.deltaTime;
    }

    void exitAirborne()
    {

    }

    void jump()
    {
        m_Velocity.y = INITIAL_JUMP_SPEED;
        m_FloatPower = INITIAL_FLOAT_POWER;
        m_IsHodlingJump = true;
    }

    public bool getIsGrounded()
    {
        if(!m_CheckedGroundedThisFrame)
        {
            m_CheckedGroundedThisFrame = true;
#if DEBUG || UNITY_EDITOR
            RaycastHit hitInfo;
#endif
            if(m_Controller.isGrounded || Physics.Raycast(transform.position, Vector3.down,out hitInfo, GROUNDED_RAYCAST_DISTANCE))
            {
				m_IsGroundedThisFrame = true;
#if DEBUG || UNITY_EDITOR
                if(i_DrawRays)
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.cyan);
                }
#endif
            }
			else
			{
				m_IsGroundedThisFrame = false;
			}
        }
        return m_IsGroundedThisFrame;
    }
}
