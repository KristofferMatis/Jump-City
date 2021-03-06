﻿using UnityEngine;
using System.Collections;

public class Police : MonoBehaviour, IHitBoxListener
{
	HitBox m_HitBox;

	public float m_DiveSpeed;
	public float m_PatrolSpeed;
	public float m_ClimbSpeed;
	public float m_RotationSpeed;
	public float m_DiveKnockbackHorizontalForce;
	public float m_DiveKnockbackVerticalForce;
	public float m_DiveChargeDelay;
	public float m_PoliceKnockbackTime;
	public float m_IdleLockTime;
	public float m_MantleTime;
	public float m_DelayBeforeStepOut;
	public float m_StepOutSpeed;

	public Vector3 m_MantleOffset;

	public float m_PlayerAttackRange;
	public float m_PlayerEscapeRange;

	public float m_DiveTime;
	public float m_GetUpTime;

	public bool m_CanClimb;
	public bool m_AutoGetUpAfterHitPlayer = false;

	public float m_KnockedOutTime;

	float m_KnockedOutTimer;

	float m_DiveChargeTimer;
	PlayerMovement m_Player;

	CharacterController m_Controller;

	Vector3 m_CurrentForward;
	
	int m_DiveStaminaHit = Constants.POLICE_STAMINA_HIT;

	enum PoliceState
	{
		e_Idle,
		e_Patrolling,
		e_Climbing,
		e_Tackling,
		e_Falling,
		e_Knockback,
		e_Mantle,
		e_KnockedOut,
		e_StepOutside
	}

	PoliceState m_CurrentState;

	Vector3 m_CurrentSpeed;

	CollisionFlags m_CollisionFlags;

	float m_KnockbackTimer;

	Collider m_GroundCollider;

	float m_Gravity = 9.8f;

	float m_IdleLockTimer;
	float m_PatrolLockTimer;

	float m_MantleTimer;

	float m_DiveTimer;
	float m_GetUpTimer;

	bool m_HasHitPlayer;

	ParticleSystem m_HitParticles;

	Animation m_Animation;

    public AudioClip[] m_EnterPatrol;
    public AudioClip[] HurtClips;
    public AudioClip[] AttackClips;
    AudioSource m_Source;

    const float patrolSoundTime = 5.0f;
    float m_timer = 0.0f;

	// Use this for initialization
	void Start () 
	{
        m_Source = gameObject.AddComponent<AudioSource>();

		m_Player = getPlayer.Instance.gameObject.GetComponent<PlayerMovement>();

		m_CurrentState = PoliceState.e_StepOutside;

		m_Controller = GetComponent<CharacterController> ();

		m_CurrentForward = Vector3.back;

		m_HitBox = GetComponentInChildren<HitBox> ();
		m_HitBox.RegisterListener(this);
		m_HitBox.gameObject.SetActive(false);

		m_HitParticles = GetComponentInChildren<ParticleSystem> ();

		m_Animation = GetComponentInChildren<Animation> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (m_timer < patrolSoundTime)
            m_timer += Time.deltaTime;
		//Debug.Log (m_CurrentState);
		//Debug.Log (m_CurrentSpeed);

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

		case PoliceState.e_Mantle:
			DoMantle();
			break;

		case PoliceState.e_KnockedOut:
			DoKnockedOut();
			break;

		case PoliceState.e_StepOutside:
			DoStepOutside();
			break;
		}
		
		m_CollisionFlags = m_Controller.Move (m_CurrentSpeed * Time.deltaTime);

		m_CurrentForward.z = (m_CurrentForward.x == 0.0f) ? -1.0f : 0.0f;

		transform.forward = m_CurrentForward;

		if(m_CurrentState != PoliceState.e_StepOutside)
		{
			Vector3 resetPosition = transform.position;
			resetPosition.z = 0.0f;
			transform.position = resetPosition;
		}

