using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum InteractionType
{
    Single,
    Double,
    Long,
    NoPress,
    FailedDouble
}

public delegate void DirectionPress(Direction direction, InteractionType interactionType);
public delegate void GamePadButtonPress(Direction direction, InteractionType interactionType);
public delegate void UIInputPress(InputType inputType);

public class InputManager_Z : MonoBehaviour
{
    [SerializeField] private float _inputDelay;
    [SerializeField] private int _pressCount;
    [SerializeField] private float defaultHoldTime;

    // TODO make clear current long arrow when 2 in bounds
    public static event DirectionPress DirectionPressed;
    public static event GamePadButtonPress GamePadButtonPressed;
    public static event UIInputPress UIInputPressed;

    public void ArrowPressed(InputAction.CallbackContext context)
    {
        Direction direction = Direction.None;
        InteractionType interactionType = InteractionType.NoPress;

        if (context.action.name.Contains("Up"))
        {
            direction = Direction.Up;
        }
        else if (context.action.name.Contains("Right"))
        {
            direction = Direction.Right;
        }
        else if (context.action.name.Contains("Down"))
        {
            direction = Direction.Down;
        }
        else if (context.action.name.Contains("Left"))
        {
            direction = Direction.Left;
        }

        if (context.interaction is HoldInteraction)
        {
            if (context.performed)
            {
                DirectionPressed?.Invoke(direction, InteractionType.Long);
                interactionType = InteractionType.Long;
            }
        }
        else if (context.interaction is MultiTapInteraction)
        {
            if (context.started)
            {
                DirectionPressed?.Invoke(direction, InteractionType.Single);
                interactionType = InteractionType.Single;
            }
            else if (context.performed)
            {
                DirectionPressed?.Invoke(direction, InteractionType.Double);
                interactionType = InteractionType.Double;
            }
            else if (context.canceled && !context.performed)
            {
                interactionType = InteractionType.FailedDouble;
            }
        }
        else if (context.interaction is PressInteraction)
        {
            if (context.performed)
            {
                DirectionPressed?.Invoke(direction, InteractionType.Single);
                Debug.Log(interactionType);
                interactionType = InteractionType.Single;
            }
        }

        if (interactionType != InteractionType.NoPress)
        {
        }
    }
    public void GamePadPressed(InputAction.CallbackContext context)
    {
        Direction direction = Direction.None;

        if (context.control.name.Contains("Up"))
        {
            direction = Direction.Up;
        }
        else if (context.control.name.Contains("Right"))
        {
            direction = Direction.Right;
        }
        else if (context.control.name.Contains("Down"))
        {
            direction = Direction.Down;
        }
        else if (context.control.name.Contains("Left"))
        {
            direction = Direction.Left;
        }

        if (context.interaction is HoldInteraction)
        {
            if (context.performed)
            {
                GamePadButtonPressed?.Invoke(direction, InteractionType.Long);
            }
        }
        else if (context.interaction is MultiTapInteraction)
        {
            if (context.performed)
            {
                GamePadButtonPressed?.Invoke(direction, InteractionType.Double);
            }
            if (context.canceled)
            {
                GamePadButtonPressed?.Invoke(direction, InteractionType.FailedDouble);
            }
            else if (context.started)
            {
                GamePadButtonPressed?.Invoke(direction, InteractionType.Single);
            }
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
}
