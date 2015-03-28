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

    Texture m_MainTexture;

    Texture[] m_GrafitiTextures;
    Texture[] m_GrafitiTexturesGrayScale;

    const string SPURCE_MATERIAL_FILE_PATH = "";
    Material m_Material;

	// Use this for initialization
	void Start () 
    {
	    //TODO:load textures
        m_Material = new Material(Resources.Load<Material>(SPURCE_MATERIAL_FILE_PATH));
        //TODO:set Textures

        gameObject.renderer.materials[0] = m_Material;
	}
}
