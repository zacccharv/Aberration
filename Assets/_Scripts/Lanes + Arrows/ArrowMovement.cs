using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Arrow))]
public class ArrowMovement : MonoBehaviour
{
    public static event DirectionPress CurrentDirectionSet;
    private Arrow _arrow;
    public Vector2 vectorDirection;
    [SerializeField] float _physicalDistance = 1;

    void OnEnable()
    {
        LaneManager.MoveArrows += Move;
        Tower.SuccesfulInput += Success;
        Tower.FailedInput += Fail;
    }
    void OnDisable()
    {
        LaneManager.MoveArrows -= Move;
        Tower.SuccesfulInput -= Success;
        Tower.FailedInput -= Fail;
    }

    void Awake()
    {
        _arrow = GetComponent<Arrow>();
    }

    private void Success()
    {
        if (!IsInBounds(transform.position, Tower.Instance.successFailBounds)) return;
        GetComponent<SpriteRenderer>().DOColor(LaneManager.Instance.SuccessColor, 1).SetEase(Ease.OutSine);
        transform.DOScale(transform.localScale * 5, 1.5f).SetEase(Ease.OutSine);
    }
    private void Fail()
    {
        if (!IsInBounds(transform.position, Tower.Instance.successFailBounds)) return;
        GetComponent<SpriteRenderer>().color = LaneManager.Instance.FailColor;
    }
    private void CheckNotPressed()
    {
        if (!IsInBounds(transform.position, Tower.Instance.successFailBounds)) return;

        if (_arrow.direction == Direction.None)
        {
            Tower.Instance.OnDirectionPressed(_arrow.direction);
        }
        else if (_arrow.direction != Direction.None && !Tower.Instance.inputPressed)
        {
            Tower.Instance.OnDirectionPressed(Direction.None);
        }
    }


    public void Move(float time)
    {
        transform.DOMove(transform.position + ((Vector3)vectorDirection * _physicalDistance), time / 2).SetEase(Ease.InOutSine);

        if (IsInBounds(transform.position, Tower.Instance.destroyBounds))
        {
            CheckNotPressed();

            DOTween.KillAll();
            Destroy(gameObject);
        }
        else if (IsInBounds(transform.position, Tower.Instance.inputBounds))
        {
            // Slightly change color
            CurrentDirectionSet?.Invoke(_arrow.direction);

            int num = (int)_arrow.direction;
            GetComponent<SpriteRenderer>().DOColor(LaneManager.Instance.arrowHighlightColor[num], time / 3).SetEase(Ease.InSine);
        }
    }

    public bool IsInBounds(Vector2 position, Bounds bounds)
    {
        if (position.x < (bounds.center.x + bounds.extents.x)
            && position.x > (bounds.center.x - bounds.extents.x)
            && position.y < (bounds.center.y + bounds.extents.y)
            && position.y > (bounds.center.y - bounds.extents.y))
        {
            _arrow.inBounds = true;
            return true;
        }
        else
        {
            _arrow.inBounds = false;
            return false;
        }
    }
}
