using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
    const string STAMINA_TEXTURES_PATH = "Textures/Stamina";
    Texture[] m_StaminaTextures;

	Stamina m_Stamina;

    Vector2 i_StaminaBarPosition = new Vector2(0.004f, 0.01f);
	Vector2 i_StaminaBarScale = new Vector2(0.30f, 0.03f);

	// Use this for initialization
	void Start () 
    {
        m_StaminaTextures = Resources.LoadAll<Texture>(STAMINA_TEXTURES_PATH);
		m_Stamina = getPlayer.Instance.gameObject.GetComponent<Stamina> ();
	}

    void OnGUI()
    {
		int increment = 10;
        Rect StaminaBar = new Rect(i_StaminaBarPosition.x * Screen.width, i_StaminaBarPosition.y * Screen.height, 
		                           (i_StaminaBarScale.x * Screen.width)/(Constants.MAX_STAMINA/ increment), i_StaminaBarScale.y * Screen.height);

		for(int i = -increment; i < Constants.MAX_STAMINA; i = i + increment)
        {
			if(i == -increment)
            {
                GUI.DrawTexture(StaminaBar, m_StaminaTextures[0]);
            }
            else if (i == 0)
            {
                GUI.DrawTexture(StaminaBar, m_StaminaTextures[1]);
            }
			else if (i + increment >= Constants.MAX_STAMINA)
            {
                GUI.DrawTexture(StaminaBar, m_StaminaTextures[4]);
            }
			else if (i + (increment * 2) >= Constants.MAX_STAMINA && m_Stamina.stamina + increment >= Constants.MAX_STAMINA)
			{
				GUI.DrawTexture(StaminaBar, m_StaminaTextures[3]);
			}
			else if(i < m_Stamina.stamina)
            {
                GUI.DrawTexture(StaminaBar, m_StaminaTextures[2]);
            }
            else
            {
                GUI.DrawTexture(StaminaBar, m_StaminaTextures[5]);
            }

			StaminaBar.position += new Vector2(StaminaBar.width, 0.0f);
        }
    }
}
