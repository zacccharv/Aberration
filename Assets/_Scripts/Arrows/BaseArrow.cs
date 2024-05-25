using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BaseArrow : MonoBehaviour
{
    public void FailState(Arrow arrow, SpriteRenderer spriteRenderer, List<Tween> tweens)
    {
        if (ArrowManager.Instance.interactableArrows[0] != arrow || GameManager.Instance.gameState == GameState.Ended) return;

        if (!arrow.inputTriggered)
        {
            arrow.inputTriggered = true;

            tweens.Add(spriteRenderer.DOColor(ArrowManager.Instance.FailColor, 1).SetEase(Ease.OutSine));
            tweens.Add(transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        KillAllTweens(tweens);
                        Destroy(gameObject);
                    }
                ));
        }

        SFXCollection.Instance.PlaySound(SFXType.Fail);

        SpawnPopUp(ScoreType.Fail, false);
    }

    public void FailState(Arrow arrow, List<SpriteRenderer> spriteRenderers, List<Tween> tweens)
    {
        if (ArrowManager.Instance.interactableArrows[0] != arrow || GameManager.Instance.gameState == GameState.Ended) return;

        if (!arrow.inputTriggered)
        {
            arrow.inputTriggered = true;

            foreach (var item in spriteRenderers)
            {
                tweens.Add(item.DOColor(ArrowManager.Instance.FailColor, 1).SetEase(Ease.OutSine));
            }
            tweens.Add(transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        KillAllTweens(tweens);
                        Destroy(gameObject);
                    }
                ));
        }

        SFXCollection.Instance.PlaySound(SFXType.Fail);

        SpawnPopUp(ScoreType.Fail, false);
    }

    public void SpawnPopUp(ScoreType scoreType, bool success)
    {
        int popUpNum = ScoreManager.Instance.comboMultiplier == 1 ? 0 : ScoreManager.Instance.comboMultiplier / 2;
        popUpNum = Mathf.Min(popUpNum, ScoreManager.Instance.succesfulNumberPopup.Count - 1);

        if (success)
        {
            if (scoreType == ScoreType.Empty)
            {
                GameObject popup = Instantiate(ScoreManager.Instance.scoreNumberPopup, transform.position, Quaternion.identity);
                popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"YES");
            }
            else if (scoreType == ScoreType.Success)
            {
                Instantiate(ScoreManager.Instance.succesfulNumberPopup[popUpNum], transform.position, Quaternion.identity);
                // popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"+{5 * ScoreManager.Instance.comboMultiplier}");
            }
        }
        else if (!success)
        {
            GameObject popup = Instantiate(ScoreManager.Instance.scoreNumberPopup, transform.position, Quaternion.identity);
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{ScoreManager.Instance.subtraction}");
            popup.GetComponentInChildren<TextMeshProUGUI>().color = ArrowManager.Instance.FailNumberColor;
        }
    }

    public void KillAllTweens(List<Tween> tweens)
    {
        foreach (var item in tweens)
        {
            item.Kill();
        }
    }
}