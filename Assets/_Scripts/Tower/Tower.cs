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
    public GameObject towerBase;
    public Bounds destroyBounds, animationBounds, successBounds;
    public Arrow _arrow_0;

    void OnEnable()
    {
        InputManager_Z.GamePadButtonPressed += OnGamePadPressed;
        InputManager_Z.DirectionPressed += OnDirectionPressed;
        ArrowStateMachines.CurrentDirectionSet += OnDirectionSet;
        ArrowStateMachines.TowerColorChange += ChangeTower;
    }

    void OnDisable()
    {
        InputManager_Z.GamePadButtonPressed -= OnGamePadPressed;
        InputManager_Z.DirectionPressed -= OnDirectionPressed;
        ArrowStateMachines.CurrentDirectionSet -= OnDirectionSet;
        ArrowStateMachines.TowerColorChange -= ChangeTower;
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

        towerBase.transform.DOScaleY(towerBase.transform.localScale.y * 1.15f, .5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        towerBase.transform.DOScaleX(towerBase.transform.localScale.x * 1.15f, .45f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDirectionSet(Direction direction)
    {
        inputDirection = direction;
    }

    private void OnGamePadPressed(Direction directionPressed)
    {
        if (Instance._arrow_0 == null || GameManager.Instance.gameState != GameState.Started)
        {
            return;
        }

        if (_arrow_0.direction == directionPressed && Instance._arrow_0.boundsIndex == 2 && !_arrow_0.inputTriggered)
        {
            SuccessfulInput?.Invoke(ScoreType.SinglePress);
        }
        else if (_arrow_0.direction == directionPressed && Instance._arrow_0.boundsIndex == 2 && _arrow_0.inputTriggered)
        {
            FailedInput?.Invoke();
        }
        else if (_arrow_0.direction != directionPressed && Instance._arrow_0.boundsIndex == 2 && !_arrow_0.inputTriggered)
        {
            FailedInput?.Invoke();
        }
    }

    public void OnDirectionPressed(Direction directionPressed)
    {
        if (Instance._arrow_0 == null || GameManager.Instance.gameState != GameState.Started)
        {
            return;
        }

        if (_arrow_0.direction == directionPressed && Instance._arrow_0.boundsIndex == 2 && !_arrow_0.inputTriggered)
        {
            // Success if not pressed and correct direction
            if (directionPressed == Direction.None)
            {
                SuccessfulInput?.Invoke(ScoreType.Empty);
            }
            else
            {
                SuccessfulInput?.Invoke(ScoreType.SinglePress);
            }
        }
        else if (_arrow_0.direction == directionPressed && Instance._arrow_0.boundsIndex == 2 && _arrow_0.inputTriggered)
        {
            FailedInput?.Invoke();
        }
        else if (_arrow_0.direction != directionPressed && Instance._arrow_0.boundsIndex == 2 && !_arrow_0.inputTriggered)
        {
            FailedInput?.Invoke();
        }
    }

    private void ChangeTower(Direction direction)
    {
        Color color = ArrowManager.Instance.arrowColors[4];

        if (direction == Direction.Up)
        {
            color = ArrowManager.Instance.arrowColors[0];
        }
        else if (direction == Direction.Right)
        {
            color = ArrowManager.Instance.arrowColors[1];
        }
        else if (direction == Direction.Down)
        {
            color = ArrowManager.Instance.arrowColors[2];
        }
        else if (direction == Direction.Left)
        {
            color = ArrowManager.Instance.arrowColors[3];
        }

        GetComponent<SpriteRenderer>().DOColor(color, .25f);
        towerBase.GetComponent<SpriteRenderer>().DOColor(color, .25f);
    }

    public static void TriggerFailedInput()
    {
        FailedInput?.Invoke();
    }
    public static bool IsInBounds(Vector2 position, Bounds bounds)
    {
        if (position.x < (bounds.center.x + bounds.extents.x)
            && position.x > (bounds.center.x - bounds.extents.x)
            && position.y < (bounds.center.y + bounds.extents.y)
            && position.y > (bounds.center.y - bounds.extents.y))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}