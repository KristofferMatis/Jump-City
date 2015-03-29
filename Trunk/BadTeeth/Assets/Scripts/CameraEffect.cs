using UnityEngine;
using System.Collections;

public class CameraEffect : MonoBehaviour 
{
	public float m_BounceTime;
	float m_BounceTimer;

	public float m_EffectSpeed;

	bool m_IsBouncing;
	bool m_IsBouncingBack;

	float m_TargetZOffset = 0.5f;
	float m_TargetFOV = 58.0f;

	float m_StartZ;

	// Use this for initialization
	void Start () 
	{
		m_StartZ = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_BounceTimer > 0.0f)
		{
			m_BounceTimer -= Time.deltaTime;

			if(m_BounceTimer <= 0.0f)
			{
				m_IsBouncing = true;
			}
		}

		Vector3 newPosition = transform.position;

		if(m_IsBouncing)
		{
			newPosition.z = Mathf.Lerp(newPosition.z, m_StartZ - m_TargetZOffset, m_EffectSpeed * Time.deltaTime);

			camera.fieldOfView = Mathf.Lerp (camera.fieldOfView, m_TargetFOV, m_EffectSpeed * Time.deltaTime);

			if(newPosition.z <= m_StartZ - m_TargetZOffset + 0.1f)
			{
				m_IsBouncing = false;
				m_IsBouncingBack = true;
			}
		}
		else if(m_IsBouncingBack)
		{
			newPosition.z = Mathf.Lerp(newPosition.z, m_StartZ, m_EffectSpeed * Time.deltaTime);

			camera.fieldOfView = Mathf.Lerp (camera.fieldOfView, 60.0f, m_EffectSpeed * Time.deltaTime);

			if(newPosition.z >= m_StartZ - 0.1f)
			{
				m_IsBouncingBack = false;

				newPosition.z = m_StartZ;
				camera.fieldOfView = 60.0f;
			}
		}

		transform.position = newPosition;
	}

	public void DoEffect()
	{
		m_IsBouncing = true;
	}
}
