using UnityEngine;
using System.Collections;

public class Stamina : MonoBehaviour 
{	
    int m_Stamina = Constants.MAX_STAMINA;
	public int stamina
	{
		get{ return m_Stamina; }
		set{ m_Stamina = value; }
	}

    const float STAMINA_REGEN_TICK_LENGTH = 0.01f;
    const int STAMINA_REGEN_PER_TICK = 2;
    float m_Timer = STAMINA_REGEN_TICK_LENGTH;

	// Update is called once per frame
	void Update () 
	{
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
	}
}
