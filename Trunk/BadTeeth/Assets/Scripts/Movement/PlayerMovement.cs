using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour 
{
	CharacterController m_Controller;
	
	Vector3 m_Velocity = Vector3.zero;

    bool m_IsGroundedThisFrame;
    bool m_CheckedGroundedThisFrame = false;
    public float GROUNDED_RAYCAST_DISTANCE = 0.3f;
    LayerMask m_GroundedRaycastIgnoreMask;

	public ParticleSystem m_RunParticles;
	public ParticleSystem m_JumpParticles;

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

    public float RUN_SPEED = 500.0f;

    Vector3 m_ExternalMovement = Vector3.zero;

    #endregion

    Stamina m_Stamina;

    List<knockBackMovement> m_KnockBackMovement = new List<knockBackMovement>();
    Vector3 m_FinalKnockBackMovement = Vector3.zero;

    PlayerAnimator m_Animator;

    // Use this for initialization
	void Start () 
	{
		m_Controller = gameObject.GetComponent<CharacterController> ();
		m_GroundedRaycastIgnoreMask = LayerMask.GetMask("Player", "Ignore Raycast");
        m_Stamina = gameObject.GetComponent<Stamina>();
        m_Animator = gameObject.GetComponent<PlayerAnimator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        int i = 0;
        m_FinalKnockBackMovement = Vector3.zero;
        while (i < m_KnockBackMovement.Count)
        {
            m_KnockBackMovement[i].update();
            if (m_KnockBackMovement[i].isExpired())
            {
                m_KnockBackMovement.RemoveAt(i);
                continue;
            }

            m_FinalKnockBackMovement += m_KnockBackMovement[i].m_Velocity;
			
			i++;
        }


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

        m_Controller.Move((m_Velocity + m_ExternalMovement + m_FinalKnockBackMovement) * Time.deltaTime);
        transform.forward = new Vector3(m_Velocity.x, 0.0f, (m_Velocity.x != 0.0f) ? 0.0f : -1.0f);

		if(m_FinalKnockBackMovement.x != 0.0f)
		{
			transform.forward = - Vector3.right * Mathf.Sign (m_FinalKnockBackMovement.x);
		}

        m_Animator.setSpeed(((m_Velocity + m_ExternalMovement + m_FinalKnockBackMovement) * Time.deltaTime).magnitude);

		m_ExternalMovement = Vector3.zero;

		Vector3 resetPosition = transform.position;
		resetPosition.z = 0.0f;
		transform.position = resetPosition;
	}

    void LateUpdate()
    {
        m_CheckedGroundedThisFrame = false;
    }

    public void externalMovement(Vector3 velocity)
    {
        m_ExternalMovement += velocity;
		m_ExternalMovement.z = 0.0f;
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

        if(!m_IsAllowedToMove)
        {
            m_Velocity.x = 0.0f;
            return;
        }

        int runCost = (int)((float)Constants.RUN_COST * Time.deltaTime);
        if (!(InputManager.getRun() && m_Stamina.stamina > runCost))
        {
            m_Velocity.x = GROUNDED_MOVE_SPEED * InputManager.getMovement() * Time.deltaTime;
            if (Mathf.Abs(m_Velocity.x) > 0.1f)
            {
                m_Animator.playAnimation(PlayerAnimator.Animations.Run);
            }
            else
            {
                m_Animator.playAnimation(PlayerAnimator.Animations.Idle);
            }
        }
        else
        {
            m_Stamina.stamina -= runCost;
            m_Velocity.x = RUN_SPEED * InputManager.getMovement() * Time.deltaTime;

            m_Animator.playAnimation(PlayerAnimator.Animations.Sprint);
        }

		if(InputManager.getRun())
		{
			m_RunParticles.Play ();
		}
		else
		{
			m_RunParticles.Stop ();
		}
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

        if (m_IsAllowedToMove)
        {
            if(!InputManager.getRun())
            m_Velocity.x = AIRBORNE_MOVE_SPEED * InputManager.getMovement() * Time.deltaTime;
        }
        else
        {
            m_Velocity.x = 0.0f;
        }
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
        GameManager.Instance.AddThreat(Constants.JUMP_THREAT);

        m_Animator.playAnimation(PlayerAnimator.Animations.Jump);

		GameObject newParticles = (GameObject) Instantiate(m_JumpParticles.gameObject, m_JumpParticles.transform.position, m_JumpParticles.transform.rotation);

		newParticles.particleSystem.Play ();
    }

    void doubleJump()
    {
        m_Velocity.y = INITIAL_JUMP_SPEED;
        m_FloatPower = INITIAL_FLOAT_POWER;
        m_IsHodlingJump = true;
        m_Stamina.stamina -= Constants.DOUBLE_JUMP_COST;
        GameManager.Instance.AddThreat(Constants.DOUBLE_JUMP_THREAT);

		m_Animator.playAnimation(PlayerAnimator.Animations.Double_Jump);
		
		GameObject newParticles = (GameObject) Instantiate(m_JumpParticles.gameObject, m_JumpParticles.transform.position, m_JumpParticles.transform.rotation);
		
		newParticles.particleSystem.Play ();
    }

    public bool getIsGrounded()
    {
        if(!m_CheckedGroundedThisFrame)
        {
            m_CheckedGroundedThisFrame = true;
#if DEBUG || UNITY_EDITOR
            RaycastHit hitInfo;
#endif
            if (/*m_Controller.isGrounded || */Physics.Raycast(transform.position, Vector3.down, out hitInfo, GROUNDED_RAYCAST_DISTANCE, ~m_GroundedRaycastIgnoreMask.value))
            {
				m_IsGroundedThisFrame = true;
                m_Velocity.y = m_Velocity.y < 0.0f ? 0.0f : m_Velocity.y;
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

	public void knockback(Vector3 velocity, int staminaHit, float time = 1.5f)
	{
        m_KnockBackMovement.Add(new knockBackMovement(velocity, time));
		m_Stamina.stamina -= staminaHit;
        m_Animator.playAnimation(PlayerAnimator.Animations.Knockback);

		if(m_Stamina.stamina == 0)
		{
			// TODO: death and stuff
		}
	}
}

class knockBackMovement
{
    public Vector3 m_Velocity;
    Vector3 m_OriginalVelocity;

    float m_TimeLeft;
    float m_OriginalTimeLeft;

    public knockBackMovement(Vector3 velocity, float time)
    {
        m_Velocity = velocity;
        m_OriginalVelocity = velocity;

        m_TimeLeft = time;
        m_OriginalTimeLeft = time;
    }

    public bool isExpired()
    {
        return m_TimeLeft < 0.0f;
    }

    public void update()
    {
        m_TimeLeft -= Time.deltaTime;
        m_Velocity = m_OriginalVelocity * (m_TimeLeft / m_OriginalTimeLeft);
    }
}