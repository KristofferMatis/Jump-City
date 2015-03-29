using UnityEngine;
using System.Collections;

public class InputSelect : MonoBehaviour 
{
	AudioSource m_Source;
	public AudioClip clip;

	public string m_nextLevel = "Game";

	void Start()
	{
		m_Source = gameObject.AddComponent<AudioSource> ();
		m_Source.clip = clip;
		m_Source.loop = true;
		m_Source.Play();

	}

	// Update is called once per frame
	void Update () 
	{
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            InputManager.IsUsingKeyboard = true;
            Application.LoadLevel(m_nextLevel);
        }

        if(GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.Any))
        {
            InputManager.IsUsingKeyboard = false;
            Application.LoadLevel(m_nextLevel);
        }
	}
}
