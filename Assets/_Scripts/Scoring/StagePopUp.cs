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
        _textMesh.DOFade(1, 2);
        transform.DOScale(transform.localScale * 1.25f, 2f);
        transform.DOMoveY(transform.position.y + .15f, 3f).SetEase(Ease.OutSine).OnComplete(() => Destroy(gameObject));
    }
}
