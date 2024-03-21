using DG.Tweening;
using UnityEngine;

public class Aberration : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    private Tweener tween;
    private ArrowStateMachines _arrowStateMachine;

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
        _arrowStateMachine = GetComponent<ArrowStateMachines>();
        _arrowStateMachine.KillTweens += KillAllTweens;
        _spriteRenderer.sprite = ArrowManager.Instance.aberrationSprites[Random.Range(0, ArrowManager.Instance.aberrationSprites.Count)];
        tween = _spriteRenderer.DOFade(.5f, .33f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
