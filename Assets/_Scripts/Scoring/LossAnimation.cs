using DG.Tweening;
using UnityEngine;

public class LossAnimation : MonoBehaviour
{
    public GameObject GameOverTitle;

    void OnEnable()
    {
        GameManager.GameStateChange += OnLoss;
    }

    void OnDisable()
    {
        GameManager.GameStateChange -= OnLoss;
    }

    private void OnLoss(GameState gameState)
    {
        if (gameState == GameState.Ended)
        {
            GameObject go = Instantiate(GameOverTitle, new(0, 1), Quaternion.identity);

            // go.transform.DOMoveY(1.8f, .3f).SetEase(Ease.OutCubic);
            // go.transform.DOScale(1.2f, .3f).SetEase(Ease.Flash);

            go.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0, .2f).SetEase(Ease.Flash).SetLoops(6, LoopType.Yoyo);
            go.GetComponent<SpriteRenderer>().DOFade(0, .2f).SetEase(Ease.Flash).SetLoops(6, LoopType.Yoyo).OnComplete(() => GameManager.Instance.Invoke(nameof(GameManager.Instance.EndMe), 1.7f));
        }
    }
}
