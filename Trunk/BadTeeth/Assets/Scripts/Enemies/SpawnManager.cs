using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour 
{
	public GameObject m_PolicePrefab;

	List<Transform> m_SpawnPoints;

	public float m_SpawnTime;
	float m_SpawnTimer;

	int m_NumberOfPoliceSpawned;

	void Start()
	{
		m_SpawnPoints = new List<Transform> ();

		foreach(Transform child in transform)
		{
			m_SpawnPoints.Add (child);
		}
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
				int index = Random.Range(0, m_SpawnPoints.Count);

				Instantiate (m_PolicePrefab, m_SpawnPoints[index].position, Quaternion.identity);

				m_SpawnTimer = m_SpawnTime;

				m_NumberOfPoliceSpawned++;
			}
		}
	}
}
