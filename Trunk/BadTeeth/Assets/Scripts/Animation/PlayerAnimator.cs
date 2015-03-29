using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour 
{
	Animator m_Animator;

	const float CROSS_FADE_TIME = 0.05f;

	string[] m_States = new string[]{"Run", 
                                    "Sprint",
									"Double_Jump",
									"Jump",
									"Shake_Can", 
									"Spray_Billboard", 
									"Idle", 
									"Knockback", 
									"Drink",
									"Punch",
                                    "New State",
									"BNew State"};

	public enum Animations
	{
		Run,
        Sprint,
		Double_Jump,
		Jump,
		Shake_Can, 
		Spray_Billboard, 
		Idle, 
		Knockback, 
		Drink,
		Punch,
        BlankMisc,
        BlankKnockBack
	};

	// Use this for initialization
	void Start () 
	{
		m_Animator = gameObject.GetComponentInChildren<Animator> ();
	}

	public void playAnimation(Animations animation)
	{
        Debug.Log(m_States[(int)animation]);
        if (animation != Animations.Punch && animation != Animations.Drink && animation != Animations.Spray_Billboard && animation != Animations.Shake_Can && animation != Animations.BlankMisc)
		{
			m_Animator.Play (m_States [(int)animation]);//, CROSS_FADE_TIME);
		}
        else if (animation != Animations.BlankKnockBack && animation != Animations.BlankKnockBack)
		{
			m_Animator.Play(m_States[(int)animation], 1);
		}
        else if (animation != Animations.BlankKnockBack)
        {
            m_Animator.Play(m_States[(int)animation], 3);
        }
	}

    const string SPEED_VARIALBE_NAME = "Speed";
	public void setSpeed(float speed)
    {
        m_Animator.SetFloat(SPEED_VARIALBE_NAME, speed);
    }
}
