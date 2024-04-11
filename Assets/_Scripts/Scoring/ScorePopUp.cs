using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private Image _img;

    void Start()
    {
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        if (_textMesh != null) _textMesh.DOFade(.9f, .3f);
        if (_img != null) _img.DOFade(.9f, .3f);

        transform.DOScale(transform.localScale * 2f, .5f);
        transform.DOMoveY(transform.position.y + .5f, .5f).OnComplete(() => Destroy(gameObject));
    }
}
