﻿using UnityEngine;
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

	static int numSigns = 0;
	static int taggedSigned = 0;

    public SignType m_SignType = SignType.Hanging;

	static string[] MAIN_TEXTURES_FILE_PATHS = new string[]                 { "Textures/SignTextures/Poster/Main",   "Textures/SignTextures/Hanging/Main",    "Textures/SignTextures/Welcome/Main",   "Textures/SignTextures/BillBoard/Main"};
	static string[] GRAFITI_TEXTURES_FILE_PATHS = new string[]              { "Textures/SignTextures/Poster/Grafiti","Textures/SignTextures/Hanging/Grafiti", "Textures/SignTextures/Welcome/Grafiti","Textures/SignTextures/BillBoard/Grafiti"};
	static string[] GREYSCALE_GRAFITI_TEXTURES_FILE_PATHS = new string[]    { "Textures/SignTextures/Poster/Mask",   "Textures/SignTextures/Hanging/Mask",    "Textures/SignTextures/Welcome/Mask",    "Textures/SignTextures/BillBoard/Mask"   };

    static Texture[][] m_MainTextures = new Texture[(int)SignType.Count][];
    static Texture[][] m_GrafitiTextures = new Texture[(int)SignType.Count][];
    static Texture[][] m_GrafitiTexturesGrayScale = new Texture[(int)SignType.Count][];

	public Material i_SourceMaterial;
    Material m_Material;

    public Transform i_PlayerMount;

	public bool m_IsFinished = false;

	static float[] TIME_TO_PAINT = new float[]{ 1.0f, 1.5f, 2.0f, 3.0f };
    float m_PaintedTime = 0.0f;

	public float NormalizedPaintedTime
	{
		get { return m_PaintedTime / TIME_TO_PAINT[(int)m_SignType]; }
	}

	public float PaintedTime
	{
		get{ return m_PaintedTime; }
		set
		{
            if (value > TIME_TO_PAINT[(int)m_SignType])
            {
                m_PaintedTime = TIME_TO_PAINT[(int)m_SignType];
                m_IsFinished = true;
            }
            else
            {
                m_PaintedTime = value;
            }


			m_Material.SetFloat("_PercentPainted", m_PaintedTime/TIME_TO_PAINT[(int)m_SignType]);

			if(m_PaintedTime < TIME_TO_PAINT[(int)m_SignType])
			{
				updateLine();
			}
			else
			{
				if(m_LineRenderer != null)
				{
					taggedSigned++;
					if (taggedSigned >= numSigns) 
					{
						Application.LoadLevel("Win");
					}
					Destroy(m_LineRenderer);
					m_LineRenderer = null;
				}
			}
		}
	}

    LineRenderer m_LineRenderer;
    public Vector3 i_ProgressStart = new Vector3(-1.0f, 1.0f, 0.0f);
	public Vector3 i_ProgressEnd = new Vector3(1.0f, 1.0f, 0.0f);
	
	const string STAMINA_TEXTURES_PATH = "Textures/Stamina";
	Texture[] m_StaminaTextures;
	
	Vector2 i_StaminaBarPosition = new Vector2(0.004f, 0.01f);
	Vector2 i_StaminaBarScale = new Vector2(0.30f, 0.03f);

	// Use this for initialization
	void Start () 
    {
		numSigns++;

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

        m_LineRenderer = gameObject.GetComponent<LineRenderer>();
		m_LineRenderer.SetPosition(0, transform.position + i_ProgressStart);
		
		PaintedTime = m_PaintedTime;

		m_StaminaTextures = Resources.LoadAll<Texture>(STAMINA_TEXTURES_PATH);
	}

	void updateLine()
	{
        m_LineRenderer.SetPosition(1, (transform.position + i_ProgressStart) + ((i_ProgressEnd - i_ProgressStart) * (m_PaintedTime / TIME_TO_PAINT[(int)m_SignType])));
	}

	void OnGUI()
	{
		Color previousColor = GUI.color;
		GUI.color = Color.red;

		Rect StaminaBar = new Rect((1.0f - i_StaminaBarPosition.x) * Screen.width - (i_StaminaBarScale.x * Screen.width) * (1.0f - (float)taggedSigned / (float)numSigns), i_StaminaBarPosition.y * Screen.height,
		                           (i_StaminaBarScale.x * Screen.width) * (1.0f - (float)taggedSigned / (float)numSigns), i_StaminaBarScale.y * Screen.height);
		GUI.DrawTexture(StaminaBar, m_StaminaTextures[2]);

		GUI.color = previousColor;
	}
}
