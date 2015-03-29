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
									"New State",
                                    "Punch2"};

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
        BlankKnockBack,
        Punch2
	};

	// Use this for initialization
	void Start () 
	{
		m_Animator = gameObject.GetComponentInChildren<Animator> ();
	}

	public bool playAnimation(Animations animation)
	{
        //Debug.Log(m_States[(int)animation]);
        if (animation != Animations.Punch && animation != Animations.Punch2 && animation != Animations.Drink && animation != Animations.Spray_Billboard && animation != Animations.Shake_Can && animation != Animations.BlankMisc)
		{
			m_Animator.Play (m_States [(int)animation]);//, CROSS_FADE_TIME);
		}
        else if (animation != Animations.BlankKnockBack && animation != Animations.BlankKnockBack && animation != Animations.Punch && animation != Animations.Punch2)
		{
			m_Animator.Play(m_States[(int)animation], 1);
		}
        else if (animation != Animations.BlankKnockBack && animation != Animations.BlankKnockBack)
        {
            if(m_Animator.GetCurrentAnimatorStateInfo(2).IsTag("Blank"))
                m_Animator.Play(m_States[(int)animation], 2);
            else
                return false;
        }
        else
        {
            m_Animator.Play(m_States[(int)animation], 4);
        }
        return true;
	}

    const string SPEED_VARIALBE_NAME = "Speed";
	public void setSpeed(float speed)
    {
        m_Animator.SetFloat(SPEED_VARIALBE_NAME, speed);
    }
}
