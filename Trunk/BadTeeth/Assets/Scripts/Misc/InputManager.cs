using UnityEngine;
using System.Collections;

using GamepadInput;

public static class InputManager
{
    public static bool IsUsingKeyboard = true;

    #region public inputs
    public static bool getJump()
    {
        if (IsUsingKeyboard)
            return getKeyboardJump();
        else
            return getGamePadJump();
    }

    public static bool getJumpUp()
    {
        if (IsUsingKeyboard)
            return getKeyboardJumpUp();
        else
            return getGamePadJumpUp();
    }

    public static bool getJumpDown()
    {
        if (IsUsingKeyboard)
            return getKeyboardJumpDown();
        else
            return getGamePadJumpDown();
    }

    public static bool getSprayPaint()
    {
        if (IsUsingKeyboard)
            return getKeyboardSprayPaint();
        else
            return getGamePadSprayPaint();
    }

    public static bool getSprayPaintUp()
    {
        if (IsUsingKeyboard)
            return getKeyboardSprayPaintUp();
        else
            return getGamePadSprayPaintUp();
    }

    public static bool getSprayPaintDown()
    {
        if (IsUsingKeyboard)
            return getKeyboardSprayPaintDown();
        else
            return getGamePadSprayPaintDown();
    }

    public static bool getDrink()
    {
        if (IsUsingKeyboard)
            return getKeyboardDrink();
        else
            return getGamePadDrink();
    }

    public static bool getDrinkUp()
    {
        if (IsUsingKeyboard)
            return getKeyboardDrinkUp();
        else
            return getGamePadDrinkUp();
    }

    public static bool getDrinkDown()
    {
        if (IsUsingKeyboard)
            return getKeyboardDrinkDown();
        else
            return getGamePadDrinkDown();
    }

    public static float getMovement()
    {
        if (IsUsingKeyboard)
            return getKeyboardMovement();
        else
            return getGamePadMovement();
    }
    #endregion
    #region Gamepad
    static bool getGamePadJump()
    {
        return GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any);
    }

    static bool getGamePadJumpUp()
    {
        return GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Any);
    }

    static bool getGamePadJumpDown()
    {
        return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any);
    }

    static bool getGamePadSprayPaint()
    {
        return GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any);
    }

    static bool getGamePadSprayPaintUp()
    {
        return GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Any);
    }

    static bool getGamePadSprayPaintDown()
    {
        return GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any);
    }

    static bool getGamePadDrink()
    {
        return GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Any);
    }

    static bool getGamePadDrinkUp()
    {
        return GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Any);
    }

    static bool getGamePadDrinkDown()
    {
        return GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Any);
    }

    static float getGamePadMovement()
    {
        return GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any).x;
    }
    #endregion
    #region Keyboard
    static bool getKeyboardJump()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow);
    }

    static bool getKeyboardJumpUp()
    {
        return Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow);
    }

    static bool getKeyboardJumpDown()
    {
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
    }

    static bool getKeyboardSprayPaint()
    {
        return Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.Alpha2);
    }

    static bool getKeyboardSprayPaintUp()
    {
        return Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.Alpha2);
    }

    static bool getKeyboardSprayPaintDown()
    {
        return Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Alpha2);
    }

    static bool getKeyboardDrink()
    {
        return Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.Alpha1);
    }

    static bool getKeyboardDrinkUp()
    {
        return Input.GetKeyUp(KeyCode.L) || Input.GetKeyUp(KeyCode.Alpha1);
    }

    static bool getKeyboardDrinkDown()
    {
        return Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Alpha1);
    }

    static float getKeyboardMovement()
    {
        float input = 0.0f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            input += 1.0f;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            input -= 1.0f;
        }
        return input;
    }
    #endregion
}
