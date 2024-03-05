using UnityEngine;
using UnityEngine.InputSystem;

public delegate void DirectionPress(Direction direction);

public class InputMan : MonoBehaviour
{
    // TODO Create testing script to press inputs for me
    public static event DirectionPress DirectionPressed;

    public void UpPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DirectionPressed?.Invoke(Direction.Up);
        }
    }

    public void RightPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DirectionPressed?.Invoke(Direction.Right);
        }
    }

    public void DownPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DirectionPressed?.Invoke(Direction.Down);
        }
    }

    public void LeftPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DirectionPressed?.Invoke(Direction.Left);
        }
    }
}
