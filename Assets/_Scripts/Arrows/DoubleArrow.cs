using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoubleArrow : BaseArrow, IArrowStates
{
    public Arrow Arrow { get; set; }
    public List<Tween> Tweens { get; set; } = new();

    public SpriteRenderer spriteRenderer, numberRenderer;
    [SerializeField] private int _pressCount;
    private bool _noPress;

    void OnEnable()
    {
        Tower.SuccessfulInput += SuccessState;
        Tower.FailedInput += FailState;
    }
    void OnDisable()
    {
        Tower.SuccessfulInput -= SuccessState;
        Tower.FailedInput -= FailState;
    }

    void Start()
    {
        Arrow = GetComponent<Arrow>();
        spriteRenderer.color = ArrowManager.Instance.arrowColors[(int)Arrow.direction];
    }

    void Update()
    {
        UpdateBounds();
    }

    public void UpdateBounds()
    {
        if (Tower.IsInBounds(transform.position, Tower.Instance.destroyBounds) && Arrow.boundsIndex == 2)
        {
            Tower.CheckNotPressed(Arrow, Tower.Instance);

            KillAllTweens(Tweens);
            Destroy(gameObject);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.successBounds) && Arrow.boundsIndex == 1)
        {
            int num = (int)Arrow.direction;
            Arrow.boundsIndex = 2;

            Tower.TriggerTowerChange(Arrow.direction, Tower.Instance);

            Tweens.Add(transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo));
            Tweens.Add(spriteRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));
            Tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));

        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.animationBounds) && Arrow.boundsIndex == 0)
        {
            Arrow.boundsIndex = 1;
        }
    }

    public void SuccessState(ScoreType scoreType, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] != Arrow || GameManager.Instance.gameState == GameState.Ended || scoreType == ScoreType.Empty) return;

        if (interactionType == InteractionType.Double && !Arrow.inputTriggered)
        {
            Arrow.inputTriggered = true;
            StartCoroutine(PressTimeOut());
        }
        else if (interactionType == InteractionType.FailedDouble && !Arrow.inputTriggered && !_noPress)
        {
            Tower.TriggerFailedInput(interactionType);
            return;
        }
        else if (interactionType == InteractionType.Single && Arrow.inputTriggered && !_noPress)
        {
            Tower.TriggerFailedInput(interactionType);
            return;
        }
        else if (interactionType == InteractionType.Single && !Arrow.inputTriggered)
        {
            SFXCollection.Instance.PlaySound(SFXType.QuietSuccess);
            return;
        }

        Tweens.Add(spriteRenderer.DOColor(ArrowManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine));
        Tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine));
        Tweens.Add(transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                KillAllTweens(Tweens);
                Destroy(gameObject);
            }
        ));

        SpawnPopUp(scoreType, true);
    }

    public void FailState(InteractionType interactionType)
    {
        FailState(Arrow, spriteRenderer, Tweens);
    }
    public IEnumerator PressTimeOut()
    {
        _noPress = true;
        yield return new WaitForSeconds(.15f);

        _noPress = false;
        yield return null;
    }
}
