using UnityEngine;
using System.Collections;

public class Grafiti : MonoBehaviour 
{
	Sign m_Target;
    PlayerMovement m_PlayerMovement;

    void Start()
    {
        m_PlayerMovement = gameObject.GetComponent<PlayerMovement>();
    }

	// Update is called once per frame
	void Update () 
	{
        if (m_Target == null)
            return;

        if(InputManager.getSprayPaint())
        {
            m_PlayerMovement.IsAllowedToMove = false;
			m_Target.PaintedTime += Time.deltaTime;
            transform.forward = new Vector3(0.0f, 0.0f, 1.0f);
        }
        else
        {
            m_PlayerMovement.IsAllowedToMove = true;
        }
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Sign",System.StringComparison.OrdinalIgnoreCase))
		{
			Sign temp = other.gameObject.GetComponent<Sign>();
			if(temp != null)
			{
				if(!temp.m_IsFinished)
				{
                    m_Target = temp;
                    return;
				}
			}
		}
	}

    void OnTriggerStay(Collider other)
    {
        if (m_Target != null)
            return;

        if (other.tag.Equals("Sign", System.StringComparison.OrdinalIgnoreCase))
        {
            Sign temp = other.gameObject.GetComponent<Sign>();
            if (temp != null)
            {
                if (!temp.m_IsFinished)
                {
                    m_Target = temp;
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
                    return;
                }
            }
        }
    }
}
