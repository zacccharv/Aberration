using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Aberration : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    private List<Tween> tween = new();
    private ArrowStateMachines _arrowStateMachine;

    void OnDisable()
    {
        _arrowStateMachine.KillAllTweens -= ArrowManager.KillAllTweens;
    }
    void Awake()
    {
        _arrowStateMachine = GetComponent<ArrowStateMachines>();
        _arrowStateMachine.KillAllTweens += ArrowManager.KillAllTweens;
        _spriteRenderer.sprite = ArrowManager.Instance.aberrationSprites[Random.Range(0, ArrowManager.Instance.aberrationSprites.Count)];
        tween.Add(_spriteRenderer.DOFade(.5f, .33f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo));
    }
}
