using UnityEngine;
using System.Collections;

public class GameManager
{
	static GameManager s_Instance;

	public static GameManager Instance
	{
		get
		{
			if(s_Instance == null)
			{
				s_Instance = new GameManager();
			}

			return s_Instance;
		}
	}

	public float ThreatLevel 
	{
		get;
		protected set;
	}

	GameManager()
	{
		ThreatLevel = 10.0f;
	}

	public void AddThreat(float threatIncrement)
	{
		ThreatLevel = Mathf.Clamp (ThreatLevel + threatIncrement, 0.0f, Constants.MAX_THREAT_LEVEL);
	}
}
