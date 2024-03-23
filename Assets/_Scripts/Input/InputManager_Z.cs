using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InteractionType
{
    Single,
    Double,
    Long
}

public delegate void DirectionPress(Direction direction);
public delegate void GamePadButtonPress(Direction direction);
public delegate void UIInputPress(InputType inputType);

public class InputManager_Z : MonoBehaviour
{
    [SerializeField] private float _inputDelay;
    [SerializeField] private int _pressCount;

    public static event DirectionPress DirectionPressed;
    public static event GamePadButtonPress GamePadButtonPressed;
    public static event UIInputPress UIInputPressed;

    public void UpPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.interaction);
            DirectionPressed?.Invoke(Direction.Up);
        }
    }

    public void GamePadNorthPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.control);
            GamePadButtonPressed?.Invoke(Direction.Up);
        }
    }
    public void RightPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.control);
            DirectionPressed?.Invoke(Direction.Right);
        }
    }

    public void GamePadEastPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.control);
            GamePadButtonPressed?.Invoke(Direction.Right);
        }
    }

    public void DownPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.control);
            DirectionPressed?.Invoke(Direction.Down);
        }
    }

    public void GamePadSouthPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.control);
            GamePadButtonPressed?.Invoke(Direction.Down);
        }
    }

    public void LeftPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.control);
            DirectionPressed?.Invoke(Direction.Left);
        }
    }

    public void GamePadWestPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.control);
            GamePadButtonPressed?.Invoke(Direction.Left);
        }
    }

    public void ConfirmPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UIInputPressed?.Invoke(InputType.Confirm);
        }
    }
    public void EscPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UIInputPressed?.Invoke(InputType.Esc);
        }
    }

    // public IEnumerator CantTouchThis(float delay)
    // {
    //     _cantTouchThis = true;

    //     yield return new WaitForSeconds(delay);

    //     _cantTouchThis = false;

    //     yield return null;
    // }
}
