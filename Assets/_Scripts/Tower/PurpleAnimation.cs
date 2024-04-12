using DG.Tweening;
using UnityEngine;

public class PurpleAnimation : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _flickerAlpha;

    void Start()
    {
        transform.DOScale(transform.localScale.x * 1.1f, .65f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        GetComponent<SpriteRenderer>().DOFade(_flickerAlpha, .9f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
