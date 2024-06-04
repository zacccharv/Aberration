using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private List<Image> _imgs;
    private float _doScaleScale = 2;

    /// <summary>
    /// 0 reset color, 1 and up combo up colors
    /// </summary>
    public List<Color> colors = new List<Color>(5);

    void Start()
    {
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        if (_textMesh != null) _textMesh.DOFade(.9f, .3f);
        if (_imgs.Count > 0)
        {
            if (ScoreManager.Instance.comboType == 1)
            {
                _imgs[1].color = ScoreManager.Instance.comboMultiplier switch
                {
                    2 => colors[1],
                    4 => colors[2],
                    6 => colors[3],
                    8 => colors[4],
                    _ => colors[4],
                };

                _imgs[0].color = Color.black;
                _doScaleScale = 3;
                ScoreManager.Instance.comboType = 0;
            }
            else if (ScoreManager.Instance.comboType == 2)
            {
                _imgs[1].color = colors[0];
                _imgs[0].color = Color.black;
                _doScaleScale = 4;
                ScoreManager.Instance.comboType = 0;
            }

        }

        transform.DOScale(transform.localScale * _doScaleScale, .5f);
        transform.DOMoveY(transform.position.y + .5f, .5f).OnComplete(() => Destroy(gameObject));
    }
}
