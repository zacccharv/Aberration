using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

[RequireComponent(typeof(Arrow))]
public class ArrowStateMachines : MonoBehaviour
{
    public static event DirectionPress CurrentDirectionSet, TowerColorChange;
    public static event Action KillTweens;
    private Arrow _arrow;
    private SpriteRenderer _renderer;
    public ScoreType _scoreType;
    private Tween tween_0, tween_1, tween_2, tween_4;
    private bool _towerColorChanged;

    void OnEnable()
    {
        Tower.SuccessfulInput += Success;
        Tower.FailedInput += Fail;
        KillTweens += KillAllTweens;
    }
    void OnDisable()
    {
        Tower.SuccessfulInput -= Success;
        Tower.FailedInput -= Fail;
        KillTweens -= KillAllTweens;
    }

    void Awake()
    {
        _arrow = GetComponent<Arrow>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Tower.IsInBounds(transform.position, Tower.Instance.destroyBounds) && _arrow.boundsIndex == 2)
        {
            CheckNotPressed();
            KillTweens?.Invoke();

            Destroy(gameObject);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.successBounds) && _arrow.boundsIndex == 1)
        {
            CurrentDirectionSet?.Invoke(_arrow.direction);
            _arrow.boundsIndex = 2;

            int num = (int)_arrow.direction;

            tween_2 = transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo);
            tween_4 = _renderer.DOColor(LaneManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine);

            if (!_towerColorChanged)
            {
                _towerColorChanged = true;
                TowerColorChange?.Invoke(_arrow.direction);
            }
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.animationBounds) && _arrow.boundsIndex == 0)
        {
            _arrow.boundsIndex = 1;
        }
    }

    private void Success(ScoreType scoreType)
    {
        if (_arrow.boundsIndex != 2 || _arrow.isPressed || GameManager.Instance.gameState == GameState.Ended || Tower.Instance._arrow_0 != _arrow) return;

        _scoreType = scoreType;
        _arrow.isPressed = true;

        GameObject popup = Instantiate(Scoring.Instance.scoreNumberPopup, transform.position, Quaternion.identity);

        if (scoreType == ScoreType.Empty)
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"YES");
        }
        else if (scoreType == ScoreType.Direction)
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"+{5 * Scoring.Instance.comboMultiplier}");
        }

        tween_0 = _renderer.DOColor(LaneManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine);
        tween_1 = transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                KillTweens?.Invoke();
                Destroy(gameObject);
            }
        );
    }
    private void Fail()
    {
        if (_arrow.boundsIndex != 2 || _arrow.isPressed || GameManager.Instance.gameState == GameState.Ended || Tower.Instance._arrow_0 != _arrow)
        {
            if (_arrow.isPressed && _arrow.boundsIndex == 2)
            {
                GameObject popup_0 = Instantiate(Scoring.Instance.scoreNumberPopup, transform.position, Quaternion.identity);
                popup_0.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{Scoring.Instance.subtraction}");
                popup_0.GetComponentInChildren<TextMeshProUGUI>().color = LaneManager.Instance.FailNumberColor;

                return;
            }
            else
            { return; }
        }

        _arrow.isPressed = true;

        GameObject popup = Instantiate(Scoring.Instance.scoreNumberPopup, transform.position, Quaternion.identity);
        popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{Scoring.Instance.subtraction}");
        popup.GetComponentInChildren<TextMeshProUGUI>().color = LaneManager.Instance.FailNumberColor;

        tween_0 = _renderer.DOColor(LaneManager.Instance.FailColor, 1).SetEase(Ease.OutSine);
        tween_1 = transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                KillTweens?.Invoke();
                Destroy(gameObject);
            }
        );
    }
    private void CheckNotPressed()
    {
        if (_arrow.boundsIndex != 2 || GameManager.Instance.gameState == GameState.Ended || _arrow.isPressed || Tower.Instance._arrow_0 != _arrow) return;

        Tower.Instance.OnDirectionPressed(Direction.None);
    }

    private void KillAllTweens()
    {
        tween_0.Kill();
        tween_1.Kill();
        tween_2.Kill();
        tween_4.Kill();
    }
}
