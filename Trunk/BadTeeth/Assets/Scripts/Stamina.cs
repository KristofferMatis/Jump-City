using UnityEngine;
using System.Collections;

public class Stamina : MonoBehaviour , CallBack
{	
    int m_Stamina = Constants.MAX_STAMINA;
	public int stamina
	{
		get{ return m_Stamina; }
		set{ m_Stamina = Mathf.Clamp (value, 0, m_Stamina); }
	}

    const float STAMINA_REGEN_TICK_LENGTH = 0.01f;
    const int STAMINA_REGEN_PER_TICK = 2;
    float m_Timer = STAMINA_REGEN_TICK_LENGTH;

    const int MAX_ENERGY_DRINKS = 3;
    const int ENERYGY_DRINK_RESTORATION_VALUE = 700;
    int m_CurrentEnergyDrinks = MAX_ENERGY_DRINKS;

    PlayerAnimator m_Animator;
    bool m_IsDoneDrinking = false;

    void Start()
    {
        m_Animator = gameObject.GetComponent<PlayerAnimator>();
        GetComponentInChildren<AnimationCallBackManager>().registerCallBack(this);
    }

	// Update is called once per frame
	void Update () 
	{
        
        if (m_IsDoneDrinking)
        {
            m_CurrentEnergyDrinks--;
            m_Stamina += ENERYGY_DRINK_RESTORATION_VALUE;
            m_Animator.playAnimation(PlayerAnimator.Animations.BlankMisc);
            gameObject.GetComponent<PlayerMovement>().IsAllowedToMove = true;
            m_IsDoneDrinking = false;
        }
        

        if (m_CurrentEnergyDrinks > 0 && InputManager.getDrinkDown())
        {       
            m_Animator.playAnimation(PlayerAnimator.Animations.Drink);
            gameObject.GetComponent<PlayerMovement>().IsAllowedToMove = false;
        }

        m_Timer -= Time.deltaTime;
        if (m_Timer > 0.0f)
            return;

        m_Timer = STAMINA_REGEN_TICK_LENGTH;
        if(m_Stamina < Constants.MAX_STAMINA)
        {
            if(m_Stamina + STAMINA_REGEN_PER_TICK > Constants.MAX_STAMINA)
            {
                m_Stamina = Constants.MAX_STAMINA;
            }
            else
            {
                m_Stamina += STAMINA_REGEN_PER_TICK;
            }
        }
        else if(m_Stamina > Constants.MAX_STAMINA)
        {
            m_Stamina -= STAMINA_REGEN_PER_TICK;
        }       
	}

    public void CallBack(CallBackEvents callBack)
    {
        if(callBack == CallBackEvents.Drink_Done)
        {
            m_IsDoneDrinking = true;
        }
    }
}
