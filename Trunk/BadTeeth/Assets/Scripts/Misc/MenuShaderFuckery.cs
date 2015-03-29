using UnityEngine;
using System.Collections;

public class MenuShaderFuckery : MonoBehaviour 
{
	public float TimeToFINISHTHEFUCKERY  = 1.5f;
	float m_Timer = 0.0f;

	// Update is called once per frame
	void Update () 
	{
		m_Timer += Time.deltaTime;
		renderer.material.SetFloat ("_PercentPainted", m_Timer / TimeToFINISHTHEFUCKERY);
		if(m_Timer > TimeToFINISHTHEFUCKERY)
		{
			Destroy(this);
		}
	}
}
