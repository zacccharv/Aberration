using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

[RequireComponent(typeof(Arrow))]
public class ArrowStateMachines : MonoBehaviour
{
    public static event DirectionPress CurrentDirectionSet, TowerColorChange;
    public event Action KillTweens;
    private Arrow _arrow;
    public SpriteRenderer m_renderer, numberRenderer;
    public ScoreType _scoreType;
    private Tween tween_0, tween_1, tween_2, tween_4, tween_5;

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

    void Start()
    {
        _arrow = GetComponent<Arrow>();
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
            int num = (int)_arrow.direction;

            tween_2 = transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo);
            tween_4 = m_renderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine);
            tween_5 = numberRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine);

            _arrow.boundsIndex = 2;

            CurrentDirectionSet?.Invoke(_arrow.direction);
            TowerColorChange?.Invoke(_arrow.direction);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.animationBounds) && _arrow.boundsIndex == 0)
        {
            _arrow.boundsIndex = 1;
        }
    }

    private void Success(ScoreType scoreType)
    {
        if (Tower.Instance._arrow_0 != _arrow || GameManager.Instance.gameState == GameState.Ended) return;

        tween_0 = m_renderer.DOColor(ArrowManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine);
        tween_5 = numberRenderer.DOColor(ArrowManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine);
        tween_1 = transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                KillTweens?.Invoke();
                Destroy(gameObject);
            }
        );

        _scoreType = scoreType;
        _arrow.isPressed = true;

        GameObject popup = Instantiate(ScoreManager.Instance.scoreNumberPopup, transform.position, Quaternion.identity);

        if (scoreType == ScoreType.Empty)
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"YES");
        }
        else if (scoreType == ScoreType.SinglePress)
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"+{5 * ScoreManager.Instance.comboMultiplier}");
        }

    }
    private void Fail()
    {
        if (Tower.Instance._arrow_0 != _arrow || GameManager.Instance.gameState == GameState.Ended) return;

        GameObject popup = Instantiate(ScoreManager.Instance.scoreNumberPopup, transform.position, Quaternion.identity);
        popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{ScoreManager.Instance.subtraction}");
        popup.GetComponentInChildren<TextMeshProUGUI>().color = ArrowManager.Instance.FailNumberColor;

        if (!_arrow.isPressed)
        {
            _arrow.isPressed = true;

            tween_0 = m_renderer.DOColor(ArrowManager.Instance.FailColor, 1).SetEase(Ease.OutSine);
            tween_5 = numberRenderer.DOColor(ArrowManager.Instance.FailColor, 1).SetEase(Ease.OutSine);
            tween_1 = transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        KillTweens?.Invoke();
                        Destroy(gameObject);
                    }
                );
        }
    }
    private void CheckNotPressed()
    {
        if (_arrow.boundsIndex != 2 || _arrow.isPressed || Tower.Instance._arrow_0 != _arrow || GameManager.Instance.gameState == GameState.Ended) return;

        Tower.Instance.OnDirectionPressed(Direction.None);
    }

    private void KillAllTweens()
    {
        tween_0.Kill();
        tween_1.Kill();
        tween_2.Kill();
        tween_4.Kill();
        tween_5.Kill();
    }
}
