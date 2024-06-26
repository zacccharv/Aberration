using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum InteractionType
{
    Single,
    Double,
    Long,
    NoPress,
    FailedDouble,
    Nil
}

public delegate void DirectionPress(InputAction.CallbackContext callbackContext, Direction direction, InteractionType interactionType);
public delegate void UIInputPress(InputType inputType);

public class InputManager_Z : MonoBehaviour
{
    public static event DirectionPress DirectionPressed;
    public static event Action InputStart;
    public static event UIInputPress UIInputPressed;

    public void ArrowPressed(InputAction.CallbackContext context)
    {
        Direction direction = Direction.None;

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
            if (context.started)
            {
                InputStart?.Invoke();
            }

            if (context.performed)
            {
                DirectionPressed?.Invoke(context, direction, InteractionType.Long);
            }
        }
        else if (context.interaction is MultiTapInteraction)
        {
            if (context.started)
            {
                InputStart?.Invoke();
                DirectionPressed?.Invoke(context, direction, InteractionType.Single);
            }
            else if (context.performed)
            {
                DirectionPressed?.Invoke(context, direction, InteractionType.Double);
            }
        }
        else if (context.interaction is PressInteraction)
        {
            if (context.performed)
            {
                DirectionPressed?.Invoke(context, direction, InteractionType.Single);
            }
        }
        else
        {
            DirectionPressed?.Invoke(context, direction, InteractionType.Nil);
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
