using UnityEngine;
using System.Collections;

public class Grafiti : MonoBehaviour , CallBack
{
	Sign m_Target;
    PlayerMovement m_PlayerMovement;

    const float MIN_PLAYER_DIST = 0.3f;
	const float PLAYER_CORRECTION_SPEED = 5.0f;

	float savedProgress = 0.0f;

    PlayerAnimator m_Animator;
	bool m_DoneShaking = false;

	public ParticleSystem m_PaintParticles;

    void Start()
    {
        m_PlayerMovement = gameObject.GetComponent<PlayerMovement>();
        m_Animator = gameObject.GetComponent<PlayerAnimator>();
        gameObject.GetComponentInChildren<AnimationCallBackManager>().registerCallBack(this);
    }

	// Update is called once per frame
	void Update () 
	{
        if (m_Target == null)
            return;

        if(m_Target.m_IsFinished)
        {
			GameManager.Instance.AddThreat(Constants.TAG_THREAT * (m_Target.NormalizedPaintedTime - savedProgress));
			savedProgress = m_Target.NormalizedPaintedTime;
            m_Target = null;
            m_PlayerMovement.IsAllowedToMove = true;
            m_PlayerMovement.FullStop = false;
            m_Animator.playAnimation(PlayerAnimator.Animations.BlankMisc);

			m_PaintParticles.Stop ();

            return;
        }

        if(InputManager.getSprayPaint())
        {
            m_PlayerMovement.IsAllowedToMove = false;
            if((m_PlayerMovement.transform.position - m_Target.i_PlayerMount.position).magnitude < MIN_PLAYER_DIST)
            {
                if (m_DoneShaking)
                {
                    m_Target.PaintedTime += Time.deltaTime;
                    transform.forward = new Vector3(0.0f, 0.0f, 1.0f);
                    m_Animator.playAnimation(PlayerAnimator.Animations.Spray_Billboard);
                    m_PlayerMovement.FullStop = true;

					
					m_PaintParticles.Play ();
                }
                else
                {
                    m_Animator.playAnimation(PlayerAnimator.Animations.Shake_Can);
                }
            }
            else
            {
				m_PlayerMovement.externalMovement(PLAYER_CORRECTION_SPEED * (m_Target.i_PlayerMount.position - m_PlayerMovement.transform.position).normalized);
                m_DoneShaking = false;
            }
        }
		else if(InputManager.getSprayPaintUp())
        {
            m_PlayerMovement.IsAllowedToMove = true;
			GameManager.Instance.AddThreat(Constants.TAG_THREAT * (m_Target.NormalizedPaintedTime - savedProgress));
			savedProgress = m_Target.NormalizedPaintedTime;
            m_DoneShaking = false;
            m_PlayerMovement.FullStop = false;
            m_Animator.playAnimation(PlayerAnimator.Animations.BlankMisc);

			m_PaintParticles.Stop ();
        }
	}

    public void CallBack(CallBackEvents callBack)
    {
        if(callBack == CallBackEvents.Shake_Done)
        {
            m_DoneShaking = true;
        }
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Sign", System.StringComparison.OrdinalIgnoreCase))
		{
			Sign temp = other.gameObject.GetComponent<Sign>();
			if(temp != null)
			{
				if(!temp.m_IsFinished)
				{
                    m_Target = temp;
					savedProgress = m_Target.NormalizedPaintedTime;
                    return;
				}
			}
		}
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Sign", System.StringComparison.OrdinalIgnoreCase))
        {
            Sign temp = other.gameObject.GetComponent<Sign>();
            if (temp != null)
            {
                if (temp == m_Target)
                {
                    m_Target = null;
					savedProgress = 0.0f;
                    return;
                }
            }
        }
    }
}
