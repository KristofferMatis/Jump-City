using UnityEngine;
using System.Collections;

public class Police : MonoBehaviour 
{
	public float m_DiveSpeed;
	public float m_PatrolSpeed;
	public float m_ClimbSpeed;
	public float m_RotationSpeed;
	public float m_DiveKnockbackForce;
	public float m_DiveStaminaHit;
	public float m_DiveChargeDelay;
	public float m_PoliceKnockbackTime;
	public float m_IdleLockTime;

	public float m_PlayerAttackRange;
	public float m_PlayerEscapeRange;

	public bool m_CanClimb;

	float m_DiveChargeTimer;
	PlayerMovement m_Player;

	CharacterController m_Controller;

	Vector3 m_CurrentForward;

	enum PoliceState
	{
		e_Idle,
		e_Patrolling,
		e_Climbing,
		e_Tackling,
		e_Falling,
		e_Knockback
	}

	PoliceState m_CurrentState;

	Vector3 m_CurrentSpeed;

	CollisionFlags m_CollisionFlags;

	float m_KnockbackTimer;

	Collider m_GroundCollider;

	float m_Gravity = 9.8f;

	float m_IdleLockTimer;

	// Use this for initialization
	void Start () 
	{
		m_Player = getPlayer.Instance.gameObject.GetComponent<PlayerMovement>();

		m_CurrentState = PoliceState.e_Idle;

		m_Controller = GetComponent<CharacterController> ();

		m_CurrentForward = Vector3.back;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log (m_CurrentState);
		Debug.Log (m_CurrentSpeed);

		switch(m_CurrentState)
		{
		case PoliceState.e_Climbing:
			DoClimb ();
			break;

		case PoliceState.e_Falling:
			DoFall ();
			break;

		case PoliceState.e_Knockback:
			DoKnockback ();
			break;

		case PoliceState.e_Patrolling:
			DoPatrol ();
			break;

		case PoliceState.e_Tackling:
			DoTackle ();
			break;

		case PoliceState.e_Idle:
			DoIdle();
			break;
		}
		
		m_CollisionFlags = m_Controller.Move (m_CurrentSpeed * Time.deltaTime);

		m_CurrentForward.z = (m_CurrentForward.x == 0.0f) ? -1.0f : 0.0f;

		transform.forward = m_CurrentForward;

		UpdateStateForCollision ();
		UpdateStateForEvents ();
	}

	void EnterClimb()
	{

	}
	
	void EnterFall()
	{

	}
	
	void EnterKnockback()
	{
		
	}
	
	void EnterPatrol()
	{

	}
	
	void EnterTackle()
	{
		
	}
	
	void EnterIdle()
	{
		m_IdleLockTimer = m_IdleLockTime;
	}

	void DoClimb()
	{
		m_CurrentSpeed.x = 0.0f;
		
		m_CurrentSpeed.y = m_ClimbSpeed;
	}

	void DoFall()
	{
		m_CurrentSpeed.x = 0.0f;

		m_CurrentSpeed.y -= m_Gravity * Time.deltaTime;
	}

	void DoKnockback()
	{

	}

	void DoPatrol()
	{
		float playerDirection = Mathf.Sign (m_Player.transform.position.x - transform.position.x);

		m_CurrentSpeed.x = playerDirection * m_PatrolSpeed;
		m_CurrentSpeed.y = 0.0f;

		m_CurrentForward.x = playerDirection;

		if(!m_CanClimb && m_GroundCollider)
		{
			if((playerDirection < 0.0f && transform.position.x <= m_GroundCollider.bounds.min.x)
			   && (playerDirection > 0.0f && transform.position.x >= m_GroundCollider.bounds.max.x))
			{
				m_CurrentSpeed.x = 0.0f;
			}
		}
	}

	void DoTackle()
	{

	}

	void DoIdle()
	{
		m_CurrentSpeed.x = 0.0f;

		m_IdleLockTimer -= Time.deltaTime;
	}

	void UpdateStateForCollision()
	{
		switch(m_CurrentState)
		{
		case PoliceState.e_Climbing:
			if((m_CollisionFlags & CollisionFlags.Sides) == 0)
			{
				m_CurrentState = PoliceState.e_Patrolling;

				EnterPatrol ();
			}
			break;
			
		case PoliceState.e_Falling:
			if((m_CollisionFlags & CollisionFlags.Below) != 0)
			{
				m_CurrentState = PoliceState.e_Patrolling;

				EnterPatrol();

				RaycastHit hitInfo;

				if(Physics.Raycast(transform.position, -transform.up, out hitInfo))
				{
					m_GroundCollider = hitInfo.collider;
				}
			}
			break;
			
		case PoliceState.e_Knockback:
			if((m_CollisionFlags & CollisionFlags.Below) != 0)
			{
				if(m_KnockbackTimer <= 0.0f)
				{
					m_CurrentState = PoliceState.e_Patrolling;

					EnterPatrol ();
				}
			}
			break;
			
		case PoliceState.e_Patrolling:
			if((m_CollisionFlags & CollisionFlags.Sides) != 0)
			{
				if(m_CanClimb && Physics.Raycast(transform.position, m_CurrentForward))
				{
					m_CurrentState = PoliceState.e_Climbing;

					EnterClimb ();
				}
			}
			else if((m_CollisionFlags & CollisionFlags.Below) == 0)
			{
				m_CurrentState = PoliceState.e_Falling;

				EnterFall ();
			}
			break;
			
		case PoliceState.e_Tackling:
			DoTackle ();
			break;

		case PoliceState.e_Idle:
			if((m_CollisionFlags & CollisionFlags.Below) == 0)
			{
				m_CurrentState = PoliceState.e_Falling;

				EnterFall ();
			}
			break;
		}
	}

	void UpdateStateForEvents()
	{
		switch(m_CurrentState)
		{
		case PoliceState.e_Climbing:
			break;
			
		case PoliceState.e_Falling:
			break;
			
		case PoliceState.e_Knockback:
			break;
			
		case PoliceState.e_Patrolling:
		{
			float playerDistance = Mathf.Abs (m_Player.transform.position.x - transform.position.x);

			if(playerDistance > m_PlayerEscapeRange)
			{
				m_CurrentState = PoliceState.e_Idle;

				EnterIdle ();
			}
		}
			break;
			
		case PoliceState.e_Tackling:
			break;

		case PoliceState.e_Idle:
		{
			float playerDistance = Mathf.Abs (m_Player.transform.position.x - transform.position.x);
			
			if(playerDistance < m_PlayerEscapeRange && m_IdleLockTimer <= 0.0f)
			{
				m_CurrentState = PoliceState.e_Idle;
				
				EnterIdle ();
			}
		}
			break;
		}
	}
}
