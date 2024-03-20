using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Aberration : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] ArrowStateMachines _arrowStateMachine;
    private Tweener tween;

    void OnEnable()
    {
        _arrowStateMachine.KillTweens += KillAllTweens;
    }
    void OnDisable()
    {
        _arrowStateMachine.KillTweens -= KillAllTweens;
    }

    private void KillAllTweens()
    {
        tween.Kill();
    }

    void Awake()
    {
        _spriteRenderer.sprite = ArrowManager.Instance.aberrationSprites[Random.Range(0, ArrowManager.Instance.aberrationSprites.Count)];
        tween = _spriteRenderer.DOFade(.5f, .33f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
