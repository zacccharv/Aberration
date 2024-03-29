using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoubleArrow : BaseArrow, IArrowStates
{
    private float _inputTimer;
    [SerializeField] private float _doublePressResetTime = .5f;

    public Arrow Arrow { get; set; }
    public List<Tween> Tweens { get; set; }

    public SpriteRenderer spriteRenderer, numberRenderer;

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

        if (Arrow.pressCount == 1)
        {
            _inputTimer += Time.deltaTime;
        }
    }

    public void SuccessState(ScoreType scoreType)
    {
        if (Tower.Instance._arrow_0 != Arrow || GameManager.Instance.gameState == GameState.Ended) return;

        Arrow.pressCount++;

        if (Arrow.pressCount == 2 && _inputTimer > _doublePressResetTime)
        {
            Tower.TriggerFailedInput();
            return;
        }
        else if (Arrow.pressCount == 2 && _inputTimer < _doublePressResetTime)
        {
            Arrow.inputTriggered = true;
            SFXCollection.Instance.PlaySound(SFXType.Success);
        }
        else
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

}
