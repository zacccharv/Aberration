using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    void Start()
    {
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        _textMesh.DOFade(.9f, .3f);
        transform.DOScale(transform.localScale * 2f, .5f);
        transform.DOMoveY(transform.position.y + .5f, .5f).OnComplete(() => Destroy(gameObject));
    }
}
