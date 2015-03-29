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
				float spawnPointDistance = Mathf.Infinity;
				int index = -1;

				for(int i = 0; i < m_SpawnPoints.Length; i++)
				{
					float distance = Mathf.Abs(m_SpawnPoints[i].transform.position.x - getPlayer.Instance.transform.position.x);

					if(distance < spawnPointDistance && m_SpawnPoints[i].IsAvailable)
					{
						spawnPointDistance = distance;

						index = i;
					}
				}

				if(index != -1)
				{
					m_SpawnPoints[index].SpawnPolice();

					m_SpawnTimer = m_SpawnTime;

					m_NumberOfPoliceSpawned++;
				}
			}
		}
	}
}
