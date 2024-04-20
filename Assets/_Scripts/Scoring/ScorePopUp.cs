using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private Image _img;
    private float _doScaleScale = 2;

    void Start()
    {
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        if (_textMesh != null) _textMesh.DOFade(.9f, .3f);
        if (_img != null)
        {
            _img.DOFade(.9f, .3f);

            if (ScoreManager.Instance.comboType == 1)
            {
                _img.color = Color.yellow;
                _doScaleScale = 3;
                ScoreManager.Instance.comboType = 0;
            }
            else if (ScoreManager.Instance.comboType == 2)
            {
                _img.color = Color.magenta;
                _doScaleScale = 4;
                ScoreManager.Instance.comboType = 0;
            }
        }

        transform.DOScale(transform.localScale * _doScaleScale, .5f);
        transform.DOMoveY(transform.position.y + .5f, .5f).OnComplete(() => Destroy(gameObject));
    }
}
