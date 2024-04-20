using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoubleArrow : BaseArrow, IArrowStates
{
    public Arrow Arrow { get; set; }
    public List<Tween> Tweens { get; set; } = new();
    public bool PerfectInputStart { get; set; }

    public SpriteRenderer spriteRenderer, numberRenderer;
    [SerializeField] private int _pressCount;

    // TODO Give a bit more time for perfect input?
    // TODO Have animation Double Flash instead of double expand
    [SerializeField] private float _perfectInputTime;
    private float _perfectInputTimer;

    void OnEnable()
    {
        Tower.StartInput += OnInputStart;
        Tower.InputEvent += SetState;
    }
    void OnDisable()
    {
        Tower.StartInput -= OnInputStart;
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
            Arrow.boundsIndex = 2;

            Tower.TriggerTowerChange(Arrow.direction, Arrow.interactionType, Tower.Instance);

            Tweens.Add(transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo));
            Tweens.Add(spriteRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));
            Tweens.Add(numberRenderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));

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

    public void SetState(ScoreType scoreType, InteractionType interactionType)
    {
        if (scoreType == ScoreType.Press) SuccessState(scoreType, interactionType);
        else if (scoreType != ScoreType.Press) FailState();
    }

    public void SuccessState(ScoreType scoreType, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] != Arrow || GameManager.Instance.gameState == GameState.Ended || scoreType == ScoreType.Empty) return;

        if (interactionType == InteractionType.NoPress || interactionType == InteractionType.Long)
        {
            Tower.TriggerFailedInput(interactionType);
        }

        if (interactionType == InteractionType.Double && !Arrow.inputTriggered)
        {
            // if (PerfectInputStart) Debug.Log("PERFECT INPUT DOUBLE");
            // else Debug.Log("IMPERFECT INPUT DOUBLE");

            Arrow.inputTriggered = true;
        }
        else if (interactionType == InteractionType.FailedDouble && !Arrow.inputTriggered)
        {
            Tower.TriggerFailedInput(interactionType);
            return;
        }
        else if (interactionType == InteractionType.Single && Arrow.inputTriggered)
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

    public void FailState()
    {
        FailState(Arrow, spriteRenderer, Tweens);
    }

    public void OnInputStart()
    {
        if (ArrowManager.Instance.interactableArrows[0] != Arrow || GameManager.Instance.gameState == GameState.Ended) return;

        if (_perfectInputTimer > _perfectInputTime) PerfectInputStart = true;
    }
}
