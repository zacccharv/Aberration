using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputLog))]
public class Tower : MonoBehaviour
{
    public delegate void SuccessDelegate(ScoreType scoreType, InteractionType interactionType);
    public delegate void InputDelegate(ScoreType scoreType, InteractionType interactionType);

    public static event InputDelegate InputEvent;
    public static event Action StartInput;

    public static Tower Instance;
    public GameObject towerBase;
    public Bounds destroyBounds, animationBounds, successBounds;
    [SerializeField] private float _perfectTime;
    [SerializeField] private Color _white;

    private InputLog _inputLog;
    private bool _noPress;

    void OnEnable()
    {
        InputManager_Z.InputStart += OnInputStart;
        InputManager_Z.GamePadButtonPressed += OnGamePadPressed;
        InputManager_Z.DirectionPressed += OnDirectionPressed;
    }

    void OnDisable()
    {
        InputManager_Z.InputStart -= OnInputStart;
        InputManager_Z.GamePadButtonPressed -= OnGamePadPressed;
        InputManager_Z.DirectionPressed -= OnDirectionPressed;
    }

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _inputLog = GetComponent<InputLog>();
    }

    void Start()
    {
        ChangeInteraction(InteractionType.Single, Direction.Null);

        towerBase.transform.DOScale(towerBase.transform.localScale.x * 1.15f, .5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }


    /// <summary>
    /// Direction press handler
    /// </summary>
    /// <param name="directionPressed"> Direction Pressed (up, down, left, right, none)</param>
    /// <param name="interactionType"> Interaction (single press, double press, etc...)</param>
    private void OnGamePadPressed(InputAction.CallbackContext callbackContext, Direction directionPressed, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] == null || GameManager.Instance.gameState != GameState.Started)
        {
            return;
        }

        ScoreType scoreType = ScoreType.Empty;
        IArrowStates arrowStates = ArrowManager.Instance.interactableArrows[0].GetComponent<IArrowStates>();
        bool perfect = false;

        if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            // Success if not pressed and correct direction
            if (directionPressed == Direction.None)
            {
                scoreType = ScoreType.Empty;
                perfect = false;

                InputEvent?.Invoke(scoreType, interactionType);
            }
            else if (interactionType == InteractionType.Single)
            {
                scoreType = ScoreType.Success;

                if (arrowStates is SingleArrow)
                {
                    if ((arrowStates as SingleArrow).perfectInputTimer < ((arrowStates as SingleArrow).perfectInputTime - GameManager.Instance.speedShrink))
                        perfect = true;
                    else
                    {
                        ScoreManager.Instance.comboCount = 0;
                        perfect = false;
                    }
                }

                InputEvent?.Invoke(scoreType, interactionType);

                StartCoroutine(PressTimeOut());
            }
            else
            {
                // Check for perfect input start, if no reset combo
                // doesn't work for single arrow

                if (!arrowStates.PerfectInputStart)
                {
                    ScoreManager.Instance.comboCount = 0;
                    perfect = false;
                }
                else
                    perfect = true;

                scoreType = ScoreType.Success;
                InputEvent?.Invoke(scoreType, interactionType);
                StartCoroutine(PressTimeOut());
            }

        }
        else if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            scoreType = ScoreType.Fail;
            perfect = false;
            InputEvent?.Invoke(scoreType, interactionType);
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction != directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            scoreType = ScoreType.Fail;
            perfect = false;

            InputEvent?.Invoke(scoreType, interactionType);
        }

        _inputLog.AddToLog(callbackContext, interactionType, directionPressed, ArrowManager.Instance.interactableArrows[0].direction, ArrowManager.Instance.interactableArrows[0].interactionType, scoreType, perfect);
    }

    public void OnDirectionPressed(InputAction.CallbackContext callbackContext, Direction directionPressed, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] == null || GameManager.Instance.gameState != GameState.Started)
        {
            return;
        }

        ScoreType scoreType = ScoreType.Empty;
        IArrowStates arrowStates = ArrowManager.Instance.interactableArrows[0].GetComponent<IArrowStates>();
        bool perfect = false;

        if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            // Success if not pressed and correct direction
            if (directionPressed == Direction.None)
            {
                scoreType = ScoreType.Empty;
                perfect = false;

                InputEvent?.Invoke(scoreType, interactionType);
            }
            else if (interactionType == InteractionType.Single)
            {
                scoreType = ScoreType.Success;

                if (ArrowManager.Instance.interactableArrows[0].TryGetComponent(out SingleArrow singleArrow))
                {
                    if (singleArrow.perfectInputTimer < singleArrow.perfectInputTime)
                    {
                        ScoreManager.Instance.comboCount = 0;
                        perfect = false;
                    }
                    else
                        perfect = true;
                }

                InputEvent?.Invoke(scoreType, interactionType);

                StartCoroutine(PressTimeOut());
            }
            else
            {
                // Check for perfect input start, if no reset combo
                // doesn't work for single arrow

                if (!arrowStates.PerfectInputStart)
                {
                    ScoreManager.Instance.comboCount = 0;
                    perfect = false;
                }
                else
                    perfect = true;

                scoreType = ScoreType.Success;
                InputEvent?.Invoke(scoreType, interactionType);
                StartCoroutine(PressTimeOut());
            }

        }
        else if (ArrowManager.Instance.interactableArrows[0].direction == directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            scoreType = ScoreType.Fail;
            perfect = false;
            InputEvent?.Invoke(scoreType, interactionType);
        }
        else if (ArrowManager.Instance.interactableArrows[0].direction != directionPressed && ArrowManager.Instance.interactableArrows[0].boundsIndex == 2 && !ArrowManager.Instance.interactableArrows[0].inputTriggered && !_noPress)
        {
            scoreType = ScoreType.Fail;
            perfect = false;

            InputEvent?.Invoke(scoreType, interactionType);
        }

        _inputLog.AddToLog(callbackContext, interactionType, directionPressed, ArrowManager.Instance.interactableArrows[0].direction, ArrowManager.Instance.interactableArrows[0].interactionType, scoreType, perfect);
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

            sequence.Append(GetComponent<SpriteRenderer>().DOColor(_white, _perfectTime / 4).SetEase(Ease.InOutQuad));
            sequence.Join(transform.DOScale(3.5f, _perfectTime / 4).SetEase(Ease.Flash));
            sequence.AppendInterval(_perfectTime / 4 * 2);
            sequence.Join(transform.DOScale(4, _perfectTime / 4).SetEase(Ease.Flash));
            sequence.Join(GetComponent<SpriteRenderer>().DOColor(ChangeTowerColor(direction), _perfectTime / 4).SetEase(Ease.Flash));
        }
        else if (interactionType == InteractionType.Double)
        {
            sequence.Play();

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

        tower.OnDirectionPressed(default, Direction.None, InteractionType.NoPress);
    }
    public IEnumerator PressTimeOut()
    {
        _noPress = true;
        yield return new WaitForSeconds(.15f);

        _noPress = false;
        yield return null;
    }
}