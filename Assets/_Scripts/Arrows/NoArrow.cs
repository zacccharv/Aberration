using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NoArrow : BaseArrow, IArrowStates
{
    public Arrow Arrow { get; set; }

    public List<Tween> Tweens { get; set; } = new();

    public SpriteRenderer m_renderer;

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
    void Awake()
    {
        m_renderer.sprite = ArrowManager.Instance.aberrationSprites[Random.Range(0, ArrowManager.Instance.aberrationSprites.Count)];
        Tweens.Add(m_renderer.DOFade(.5f, .33f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo));
    }

    void Start()
    {
        Arrow = GetComponent<Arrow>();
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
            Tweens.Add(m_renderer.DOColor(ArrowManager.Instance.arrowHighlightColor[num], .15f).SetEase(Ease.InSine));

            Arrow.boundsIndex = 2;

            Tower.TriggerTowerChange(Arrow.direction, Tower.Instance);
        }
        else if (Tower.IsInBounds(transform.position, Tower.Instance.animationBounds) && Arrow.boundsIndex == 0)
        {
            Arrow.boundsIndex = 1;
        }
    }

    public void SuccessState(ScoreType scoreType, InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0] != Arrow || GameManager.Instance.gameState == GameState.Ended) return;

        Arrow.inputTriggered = true;
        SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
    }

    public void FailState(InteractionType interactionType)
    {
        FailState(Arrow, m_renderer, Tweens);
    }
}
