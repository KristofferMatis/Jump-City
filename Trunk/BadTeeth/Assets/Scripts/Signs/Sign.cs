using UnityEngine;
using System.Collections;

public class Sign : MonoBehaviour 
{
    public enum SignType
    {
		Poster,//smallest
        Hanging,//smaller        
        Welcome,//bigger
		BillBoard,//biggest
        Count
    };

    public SignType m_SignType = SignType.Hanging;

	static string[] MAIN_TEXTURES_FILE_PATHS = new string[]                 { "Textures/SignTextures/Poster/Main",   "Textures/SignTextures/Hanging/Main",    "Textures/SignTextures/Welcome/Main",   "Textures/SignTextures/BillBoard/Main"};
	static string[] GRAFITI_TEXTURES_FILE_PATHS = new string[]              { "Textures/SignTextures/Poster/Grafiti","Textures/SignTextures/Hanging/Grafiti", "Textures/SignTextures/Welcome/Grafiti","Textures/SignTextures/BillBoard/Grafiti"};
	static string[] GREYSCALE_GRAFITI_TEXTURES_FILE_PATHS = new string[]    { "Textures/SignTextures/Poster/Mask",   "Textures/SignTextures/Hanging/Mask",    "Textures/SignTextures/Welcome/Mask",    "Textures/SignTextures/BillBoard/Mask"   };

    static Texture[][] m_MainTextures = new Texture[(int)SignType.Count][];
    static Texture[][] m_GrafitiTextures = new Texture[(int)SignType.Count][];
    static Texture[][] m_GrafitiTexturesGrayScale = new Texture[(int)SignType.Count][];

	public Material i_SourceMaterial;
    Material m_Material;

	public bool m_IsFinished = false;

	static float[] TIME_TO_PAINT = new float[]{ 1.0f, 1.5f, 2.0f, 3.0f };
    public float m_PaintedTime = 0.0f;

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

		gameObject.tag = "Sign";
	}
}
