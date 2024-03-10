using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public delegate void SuccessDelegate(ScoreType scoreType);
    public static event SuccessDelegate SuccessfulInput;
    public static event Action FailedInput;

    public static Tower Instance;
    public Direction inputDirection;
    public GameObject arrow;
    public Bounds destroyBounds, animationBounds, successBounds, failBounds;
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

        if (inputDirection == directionPressed && InSuccessBounds())
        {
            // TODO fix early press (mostly fixed)
            if (directionPressed == Direction.None)
            {
                SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
                SuccessfulInput?.Invoke(ScoreType.Empty);
            }
            else
            {
                SFXCollection.Instance.PlaySound(SFXType.Success);
                SuccessfulInput?.Invoke(ScoreType.Direction);

            }

            inputPressed = true;
            Debug.Log($"<color=#4fb094>Succesful Input {directionPressed}!</color>");
        }
        else
        {
            FailedInput?.Invoke();
            SFXCollection.Instance.PlaySound(SFXType.Fail);

            Debug.Log($"<color=#ff647d>Unsuccesful Input {directionPressed}.</color>");
            inputPressed = true;
        }


    }

    private bool InSuccessBounds()
    {
        bool value = false;

        if (arrow != null)
        {
            value = arrow.GetComponent<Arrow>().inSuccessBounds;
        }

        if (!value || arrow == null)
        {
            FailedInput?.Invoke();
            SFXCollection.Instance.PlaySound(SFXType.Fail);
            return value;
        }

        return value;
    }
}
