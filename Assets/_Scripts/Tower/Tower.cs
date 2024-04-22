using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public delegate void SuccessDelegate(ScoreType scoreType, InteractionType interactionType);
    public delegate void InputDelegate(ScoreType scoreType, InteractionType interactionType);

    public static event InputDelegate InputEvent;
    public static event Action StartInput;

    public static Tower Instance;
    public GameObject towerBase;
    public Bounds destroyBounds, animationBounds, successBounds;
    private bool _noPress;
    [SerializeField] private float _perfectTime;
    [SerializeField] private Color _white;

    void OnEnable()
    {
        InputManager_Z.GamePadButtonPressed += OnGamePadPressed;
        InputManager_Z.DirectionPressed += OnDirectionPressed;
        InputManager_Z.InputStart += OnInputStart;
    }

    void OnDisable()
    {
        InputManager_Z.GamePadButtonPressed -= OnGamePadPressed;
        InputManager_Z.DirectionPressed -= OnDirectionPressed;
        InputManager_Z.InputStart -= OnInputStart;
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
        ChangeInteraction(InteractionType.Single, Direction.Null);

        towerBase.transform.DOScale(towerBase.transform.localScale.x * 1.15f, .5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnGamePadPressed(Direction directionPressed, InteractionType interactionType)
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
                InputEvent?.Invoke(ScoreType.Empty, interactionType);
            }
            else if (interactionType == InteractionType.Single)
            {
                InputEvent?.Invoke(ScoreType.Press, interactionType);
                StartCoroutine(PressTimeOut());
            }
            else
            {
                // Check for perfect input start, if no reset combo
                if (!ArrowManager.Instance.interactableArrows[0].GetComponent<IArrowStates>().PerfectInputStart) ScoreManager.Instance.comboCount = 0;

                InputEvent?.Invoke(ScoreType.Press, interactionType);
                StartCoroutine(PressTimeOut());
            }
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            InputEvent?.Invoke(ScoreType.Fail, interactionType);
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction != directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            InputEvent?.Invoke(ScoreType.Fail, interactionType);
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
                InputEvent?.Invoke(ScoreType.Empty, interactionType);
            }
            else if (interactionType == InteractionType.Single)
            {
                InputEvent?.Invoke(ScoreType.Press, interactionType);
                StartCoroutine(PressTimeOut());
            }
            else
            {
                // Check for perfect input start, if no reset combo
                if (!ArrowManager.Instance.interactableArrows[0].GetComponent<IArrowStates>().PerfectInputStart) ScoreManager.Instance.comboCount = 0;

                InputEvent?.Invoke(ScoreType.Press, interactionType);
                StartCoroutine(PressTimeOut());
            }
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            InputEvent?.Invoke(ScoreType.Fail, interactionType);
            StartCoroutine(PressTimeOut());
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction != directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            InputEvent?.Invoke(ScoreType.Fail, interactionType);
            StartCoroutine(PressTimeOut());
        }
    }

    public void OnInputStart()
    {
        StartInput?.Invoke();
    }

    public static void TriggerFailedInput(InteractionType interactionType)
    {
        InputEvent?.Invoke(ScoreType.Fail, interactionType);
    }
    private Color ChangeTowerColor(Direction direction)
    {
        Color color = ArrowManager.Instance.arrowColors[5];

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

        return color;
    }

    private void ChangeInteraction(InteractionType interactionType, Direction direction)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        if (interactionType == InteractionType.Single)
        {
            sequence.Play();

            // sequence.Append(transform.DOScale(transform.localScale.x * 4, _perfectTime).SetEase(Ease.InOutQuad));
            sequence.Append(GetComponent<SpriteRenderer>().DOColor(_white, _perfectTime / 4).SetEase(Ease.InOutQuad));
            sequence.Join(transform.DOScale(3.5f, _perfectTime / 4).SetEase(Ease.Flash));
            sequence.AppendInterval(_perfectTime / 4 * 2);
            sequence.Join(transform.DOScale(4, _perfectTime / 4).SetEase(Ease.Flash));
            sequence.Join(GetComponent<SpriteRenderer>().DOColor(ChangeTowerColor(direction), _perfectTime / 4).SetEase(Ease.Flash));

        }
        else if (interactionType == InteractionType.Double)
        {
            sequence.Play();

            // sequence.Append(transform.DOScale(transform.localScale.x * 4, _perfectTime / 2).SetEase(Ease.Linear));            sequence.Append(GetComponent<SpriteRenderer>().DOFade(.2f, _perfectTime / 4).SetEase(Ease.InOutQuad));
            sequence.Append(GetComponent<SpriteRenderer>().DOColor(_white, _perfectTime / 2 / 4).SetEase(Ease.InOutQuad));
            sequence.Join(transform.DOScale(3.5f, _perfectTime / 2 / 4).SetEase(Ease.InOutQuad));
            sequence.AppendInterval(_perfectTime / 2 / 4);
            sequence.Join(transform.DOScale(4, _perfectTime / 2 / 4).SetEase(Ease.InOutQuad));
            sequence.Join(GetComponent<SpriteRenderer>().DOColor(ChangeTowerColor(direction), _perfectTime / 2 / 4).SetEase(Ease.InOutQuad));

            sequence.SetLoops(2, LoopType.Restart);
        }
        else if (interactionType == InteractionType.Long)
        {
            sequence.Play();

            sequence.Append(GetComponent<SpriteRenderer>().DOColor(_white, _perfectTime / 2 / 3).SetEase(Ease.InOutSine));
            sequence.Join(transform.DOScale(3.5f, _perfectTime / 8).SetEase(Ease.InOutSine));
            sequence.AppendInterval(_perfectTime * 2 / 4 * 2);
            sequence.Append(transform.DOScale(4, _perfectTime * 2 / 4).SetEase(Ease.InOutSine));
            sequence.Join(GetComponent<SpriteRenderer>().DOColor(ChangeTowerColor(direction), _perfectTime * 2 / 4).SetEase(Ease.InOutSine));
        }
    }

    public static void TriggerTowerChange(Direction direction, InteractionType interactionType, Tower tower)
    {
        tower.ChangeInteraction(interactionType, direction);
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