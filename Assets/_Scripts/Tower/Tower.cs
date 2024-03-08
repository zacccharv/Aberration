using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public static event Action SuccesfulInput, FailedInput;

    public static Tower Instance;
    public Direction inputDirection;
    public Bounds destroyBounds, inputBounds, successFailBounds;
    public bool inputPressed;

    void OnEnable()
    {
        InputMan.DirectionPressed += OnDirectionPressed;
        ArrowMovement.CurrentDirectionSet += OnDirectionSet;
    }
    void OnDisable()
    {
        InputMan.DirectionPressed -= OnDirectionPressed;
        ArrowMovement.CurrentDirectionSet -= OnDirectionSet;
    }

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDirectionSet(Direction direction)
    {
        inputDirection = direction;
        inputPressed = false;
    }

    public void OnDirectionPressed(Direction directionPressed)
    {
        if (inputPressed)
        {
            return;
        }

        if (inputDirection == directionPressed)
        {
            SuccesfulInput?.Invoke();
            Debug.Log($"<color=#4fb094>Succesful Input {directionPressed}!</color>");
        }
        else
        {
            FailedInput?.Invoke();
            Debug.Log($"<color=#ff647d>Unsuccesful Input {directionPressed}.</color>");
        }

        inputPressed = true;
    }
}
