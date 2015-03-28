using UnityEngine;
using System.Collections;

public class Sign : MonoBehaviour 
{
    public enum SignType
    {
        Hanging,
        Poster,
        BillBoard,
        Welcome,
        Count
    };

    public SignType m_SignType = SignType.Hanging;

    static string[] MAIN_TEXTURES_FILE_PATHS = new string[]                 { "Textures/SignTextures/Hanging/Main",   "Textures/SignTextures/Poster/Main",    "Textures/SignTextures/BillBoard/Main",   "Textures/SignTextures/Welcome/Main"};
    static string[] GRAFITI_TEXTURES_FILE_PATHS = new string[]              { "Textures/SignTextures/Hanging/Grafiti","Textures/SignTextures/Poster/Grafiti", "Textures/SignTextures/BillBoard/Grafiti","Textures/SignTextures/Welcome/Grafiti"};
    static string[] GREYSCALE_GRAFITI_TEXTURES_FILE_PATHS = new string[]    { "Textures/SignTextures/Hanging/Mask",   "Textures/SignTextures/Poster/MAsk",    "Textures/SignTextures/BillBoard/Mask",   "Textures/SignTextures/Welcome/Mask"};

    static Texture[][] m_MainTextures = new Texture[(int)SignType.Count][];
    static Texture[][] m_GrafitiTextures = new Texture[(int)SignType.Count][];
    static Texture[][] m_GrafitiTexturesGrayScale = new Texture[(int)SignType.Count][];

	public Material i_SourceMaterial;
    Material m_Material;

	// Use this for initialization
	void Start () 
    {
        m_Material = new Material(i_SourceMaterial);

	    if(m_MainTextures[(int)m_SignType] == null)
        {
            m_MainTextures[(int)m_SignType] = Resources.LoadAll<Texture>(MAIN_TEXTURES_FILE_PATHS[(int)m_SignType]);
        }
        m_Material.SetTexture("_MainTexture", m_MainTextures[(int)m_SignType][Random.Range(0, m_MainTextures[(int)m_SignType].Length)]);

        if (m_GrafitiTextures[(int)m_SignType] == null)
        {
            m_GrafitiTextures[(int)m_SignType] = Resources.LoadAll<Texture>(GRAFITI_TEXTURES_FILE_PATHS[(int)m_SignType]);
        }
        m_Material.SetTexture("_GrafitiTexture", m_GrafitiTextures[(int)m_SignType][Random.Range(0, m_GrafitiTextures[(int)m_SignType].Length)]);

        if (m_GrafitiTexturesGrayScale[(int)m_SignType] == null)
        {
            m_GrafitiTexturesGrayScale[(int)m_SignType] = Resources.LoadAll<Texture>(GREYSCALE_GRAFITI_TEXTURES_FILE_PATHS[(int)m_SignType]);
        }
        m_Material.SetTexture("_GrafitiGreyScale", m_GrafitiTexturesGrayScale[(int)m_SignType][Random.Range(0, m_GrafitiTexturesGrayScale[(int)m_SignType].Length)]);

       gameObject.renderer.material = m_Material;
	}
}
