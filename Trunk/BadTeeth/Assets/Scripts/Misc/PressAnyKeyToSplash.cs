using UnityEngine;
using System.Collections;

public class PressAnyKeyToSplash : MonoBehaviour 
{	
	float m_TimeBeforeInput = 2.0f;

	// Update is called once per frame
	void Update () 
	{
		m_TimeBeforeInput -= Time.deltaTime;

		if(m_TimeBeforeInput < 0.0f && InputManager.getJumpDown ())
		{
			Application.LoadLevel("InputSelect");
		}
	}
}
