using UnityEngine;
using System.Collections;

public class Grafiti : MonoBehaviour 
{
	Sign m_Target;
	// Update is called once per frame
	void Update () 
	{
        if (m_Target == null)
            return;

        if(InputManager.getSprayPaint())
        {

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
