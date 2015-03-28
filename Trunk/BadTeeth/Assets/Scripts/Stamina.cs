using UnityEngine;
using System.Collections;

public class Stamina : MonoBehaviour 
{
	public HUD i_HUD;
	int m_Stamina = Constants.MAX_STAMINA;
	public int stamina
	{
		get{ return m_Stamina; }
		set{ m_Stamina = value; }
	}
	
	// Update is called once per frame
	void Update () 
	{
		i_HUD.stamina = stamina;
	}
}
