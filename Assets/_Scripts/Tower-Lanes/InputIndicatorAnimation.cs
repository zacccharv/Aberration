using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InputIndicatorAnimation : MonoBehaviour
{
    void Start()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.Append(GetComponent<SpriteRenderer>().material.DOFloat(0.1f, "_Circle_size", 2));
        sequence.AppendInterval(1);
        sequence.Append(GetComponent<SpriteRenderer>().material.DOFloat(0, "_Circle_size", 1)).
        OnComplete(() => Destroy(gameObject));
    }
}
