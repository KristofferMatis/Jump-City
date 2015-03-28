using UnityEngine;
using System.Collections;

public class getPlayer : MonoBehaviour 
{
    public static getPlayer Instance;
	void Awake () 
    {
        Instance = this;
	}
}
