using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour 
{
	SpawnPoint[] m_SpawnPoints;
	
	public float m_SpawnTime;
	float m_SpawnTimer;

	int m_NumberOfPoliceSpawned;

	void Start()
	{
		m_SpawnPoints = GetComponentsInChildren<SpawnPoint> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_NumberOfPoliceSpawned < (int)(GameManager.Instance.ThreatLevel * Constants.POLICE_PER_THREAT_RATIO))
		{
			if(m_SpawnTimer > 0.0f)
			{
				m_SpawnTimer -= Time.deltaTime;
			}
			else
			{
				int numberOfTries = 0;

				do
				{
					int index = Random.Range(0, m_SpawnPoints.Length);

					if(m_SpawnPoints[index].IsAvailable)
					{
						m_SpawnPoints[index].SpawnPolice();

						m_SpawnTimer = m_SpawnTime;
					}

					numberOfTries++;
				}
				while(m_SpawnTimer <= 0.0f && numberOfTries < m_SpawnPoints.Length * 2);

				m_NumberOfPoliceSpawned++;
			}
		}
	}
}
