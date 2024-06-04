using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputLog))]
public class Tower : MonoBehaviour
{
    public delegate void InputDelegate(ScoreType scoreType, InteractionType interactionType);
    public delegate void TowerChangeDelegate(Direction direction, InteractionType interactionType);

    public static event InputDelegate InputEvent;
    public static event TowerChangeDelegate TowerChangeEvent;
    public static event Action StartInput;

    public static Tower Instance;
    public Bounds destroyBounds, animationBounds, successBounds;
    [SerializeField] private float _perfectTime;
    [SerializeField] private Color _tooEarly, _justRight;

    [SerializeField] private float _bounceMin, _bounceMax;
    private InputLog _inputLog;
    private bool _noPress;

    void OnEnable()
    {
        InputManager_Z.InputStart += OnInputStart;
        InputManager_Z.DirectionPressed += OnDirectionPressed;
    }

    void OnDisable()
    {
        InputManager_Z.InputStart -= OnInputStart;
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
    }

    /// <summary>
    /// Direction press handler
    /// </summary>
    /// <param name="directionPressed"> Direction Pressed (up, down, left, right, none)</param>
    /// <param name="interactionType"> Interaction (single press, double press, etc...)</param>
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
                    if (singleArrow.perfectInputTimer < singleArrow.perfectInputTime - GameManager.Instance.speedShrink)
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
        // TODO: Needs a refactor
        Color color = ArrowManager.Instance.FailColor;

        if (direction == Direction.Up)
        {
            color = ArrowManager.Instance.SuccessColor;
        }
        else if (direction == Direction.Right)
        {
            color = ArrowManager.Instance.SuccessColor;
        }
        else if (direction == Direction.Down)
        {
            color = ArrowManager.Instance.SuccessColor;
        }
        else if (direction == Direction.Left)
        {
            color = ArrowManager.Instance.SuccessColor;
        }
        else if (direction == Direction.None)
        {
            color = ArrowManager.Instance.SuccessColor;
        }

        return color;
    }

    private void ChangeInteraction(InteractionType interactionType, Direction direction)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        _perfectTime = (1 - GameManager.Instance.speedShrink) / 2;

        if (interactionType == InteractionType.Single)
        {
            sequence.Play();

            sequence.Append(GetComponent<SpriteRenderer>().DOColor(_tooEarly, _perfectTime / 8).SetEase(Ease.Flash));
            sequence.Join(transform.DOScale(_bounceMin, _perfectTime / 8).SetEase(Ease.Flash));
            sequence.AppendInterval(.4f - GameManager.Instance.speedShrink);
            sequence.Append(transform.DOScale(_bounceMax, _perfectTime / 8).SetEase(Ease.Flash));
            sequence.Join(GetComponent<SpriteRenderer>().DOColor(_justRight, _perfectTime / 8).SetEase(Ease.Flash));

            TowerChangeEvent?.Invoke(direction, interactionType);
        }
        else if (interactionType == InteractionType.Double)
        {
            sequence.Play();

            if (GameManager.Instance.speedShrink < .2)
            {
                sequence.Append(GetComponent<SpriteRenderer>().DOColor(_tooEarly, _perfectTime / 8).SetEase(Ease.Flash));
                sequence.Join(transform.DOScale(_bounceMin, _perfectTime / 8).SetEase(Ease.Flash));
                sequence.AppendInterval(.2f - GameManager.Instance.speedShrink - (_perfectTime / 8));
                sequence.Append(transform.DOScale(_bounceMax, _perfectTime / 8).SetEase(Ease.Flash));
                sequence.Join(GetComponent<SpriteRenderer>().DOColor(_justRight, _perfectTime / 8).SetEase(Ease.Flash));
            }
            else
            {
                sequence.Append(GetComponent<SpriteRenderer>().DOColor(_justRight, _perfectTime / 8).SetEase(Ease.Flash));
            }

            TowerChangeEvent?.Invoke(direction, interactionType);
        }
        else if (interactionType == InteractionType.Long)
        {
            sequence.Play();

            sequence.Append(GetComponent<SpriteRenderer>().DOColor(_tooEarly, _perfectTime / 8).SetEase(Ease.Flash));
            sequence.Join(transform.DOScale(_bounceMin, _perfectTime / 8).SetEase(Ease.InOutSine));
            _ = sequence.AppendInterval(.7f - GameManager.Instance.speedShrink - (_perfectTime / 8));
            sequence.Append(transform.DOScale(_bounceMax, _perfectTime / 8).SetEase(Ease.InOutSine));
            sequence.Join(GetComponent<SpriteRenderer>().DOColor(_justRight, _perfectTime / 8).SetEase(Ease.InOutSine));

            TowerChangeEvent?.Invoke(direction, interactionType);
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