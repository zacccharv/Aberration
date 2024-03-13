using System;
using DG.Tweening;
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

    void Start()
    {
        transform.DOScaleX(transform.localScale.x * 1.15f, .45f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        transform.DOScaleY(transform.localScale.y * 1.15f, .5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDirectionSet(Direction direction)
    {
        inputDirection = direction;
        inputPressed = false;

        Color color = LaneManager.Instance.arrowColors[4];

        if (direction == Direction.Up)
        {
            color = LaneManager.Instance.arrowColors[0];
        }
        else if (direction == Direction.Right)
        {
            color = LaneManager.Instance.arrowColors[1];
        }
        else if (direction == Direction.Down)
        {
            color = LaneManager.Instance.arrowColors[2];
        }
        else if (direction == Direction.Left)
        {
            color = LaneManager.Instance.arrowColors[3];
        }

        color.a = .7f;

        GetComponent<SpriteRenderer>().DOColor(color, .25f);
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