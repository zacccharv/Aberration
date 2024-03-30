using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public delegate void SuccessDelegate(ScoreType scoreType, InteractionType interactionType);
    public delegate void FailDelegate(InteractionType interactionType);
    public static event SuccessDelegate SuccessfulInput;
    public static event FailDelegate FailedInput;

    public static Tower Instance;
    public GameObject towerBase;
    public Bounds destroyBounds, animationBounds, successBounds;
    private bool _noPress;

    void OnEnable()
    {
        InputManager_Z.GamePadButtonPressed += OnGamePadPressed;
        InputManager_Z.DirectionPressed += OnDirectionPressed;
    }

    void OnDisable()
    {
        InputManager_Z.GamePadButtonPressed -= OnGamePadPressed;
        InputManager_Z.DirectionPressed -= OnDirectionPressed;
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

    private void OnGamePadPressed(Direction directionPressed, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] == null || GameManager.Instance.gameState != GameState.Started)
        {
            return;
        }

        if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered)
        {
            // Success if not pressed and correct direction
            if (directionPressed == Direction.None)
            {
                SuccessfulInput?.Invoke(ScoreType.Empty, InteractionType.NoPress);
            }
            else
            {
                SuccessfulInput?.Invoke(ScoreType.SinglePress, interactionType);
            }
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && ArrowManager.Instance.interactableArrows[0].inputTriggered)
        {
            FailedInput?.Invoke(interactionType);
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction != directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered)
        {
            FailedInput?.Invoke(interactionType);
        }
    }

    public void OnDirectionPressed(Direction directionPressed, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] == null || GameManager.Instance.gameState != GameState.Started)
        {
            return;
        }

        if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            // Success if not pressed and correct direction
            if (directionPressed == Direction.None)
            {
                SuccessfulInput?.Invoke(ScoreType.Empty, interactionType);
            }
            else
            {
                SuccessfulInput?.Invoke(ScoreType.SinglePress, interactionType);
                StartCoroutine(PressTimeOut());
            }
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            FailedInput?.Invoke(interactionType);
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction != directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            FailedInput?.Invoke(interactionType);
        }
    }

    public void ChangeTower(Direction direction)
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
        else if (direction == Direction.None)
        {
            color = ArrowManager.Instance.arrowColors[4];
        }

        GetComponent<SpriteRenderer>().DOColor(color, .25f);
        towerBase.GetComponent<SpriteRenderer>().DOColor(color, .25f);
    }

    public static void TriggerTowerChange(Direction direction, Tower tower)
    {
        tower.ChangeTower(direction);
    }

    public static void TriggerFailedInput(InteractionType interactionType)
    {
        FailedInput?.Invoke(interactionType);
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
    public static void CheckNotPressed(Arrow arrow, Tower tower)
    {
        if (arrow.boundsIndex != 2 || arrow.inputTriggered || ArrowManager.Instance.interactableArrows[0] != arrow || GameManager.Instance.gameState == GameState.Ended) return;

        tower.OnDirectionPressed(Direction.None, InteractionType.NoPress);
    }
    public IEnumerator PressTimeOut()
    {
        _noPress = true;
        yield return new WaitForSeconds(.15f);

        _noPress = false;
        yield return null;
    }
}