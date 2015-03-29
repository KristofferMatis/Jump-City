using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour 
{
	public GameObject m_PolicePrefab;
	public Transform m_SpawnPosition;

	Animation m_AnimatedDoor;

	public bool IsAvailable
	{
		get
		{
			return !m_AnimatedDoor.isPlaying;
		}
	}

	// Use this for initialization
	void Start () 
	{
		m_AnimatedDoor = GetComponentInChildren<Animation> ();
	}
	
	public void SpawnPolice()
	{
		Instantiate (m_PolicePrefab, m_SpawnPosition.transform.position, Quaternion.identity);

		m_AnimatedDoor.Play ();
	}
}
