using UnityEngine;
using System.Collections;

public class RandomizeAnimationStart : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		animation ["Shake_Can"].normalizedTime = Random.Range (0.0f, 1.0f);
	}
}
