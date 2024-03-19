using DG.Tweening;
using UnityEngine;

public class TowerIndicatorAnimation : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _flickerAlpha;

    void Start()
    {
        transform.DOScaleX(transform.localScale.x * 1.1f, .65f).SetEase(Ease.InOutSine).SetDelay(0).SetLoops(-1, LoopType.Yoyo);
        transform.DOScaleY(transform.localScale.y / 1.1f, .7f).SetEase(Ease.InOutSine).SetDelay(.2f).SetLoops(-1, LoopType.Yoyo);
        GetComponent<SpriteRenderer>().DOFade(_flickerAlpha, .9f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
