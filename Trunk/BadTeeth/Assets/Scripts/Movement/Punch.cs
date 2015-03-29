using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour, IHitBoxListener
{
	public bool m_CanHitMultipleEnemies = false;

	public float m_HorizontalKnockback;
	public float m_VerticalKnockback;

	public float m_PunchTime;
	public float m_TimeToCombo;

	float m_PunchTimer;
	float m_ComboTimer;

	HitBox m_HitBox;
	ParticleSystem m_HitParticles;

	int m_Combo;

	Stamina m_Stamina;
    PlayerAnimator m_Animator;

    
    public AudioClip[] m_PunchClips;
    AudioSource m_Source;
	void Start()
	{
        m_Source = gameObject.AddComponent<AudioSource>();

		m_HitBox = GetComponentInChildren<HitBox> ();
		m_HitBox.RegisterListener(this);
		m_HitBox.gameObject.SetActive(false);
		
		m_HitParticles = GetComponentInChildren<ParticleSystem> ();

        m_Animator = GetComponentInChildren<PlayerAnimator>();
		m_Stamina = GetComponent<Stamina> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if(m_PunchTimer > 0.0f)
		{
			m_PunchTimer -= Time.deltaTime;

			if(m_PunchTimer <= 0.0f)
			{
				m_HitBox.gameObject.SetActive(false);

				m_ComboTimer = m_TimeToCombo;
			}
		}
		else if(InputManager.getPunchDown() && m_Stamina.stamina >= Constants.PUNCH_COST)
		{
			m_PunchTimer = m_PunchTime;

			m_HitBox.gameObject.SetActive(true);

			m_Stamina.stamina -= Constants.PUNCH_COST;
            
			if(m_Combo == 0)
				m_Animator.playAnimation(PlayerAnimator.Animations.Punch2);
			else
				m_Animator.playAnimation(PlayerAnimator.Animations.Punch);

			if(m_ComboTimer > 0.0f)
			{
				m_Combo++;
				m_Combo %= 2;               
			}
		}

		m_ComboTimer -= Time.deltaTime;

		if(m_ComboTimer <= 0.0f)
		{
			m_Combo = 0;
		}
	}

	public void OnHitBoxEnter(Collider otherCollider)
	{
		Police police = otherCollider.GetComponent<Police> ();
		
		if(police)
		{
			bool hasActuallyHit = police.Knockback(transform.forward * m_HorizontalKnockback + transform.up * m_VerticalKnockback);
			
			Vector3 newPosition = m_HitParticles.transform.position;
			newPosition.z = -0.5f;
			m_HitParticles.transform.position = newPosition;
            m_Source.PlayOneShot(m_PunchClips[Random.Range(0, m_PunchClips.Length)]);
			m_HitParticles.Play ();


			if(hasActuallyHit && !m_CanHitMultipleEnemies)
			{
				m_HitBox.gameObject.SetActive(false);
			}
		}
	}
}
