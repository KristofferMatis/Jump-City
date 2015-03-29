using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour 
{
	Animation m_Animation;

	bool m_IsSpawned;

	void Start()
	{
		m_Animation = GetComponentInChildren<Animation> ();

		if(m_Animation)
		{
			m_Animation.gameObject.SetActive (false);
		}
	}

	public void CreateLadder()
	{
		if(!m_IsSpawned)
		{
			m_IsSpawned = true;

			if(m_Animation)
			{
				m_Animation.gameObject.SetActive (true);

				m_Animation.Play ();
			}
		}
	}
}
