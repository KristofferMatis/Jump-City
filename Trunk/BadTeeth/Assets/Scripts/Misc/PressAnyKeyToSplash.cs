﻿using UnityEngine;
using System.Collections;

public class PressAnyKeyToSplash : MonoBehaviour 
{	
	// Update is called once per frame
	void Update () 
	{
		if(InputManager.getJumpDown ())
		{
			Application.LoadLevel("InputSelect");
		}
	}
}
