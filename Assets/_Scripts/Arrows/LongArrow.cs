using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LongArrow : MonoBehaviour, IArrowStates
{
    public event KillTweens KillAllTweens;

    [field: SerializeField] public Arrow Arrow { get; set; }

    [field: SerializeField] public List<Tween> Tweens { get; set; } = new();
    [SerializeField] private List<SpriteRenderer> renderers = new();

    private Vector2 _offsetPosition;


    void OnEnable()
    {
        Tower.SuccessfulInput += Success;
        Tower.FailedInput += Fail;
        KillAllTweens += ArrowManager.KillAllTweens;
    }
    void OnDisable()
    {
        Tower.SuccessfulInput -= Success;
        Tower.FailedInput -= Fail;
        KillAllTweens -= ArrowManager.KillAllTweens;
    }

    void Awake()
    {
        Arrow = GetComponent<Arrow>();
    }

    void Update()
    {
        _offsetPosition = CollisionPosition(Arrow.vectorDirection, transform.position);

        if (Tower.IsInBounds(_offsetPosition, Tower.Instance.destroyBounds))
        {
            Tower.CheckNotPressed(Arrow, Tower.Instance);
            KillAllTweens?.Invoke(Tweens);
            Destroy(gameObject);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.successBounds) && Arrow.boundsIndex == 1)
        {
            int num = (int)Arrow.direction;

            Tweens.Add(transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo));

            foreach (var item in renderers)
            {
                Tweens.Add(item.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));
            }

            Arrow.boundsIndex = 2;

            Tower.SetDirection(Arrow.direction, Tower.Instance);
            Tower.TriggerTowerChange(Arrow.direction, Tower.Instance);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.animationBounds) && Arrow.boundsIndex == 0)
        {
            Arrow.boundsIndex = 1;
        }

        // TODO arrow animation to fold arrow in instead
        static Vector2 CollisionPosition(Vector2 vectorDirection, Vector2 position)
        {
            Vector2 vector2 = default;

            switch (vectorDirection)
            {
                case var val when val == Vector2.right:
                    vector2 = val;
                    break;
                case var val when val == Vector2.up:
                    vector2 = val;
                    break;
                case var val when val == Vector2.left:
                    vector2 = val;
                    break;
                case var val when val == Vector2.down:
                    vector2 = val;
                    break;
                default:
                    break;
            }

            return position - vector2;
        }
    }

    public void Success(ScoreType scoreType)
    {
        if (Tower.Instance._arrow_0 != Arrow || GameManager.Instance.gameState == GameState.Ended) return;

        foreach (var item in renderers)
        {
            Tweens.Add(item.DOColor(ArrowManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine));
        }

        Tweens.Add(transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                KillAllTweens?.Invoke(Tweens);
                Destroy(gameObject);
            }
        ));

        GameObject popup = Instantiate(ScoreManager.Instance.scoreNumberPopup, transform.position, Quaternion.identity);

        if (scoreType == ScoreType.Empty)
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"YES");
        }
        else if (scoreType == ScoreType.SinglePress)
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"+{5 * ScoreManager.Instance.comboMultiplier}");
        }
    }

    public void Fail()
    {
        if (Tower.Instance._arrow_0 != Arrow || GameManager.Instance.gameState == GameState.Ended) return;

        if (!Arrow.inputTriggered)
        {
            Arrow.inputTriggered = true;

            foreach (var item in renderers)
            {
                Tweens.Add(item.DOColor(ArrowManager.Instance.FailColor, 1).SetEase(Ease.OutSine));
            }

            Tweens.Add(transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        KillAllTweens?.Invoke(Tweens);
                        Destroy(gameObject);
                    }
                ));
        }

        SFXCollection.Instance.PlaySound(SFXType.Fail);

        GameObject popup = Instantiate(ScoreManager.Instance.scoreNumberPopup, transform.position, Quaternion.identity);
        popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{ScoreManager.Instance.subtraction}");
        popup.GetComponentInChildren<TextMeshProUGUI>().color = ArrowManager.Instance.FailNumberColor;
    }
}
