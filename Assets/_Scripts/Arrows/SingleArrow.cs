using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SingleArrow : BaseArrow, IArrowStates
{
    public Arrow Arrow { get; set; }
    public List<Tween> Tweens { get; set; } = new();
    public bool PerfectInputStart { get; set; }

    public SpriteRenderer spriteRenderer, numberRenderer;

    [Range(0, .8f)] public float perfectInputTime = .4f;
    public float perfectInputTimer;

    void OnEnable()
    {
        Tower.InputEvent += SetState;
    }
    void OnDisable()
    {
        Tower.InputEvent -= SetState;
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

            Tweens.Add(transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo));
            Tweens.Add(spriteRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));
            Tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));

            Arrow.boundsIndex = 2;

            Tower.TriggerTowerChange(Arrow.direction, Arrow.interactionType, perfectInputTime, Tower.Instance);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.animationBounds) && Arrow.boundsIndex == 0)
        {
            Arrow.boundsIndex = 1;
        }

        if (Tower.IsInBounds(transform.position, Tower.Instance.successBounds))
        {
            perfectInputTimer += GameManager.deltaTime;
        }
    }

    public void SetState(ScoreType scoreType, InteractionType interactionType)
    {
        if (scoreType == ScoreType.Success) SuccessState(scoreType, interactionType);
        else if (scoreType != ScoreType.Success) FailState();
    }

    public void SuccessState(ScoreType scoreType, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] != Arrow || GameManager.Instance.gameState == GameState.Ended) return;

        if (interactionType == InteractionType.Double || interactionType == InteractionType.NoPress)
        {
            Tower.TriggerFailedInput(interactionType);
            return;
        }

        Arrow.inputTriggered = true;

        Tweens.Add(spriteRenderer.DOColor(ArrowManager.Instance.SuccessColor, .7f).SetEase(Ease.OutSine));
        Tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.SuccessColor, .7f).SetEase(Ease.OutSine));
        Tweens.Add(transform.DOScale(transform.localScale * 4, .7f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                KillAllTweens(Tweens);
                Destroy(gameObject);
            }
        ));

        SpawnPopUp(scoreType, true);
    }

    public void FailState()
    {
        FailState(Arrow, spriteRenderer, Tweens);
    }
}
