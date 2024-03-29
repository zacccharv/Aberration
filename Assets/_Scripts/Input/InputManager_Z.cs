using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum InteractionType
{
    Single,
    Double,
    Long,
    NoPress
}

public delegate void DirectionPress(Direction direction, InteractionType interactionType);
public delegate void LongDirectionPress(Direction direction, bool canceled, double duration, float defaultDuration);
public delegate void GamePadButtonPress(Direction direction);
public delegate void UIInputPress(InputType inputType);

public class InputManager_Z : MonoBehaviour
{
    [SerializeField] private float _inputDelay;
    [SerializeField] private int _pressCount;
    [SerializeField] private float defaultHoldTime;

    public static event DirectionPress DirectionPressed;
    public static event LongDirectionPress LongDirectionPressed;

    // TODO finish adding interaction to press events
    public static event GamePadButtonPress GamePadButtonPressed;
    public static event UIInputPress UIInputPressed;

    public void UpPressed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.performed)
            {
                DirectionPressed?.Invoke(Direction.Up, InteractionType.Long);
            }
        }
        else if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                Debug.Log(context.interaction);
                DirectionPressed?.Invoke(Direction.Up, InteractionType.Single);
            }
        }
    }
    public void GamePadNorthPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.control);
            GamePadButtonPressed?.Invoke(Direction.Up);
        }
    }

    public void RightPressed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.performed)
            {
                DirectionPressed?.Invoke(Direction.Right, InteractionType.Long);
            }
        }
        else if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                Debug.Log(context.interaction);
                DirectionPressed?.Invoke(Direction.Right, InteractionType.Single);
            }
        }
    }

    public void GamePadEastPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.control);
            GamePadButtonPressed?.Invoke(Direction.Right);
        }
    }

    public void DownPressed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.performed)
            {
                DirectionPressed?.Invoke(Direction.Down, InteractionType.Long);
            }
        }
        else if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                Debug.Log(context.interaction);
                DirectionPressed?.Invoke(Direction.Down, InteractionType.Single);
            }
        }
    }

    public void GamePadSouthPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.control);
            GamePadButtonPressed?.Invoke(Direction.Down);
        }
    }

    public void LeftPressed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.performed)
            {
                DirectionPressed?.Invoke(Direction.Left, InteractionType.Long);
            }
        }
        else if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                Debug.Log(context.interaction);
                DirectionPressed?.Invoke(Direction.Left, InteractionType.Single);
            }
        }
    }

    public void GamePadWestPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.control);
            GamePadButtonPressed?.Invoke(Direction.Left);
        }
    }

    public void ConfirmPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIInputPressed?.Invoke(InputType.Confirm);
        }
    }
    public void EscPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
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
