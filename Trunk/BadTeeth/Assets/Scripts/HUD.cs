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
        Rect StaminaBar = new Rect(i_StaminaBarPosition.x * Screen.width, i_StaminaBarPosition.y * Screen.height,
                                   (i_StaminaBarScale.x * Screen.width) * ((float)m_Stamina.stamina / Constants.MAX_STAMINA), i_StaminaBarScale.y * Screen.height);
        GUI.DrawTexture(StaminaBar, m_StaminaTextures[2]);
    }
}
