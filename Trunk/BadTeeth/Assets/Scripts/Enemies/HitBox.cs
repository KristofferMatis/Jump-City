﻿using UnityEngine;
using System.Collections;

public class HitBox: MonoBehaviour 
{
	IHitBoxListener m_Listener;

	public void RegisterListener(IHitBoxListener listener)
	{
		m_Listener = listener;
	}

	void OnTriggerEnter(Collider otherCollider)
	{
		m_Listener.OnHitBoxEnter (otherCollider);
	}
}
