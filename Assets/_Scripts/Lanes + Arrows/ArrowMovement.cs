using UnityEngine;
using DG.Tweening;
using TMPro;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

[RequireComponent(typeof(Arrow))]
public class ArrowMovement : MonoBehaviour
{
    public static event DirectionPress CurrentDirectionSet;
    public Vector2 vectorDirection;
    private Arrow _arrow;
    [SerializeField] float _physicalDistance = 1;
    private SpriteRenderer _renderer;
    public bool _canPress, _pressed;
    public ScoreType scoreType1;
    private Tween tween_0, tween_1, tween_2, tween_3, tween_4;

    void OnEnable()
    {
        Tower.SuccessfulInput += Success;
        Tower.FailedInput += Fail;
        LaneManager.MoveArrows += Move;
    }
    void OnDisable()
    {
        Tower.SuccessfulInput -= Success;
        Tower.FailedInput -= Fail;
        LaneManager.MoveArrows -= Move;
    }

    void Awake()
    {
        _arrow = GetComponent<Arrow>();
        _renderer = GetComponent<SpriteRenderer>();
        _canPress = false;
    }

    private void Success(ScoreType scoreType)
    {
        scoreType1 = scoreType;
        if (!IsInBounds(transform.position, Tower.Instance.successBounds) || Tower.Instance.inputPressed) return;

        GameObject popup = Instantiate(Scoring.Instance.popUp, transform.position, Quaternion.identity);

        if (scoreType == ScoreType.Empty)
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"NO");
        }
        else if (scoreType == ScoreType.Direction)
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"+{5 * Scoring.Instance.comboMultiplier}");
        }

        tween_0 = _renderer.DOColor(LaneManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine);
        tween_1 = transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine);

        _canPress = false;
        _pressed = true;
    }
    private void Fail()
    {
        if (!IsInBounds(transform.position, Tower.Instance.failBounds)) return;

        GameObject popup = Instantiate(Scoring.Instance.popUp, transform.position, Quaternion.identity);
        popup.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{Scoring.Instance.subtraction}");
        popup.GetComponentInChildren<TextMeshProUGUI>().color = LaneManager.Instance.FailNumberColor;

        _renderer.color = LaneManager.Instance.FailColor;

        _canPress = false;
        _pressed = true;
    }
    private void CheckNotPressed()
    {
        if (!IsInBounds(transform.position, Tower.Instance.successBounds)) return;

        if (_arrow.direction == Direction.None)
        {
            Tower.Instance.OnDirectionPressed(_arrow.direction);
        }
        else if (_arrow.direction != Direction.None && _canPress)
        {
            Tower.Instance.OnDirectionPressed(Direction.None);
        }
    }

    void Update()
    {
        if (IsInBounds(transform.position, Tower.Instance.successBounds) && !_canPress && IsInBounds(transform.position, Tower.Instance.successBounds) && !_canPress && !_pressed)
        {
            _canPress = true;
            tween_2 = transform.DOScale(transform.localScale * 1.5f, .2f).SetLoops(-1, LoopType.Yoyo);

            _arrow.inSuccessBounds = true;
            Tower.Instance.arrow = gameObject;// TODO fix early press

        }
        if (IsInBounds(transform.position, Tower.Instance.failBounds))
        {
            _arrow.inFailBounds = true;
        }
        if (IsInBounds(transform.position, Tower.Instance.animationBounds))
        {
            _arrow.inAnimationBounds = true;
        }
    }

    public void Move(float time)
    {
        tween_3 = transform.DOMove(transform.position + ((Vector3)vectorDirection * _physicalDistance), time / 2).SetEase(Ease.InOutSine);

        if (IsInBounds(transform.position, Tower.Instance.destroyBounds))
        {
            CheckNotPressed();

            tween_0.Kill();
            tween_1.Kill();
            tween_2.Kill();
            tween_3.Kill();
            tween_4.Kill();

            Destroy(gameObject);
        }
        else if (IsInBounds(transform.position, Tower.Instance.animationBounds))
        {
            // Slightly change color
            CurrentDirectionSet?.Invoke(_arrow.direction);
            _canPress = false;
            _arrow.inAnimationBounds = true;

            int num = (int)_arrow.direction;
            tween_4 = _renderer.DOColor(LaneManager.Instance.arrowHighlightColor[num], time / 3).SetEase(Ease.InSine);
        }
    }

    public bool IsInBounds(Vector2 position, Bounds bounds)
    {
        if (position.x < (bounds.center.x + bounds.extents.x)
            && position.x > (bounds.center.x - bounds.extents.x)
            && position.y < (bounds.center.y + bounds.extents.y)
            && position.y > (bounds.center.y - bounds.extents.y))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
