using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StagePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    void Start()
    {
        transform.position = new(0, 0);
        _textMesh.DOFade(1, 1);
        transform.DOScale(transform.localScale * 1.5f, 2f);
        transform.DOMoveY(1f, 2f).SetEase(Ease.OutSine).OnComplete(() => Destroy(gameObject));
    }
}
