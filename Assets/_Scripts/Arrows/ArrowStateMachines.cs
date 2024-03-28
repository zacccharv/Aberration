using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Arrow))]
public class ArrowStateMachines : MonoBehaviour
{
    public event KillTweens KillAllTweens;
    [SerializeField] private List<Tween> _tweens = new();
    [SerializeField] private float _doublePressResetTime = .5f;
    private float _inputTimer;
    public SpriteRenderer m_renderer, numberRenderer;
    public ScoreType _scoreType;
    private Arrow _arrow;
    [SerializeField] private Vector2 position;

    void OnEnable()
    {
        Tower.SuccessfulInput += Success;
        Tower.FailedInput += Fail;
        KillAllTweens += ArrowManager.KillAllTweens;
    }
    void OnDisable()
    {
        Tower.SuccessfulInput -= Success;
        Tower.FailedInput -= Fail;
        KillAllTweens -= ArrowManager.KillAllTweens;
    }

    void Start()
    {
        _arrow = GetComponent<Arrow>();
    }

    void Update()
    {
        if (Tower.IsInBounds(transform.position, Tower.Instance.destroyBounds) && _arrow.boundsIndex == 2)
        {
            Tower.CheckNotPressed(_arrow, Tower.Instance);

            KillAllTweens?.Invoke(_tweens);
            Destroy(gameObject);
        }
        else if (_arrow.interactionType == InteractionType.Long && _arrow.boundsIndex == 2)
        {
            position = CollisionPosition(_arrow.vectorDirection, transform.position);

            if (Tower.IsInBounds(position, Tower.Instance.destroyBounds))
            {
                Tower.CheckNotPressed(_arrow, Tower.Instance);
                KillAllTweens?.Invoke(_tweens);
                Destroy(gameObject);
            }

            static Vector2 CollisionPosition(Vector2 vectorDirection, Vector2 position)
            {
                Vector2 vector2 = default;

                switch (vectorDirection)
                {
                    case var val when val == Vector2.right:
                        vector2 = val;
                        break;
                    case var val when val == Vector2.up:
                        vector2 = val;
                        break;
                    case var val when val == Vector2.left:
                        vector2 = val;
                        break;
                    case var val when val == Vector2.down:
                        vector2 = val;
                        break;
                    default:
                        break;
                }

                return position - vector2;
            }
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.successBounds) && _arrow.boundsIndex == 1)
        {
            int num = (int)_arrow.direction;

            _tweens.Add(transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo));
            _tweens.Add(m_renderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));
            if (numberRenderer != null) _tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));

            _arrow.boundsIndex = 2;

            Tower.SetDirection(_arrow.direction, Tower.Instance);
            Tower.TriggerTowerChange(_arrow.direction, Tower.Instance);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.animationBounds) && _arrow.boundsIndex == 0)
        {
            _arrow.boundsIndex = 1;
        }

        if (_arrow.pressCount == 1)
        {
            _inputTimer += Time.deltaTime;
        }
    }

    private void Success(ScoreType scoreType)
    {
        if (Tower.Instance._arrow_0 != _arrow || GameManager.Instance.gameState == GameState.Ended) return;

        _scoreType = scoreType;

        if (_arrow.interactionType == InteractionType.Double)
        {
            _arrow.pressCount++;

            if (_arrow.pressCount == 2 && _inputTimer > _doublePressResetTime)
            {
                Tower.TriggerFailedInput();
                return;
            }
            else if (_arrow.pressCount == 2 && _inputTimer < _doublePressResetTime)
            {
                _arrow.inputTriggered = true;
                SFXCollection.Instance.PlaySound(SFXType.Success);
            }
            else
            {
                SFXCollection.Instance.PlaySound(SFXType.QuietSuccess);
                return;
            }
        }
        else if (_arrow.interactionType == InteractionType.Single)
        {
            _arrow.inputTriggered = true;
            SFXCollection.Instance.PlaySound(SFXType.Success);
        }
        else if (_arrow.interactionType == InteractionType.NoPress)
        {
            _arrow.inputTriggered = true;
            SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
        }

        _tweens.Add(m_renderer.DOColor(ArrowManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine));
        if (numberRenderer != null) _tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine));
        _tweens.Add(transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                KillAllTweens?.Invoke(_tweens);
                Destroy(gameObject);
            }
        ));

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

        if (!_arrow.inputTriggered)
        {
            _arrow.inputTriggered = true;

            _tweens.Add(m_renderer.DOColor(ArrowManager.Instance.FailColor, 1).SetEase(Ease.OutSine));
            if (numberRenderer != null) _tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.FailColor, 1).SetEase(Ease.OutSine));

            _tweens.Add(transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        KillAllTweens?.Invoke(_tweens);
                        Destroy(gameObject);
                    }
                ));
        }

        SFXCollection.Instance.PlaySound(SFXType.Fail);

        GameObject popup = Instantiate(ScoreManager.Instance.scoreNumberPopup, transform.position, Quaternion.identity);
        popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{ScoreManager.Instance.subtraction}");
        popup.GetComponentInChildren<TextMeshProUGUI>().color = ArrowManager.Instance.FailNumberColor;
    }
}
