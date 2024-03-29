using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void DirectionPress(Direction direction);
public delegate void GamePadButtonPress(Direction direction);

public class InputMan : MonoBehaviour
{
    private bool _cantTouchThis;
    [SerializeField] private float _inputDelay;

    public static event DirectionPress DirectionPressed;
    public static event GamePadButtonPress GamePadButtonPressed;
    public static event UIInputPress UIInputPressed;

    public void UpPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            DirectionPressed?.Invoke(Direction.Up);
        }
    }

    public void GamePadNorthPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            GamePadButtonPressed?.Invoke(Direction.Up);
        }
    }
    public void RightPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            DirectionPressed?.Invoke(Direction.Right);
        }
    }

    public void GamePadEastPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            GamePadButtonPressed?.Invoke(Direction.Right);
        }
    }

    public void DownPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            DirectionPressed?.Invoke(Direction.Down);
        }
    }

    public void GamePadSouthPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            GamePadButtonPressed?.Invoke(Direction.Down);
        }
    }

    public void LeftPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            DirectionPressed?.Invoke(Direction.Left);
        }
    }

    public void GamePadWestPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            GamePadButtonPressed?.Invoke(Direction.Left);
        }
    }

    public void ConfirmPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            UIInputPressed?.Invoke(InputType.Confirm);
        }
    }
    public void EscPressed(InputAction.CallbackContext context)
    {
        if (context.started && !_cantTouchThis)
        {
            StartCoroutine(CantTouchThis(_inputDelay));
            UIInputPressed?.Invoke(InputType.Esc);
        }
    }

    public IEnumerator CantTouchThis(float delay)
    {
        _cantTouchThis = true;

        yield return new WaitForSeconds(delay);

        _cantTouchThis = false;

        yield return null;
    }
}
