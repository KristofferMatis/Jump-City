using UnityEngine;
using System.Collections;

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
				int index = Random.Range(0, m_SpawnPoints.Length);

				m_SpawnPoints[index].SpawnPolice();

				m_SpawnTimer = m_SpawnTime;

				m_NumberOfPoliceSpawned++;
			}
		}
	}
}