		UpdateStateForCollision ();
		UpdateStateForEvents ();
	}

	void EnterClimb()
	{
		m_Animation.Play ("Fly_Loop");
	}
	
	void EnterFall()
	{
		m_Animation.Play ("Fly_Loop");
	}
	
	void EnterKnockback()
	{
		m_KnockbackTimer = m_PoliceKnockbackTime;
        m_Source.PlayOneShot(HurtClips[Random.Range(0, HurtClips.Length)]);
		m_Animation.Play ("Knockback");
	}
	
	void EnterPatrol()
	{
		m_CurrentSpeed.x = m_CurrentForward.x * m_PatrolSpeed;
        if (m_timer > patrolSoundTime)
        {
            m_timer = 0.0f;
            m_Source.PlayOneShot(m_EnterPatrol[Random.Range(0, m_EnterPatrol.Length)]);
        }
		m_Animation.Play ("Run");
	}

	void EnterKnockedOut()
	{
		m_CurrentSpeed.x = 0.0f;

		m_KnockedOutTimer = m_KnockedOutTime;

		m_Animation.Play ("KnockedOut");
	}
	
	void EnterTackle()
	{
		m_DiveChargeTimer = m_DiveChargeDelay;

		m_CurrentSpeed.x = 0.0f;

		m_HasHitPlayer = false;

		m_HitBox.gameObject.SetActive(false);

		float playerDirection = Mathf.Sign (m_Player.transform.position.x - transform.position.x);

		m_CurrentForward.x = playerDirection;

		m_Animation.Play ("Tackle");
	}
	
	void EnterIdle()
	{
		m_IdleLockTimer = m_IdleLockTime;

		m_Animation.Play ("Idle2");
	}

	void EnterMantle()
	{
		m_MantleTimer = m_MantleTime;

		m_Animation.Play ("Fly_Land");
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
		m_CurrentSpeed.y -= m_Gravity * Time.deltaTime;

		if(m_KnockbackTimer > 0.0f)
		{
			m_KnockbackTimer -= Time.deltaTime;
		}
	}

	void DoPatrol()
	{
		float playerDirection = Mathf.Sign (m_Player.transform.position.x - transform.position.x);

		m_CurrentSpeed.x = playerDirection * m_PatrolSpeed;
		m_CurrentSpeed.y = -0.1f;

		m_CurrentForward.x = playerDirection;

		if(!m_CanClimb && m_GroundCollider)
		{
			if((playerDirection < 0.0f && transform.position.x <= m_GroundCollider.bounds.min.x + 1.0f)
			   && (playerDirection > 0.0f && transform.position.x >= m_GroundCollider.bounds.max.x - 1.0f))
			{
				m_CurrentSpeed.x = 0.0f;
			}
		}
	}

	void DoTackle()
	{
		if(m_DiveChargeTimer > 0.0f)
		{
			m_DiveChargeTimer -= Time.deltaTime;

			if(m_DiveChargeTimer <= 0.0f)
			{
				m_CurrentSpeed.x = m_CurrentForward.x * m_DiveSpeed;
				
				m_DiveTimer = m_DiveTime;

				m_HitBox.gameObject.SetActive(true);
                m_Source.PlayOneShot(AttackClips[Random.Range(0, AttackClips.Length)]);
			}
		}
		else if(m_DiveTimer > 0.0f)
		{
			m_DiveTimer -= Time.deltaTime;

			if(m_DiveTimer <= 0.0f)
			{
				m_HitBox.gameObject.SetActive(false);

				m_GetUpTimer = m_GetUpTime;

				m_CurrentSpeed.x = 0.0f;
			}
		}
		else if(m_GetUpTimer > 0.0f)
		{
			m_GetUpTimer -= Time.deltaTime;
			
			if(m_GetUpTimer <= 0.0f)
			{
				m_CurrentState = PoliceState.e_Patrolling;

				EnterPatrol();
			}
		}
	}

	void DoIdle()
	{
		m_CurrentSpeed.x = 0.0f;

		if(m_IdleLockTimer > 0.0f)
		{
			m_IdleLockTimer -= Time.deltaTime;

			if(m_IdleLockTimer <= 0.0f)
			{
				m_PatrolLockTimer = m_IdleLockTime;
			}
		}

		m_PatrolLockTimer -= Time.deltaTime;
	}

	void DoMantle()
	{
		m_MantleTimer -= Time.deltaTime;

		m_CurrentSpeed.y = 0.0f;

		if(m_MantleTimer <= 0.0f)
		{
			m_CurrentState = PoliceState.e_Patrolling;

			transform.position += transform.up * m_MantleOffset.y + m_CurrentForward * m_MantleOffset.x;
		}
	}

	void DoKnockedOut()
	{
		m_KnockedOutTimer -= Time.deltaTime;
	}

	void DoStepOutside()
	{
		m_CurrentForward = Vector3.back;

		m_DelayBeforeStepOut -= Time.deltaTime;

		if((m_CollisionFlags & CollisionFlags.Below) == 0)
		{
			m_CurrentSpeed.y -= m_Gravity * Time.deltaTime;
		}

		if(m_DelayBeforeStepOut <= 0.0f)
		{
			m_CurrentSpeed = m_CurrentForward * m_StepOutSpeed;

			if(transform.position.z <= 0.0f)
			{
				m_CurrentState = PoliceState.e_Patrolling;

				EnterPatrol ();

				Vector3 newPosition = transform.position;
				newPosition.z = 0.0f;
				transform.position = newPosition;

				m_CurrentSpeed.z = 0.0f;
			}
		}
	}

	void UpdateStateForCollision()
	{
		switch(m_CurrentState)
		{
		case PoliceState.e_Climbing:

			Vector3 origin = transform.position;
			origin.y = m_Controller.bounds.min.y;
			origin.y += m_MantleOffset.y;

			if(!Physics.Raycast(origin, m_CurrentForward, m_Controller.radius + 0.1f))
			{
				m_CurrentState = PoliceState.e_Mantle;

				EnterMantle ();
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
					m_CurrentState = PoliceState.e_KnockedOut;

					EnterKnockedOut ();
				}
				else
				{
					m_CurrentSpeed.x = 0.0f;
					m_CurrentSpeed.y = 0.0f;
				}
			}
			break;
			
		case PoliceState.e_Patrolling:
			
			RaycastHit hitInfo;

			if((m_CollisionFlags & CollisionFlags.Sides) != 0)
			{
				float playerDirection = Mathf.Sign(m_Player.transform.position.x - transform.position.x);

				if(m_CanClimb && Mathf.Sign (m_CurrentForward.x) == playerDirection && Physics.Raycast(transform.position + 0.3f * transform.up, m_CurrentForward, out hitInfo))
				{
					if(hitInfo.collider.gameObject != m_Player.gameObject)
					{
						m_CurrentState = PoliceState.e_Climbing;

						EnterClimb ();
					}
				}
			}
			else if(Physics.Raycast(transform.position , -transform.up, out hitInfo, 0.2f))
			{
				Vector3 newPosition = transform.position;
				newPosition.y = hitInfo.point.y + 0.1f;
				transform.position = newPosition;
			}
			else if((m_CollisionFlags & CollisionFlags.Below) == 0)
			{
				m_CurrentState = PoliceState.e_Falling;

				EnterFall ();
			}
			break;
			
		case PoliceState.e_Tackling:
			if((m_CollisionFlags & CollisionFlags.Below) == 0)
			{
				m_CurrentState = PoliceState.e_Falling;
				
				EnterFall ();
			}
			break;

		case PoliceState.e_Idle:
			if((m_CollisionFlags & CollisionFlags.Below) == 0)
			{
				m_CurrentState = PoliceState.e_Falling;

				EnterFall ();
			}
			break;

		case PoliceState.e_KnockedOut:
			{

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
			else if(playerDistance <= m_PlayerAttackRange)
			{
				float playerDistanceInY = Mathf.Abs (m_Player.transform.position.y - transform.position.y);

				if(playerDistanceInY <= 2.0f)
				{
					m_CurrentState = PoliceState.e_Tackling;

					EnterTackle ();
				}
			}
		}
			break;
			
		case PoliceState.e_Tackling:
			break;

		case PoliceState.e_Idle:
		{
			float playerDistance = Mathf.Abs (m_Player.transform.position.x - transform.position.x);
			
			if(playerDistance < m_PlayerEscapeRange && (m_IdleLockTimer > 0.0f || m_PatrolLockTimer <= 0.0f))
			{
				m_CurrentState = PoliceState.e_Patrolling;
				
				EnterPatrol ();
			}
		}
			break;

		case PoliceState.e_KnockedOut:
			{

			}
			break;
		}
	}

	public void OnHitBoxEnter (Collider otherCollider)
	{
		PlayerMovement player = otherCollider.GetComponent<PlayerMovement> ();

		if(!m_HasHitPlayer && player)
		{
			player.knockback(m_CurrentForward * m_DiveKnockbackHorizontalForce + transform.up * m_DiveKnockbackVerticalForce, m_DiveStaminaHit, 0.5f);

			m_HasHitPlayer = true;

			Vector3 newPosition = m_HitParticles.transform.position;
			newPosition.z = -0.5f;
			m_HitParticles.transform.position = newPosition;

			m_HitParticles.Play ();

			m_CurrentSpeed.x = 0.0f;

			if(m_AutoGetUpAfterHitPlayer)
			{
				m_HitBox.gameObject.SetActive(false);

				m_CurrentState = PoliceState.e_Patrolling;

				EnterPatrol();
			}
		}
	}

	public bool Knockback(Vector3 knockbackSpeed)
	{
		bool result = true;

		if(m_CurrentState == PoliceState.e_Knockback)
		{
			result = false;
		}

		m_CurrentState = PoliceState.e_Knockback;

		EnterKnockback ();

		m_CurrentSpeed = knockbackSpeed;

		m_CurrentForward.x = - Mathf.Sign (m_CurrentSpeed.x);

		return result;
	}
}
