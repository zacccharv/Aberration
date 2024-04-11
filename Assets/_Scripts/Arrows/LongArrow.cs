using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LongArrow : BaseArrow, IArrowStates
{
    public Arrow Arrow { get; set; }
    [field: SerializeField] public List<Tween> Tweens { get; set; } = new();
    [SerializeField] private List<SpriteRenderer> renderers = new();
    [SerializeField] private float _perfectInputDivider = 6;
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

        foreach (var item in renderers)
        {
            item.color = ArrowManager.Instance.arrowColors[(int)Arrow.direction];
        }
    }

    void Update()
    {
        UpdateBounds();
    }

    public void UpdateBounds()
    {
        // TODO make clear current long arrow when 2 in bounds
        if (Tower.IsInBounds(transform.position, Tower.Instance.destroyBounds))
        {
            Tower.CheckNotPressed(Arrow, Tower.Instance);

            KillAllTweens(Tweens);
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
        if (ArrowManager.Instance.interactableArrows[0] != Arrow || GameManager.Instance.gameState == GameState.Ended || scoreType == ScoreType.Empty) return;

        Arrow.inputTriggered = true;

        if (interactionType == InteractionType.Single || interactionType == InteractionType.Double || interactionType == InteractionType.NoPress)
        {
            Tower.TriggerFailedInput(interactionType);
            return;
        }

        if (_perfectInputTimer > ((Arrow.moveSpeed / _perfectInputDivider) + Arrow.moveSpeed)) Debug.Log("PERFECT INPUT LONG");
        else Debug.Log("IMMPERFECT INPUT LONG");

        foreach (var item in renderers)
        {
            Tweens.Add(item.DOColor(ArrowManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine));
        }

        Tweens.Add(transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                KillAllTweens(Tweens);
                Destroy(gameObject);
            }
        ));

        SFXCollection.Instance.PlaySound(SFXType.Success);
        SpawnPopUp(scoreType, true);
    }

    public void FailState(InteractionType interactionType)
    {
        FailState(Arrow, renderers, Tweens);
    }

}
