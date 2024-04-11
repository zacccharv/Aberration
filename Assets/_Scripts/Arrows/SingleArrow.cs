using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SingleArrow : BaseArrow, IArrowStates
{
    public Arrow Arrow { get; set; }

    public List<Tween> Tweens { get; set; } = new();

    public SpriteRenderer spriteRenderer, numberRenderer;
    [SerializeField] private float _perfectInputDivider = 1.7f;
    private float _perfectInputTimer;

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

            Tweens.Add(transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo));
            Tweens.Add(spriteRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));
            Tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));

            Arrow.boundsIndex = 2;

            Tower.TriggerTowerChange(Arrow.direction, Tower.Instance);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.animationBounds) && Arrow.boundsIndex == 0)
        {
            Arrow.boundsIndex = 1;
        }

        if (Tower.IsInBounds(transform.position, Tower.Instance.successBounds))
        {
            _perfectInputTimer += Time.deltaTime;
        }
    }
    public void SuccessState(ScoreType scoreType, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] != Arrow || GameManager.Instance.gameState == GameState.Ended) return;

        if (interactionType == InteractionType.Double || interactionType == InteractionType.NoPress)
        {
            Tower.TriggerFailedInput();
            return;
        }

        if (_perfectInputTimer > (Arrow.moveSpeed / _perfectInputDivider)) Debug.Log("PERFECT INPUT SINGLE");
        else Debug.Log("IMPERFECT INPUT SINGLE");

        Arrow.inputTriggered = true;
        SFXCollection.Instance.PlaySound(SFXType.Success);

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
    public void FailState()
    {
        FailState(Arrow, spriteRenderer, Tweens);
    }
}
