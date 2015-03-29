using UnityEngine;
using System.Collections;

public class InputSelect : MonoBehaviour 
{
	public string m_nextLevel = "Game";
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
