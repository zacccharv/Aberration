using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    public static Tower Instance;
    public Direction inputDirection;
    public Bounds destroyBounds, inputBounds;

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
    }

    public void OnDirectionPressed(Direction directionPressed)
    {
        if (inputDirection == directionPressed)
        {
            Debug.Log($"<color=#4fb094>Succesful Input {directionPressed}!</color>");
        }
        else
        {
            Debug.Log($"<color=#ff647d>Unsuccesful Input {directionPressed}.</color>");
        }
    }
}
