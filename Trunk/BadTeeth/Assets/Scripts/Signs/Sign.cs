using UnityEngine;
using System.Collections;

public class Sign : MonoBehaviour 
{
    public enum SignType
    {
        Hanging,
        Poster,
        BillBoard,
        Welcome
    };

    public SignType m_SignType = SignType.Hanging;

    string[] MAIN_TEXTURE_FILE_PATH = new string[] { "" };
    string[] TEXTUREs_FILE_PATHs = new string[] { "" };

    public Texture m_MainTexture;

    Texture[] m_GrafitiTextures;
    Texture[] m_GrafitiTexturesGrayScale;

	public Material i_SourceMaterial;
    Material m_Material;

	// Use this for initialization
	void Start () 
    {
	    //TODO:load textures
		m_Material = new Material(i_SourceMaterial);
        m_Material.SetTexture("_MainTexture", m_MainTexture);


        gameObject.renderer.material = m_Material;
	}
}
