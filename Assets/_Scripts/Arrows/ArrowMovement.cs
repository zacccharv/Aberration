using DG.Tweening;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField] private float _physicalDistance = 1;
    public IArrowStates _arrowStates;
    [SerializeField] private Transform _otherTransform;
    private Arrow _arrow;
    private bool folded;

    void OnEnable()
    {
        ArrowManager.MoveArrows += Move;
    }
    void OnDisable()
    {
        ArrowManager.MoveArrows -= Move;
    }

    void Awake()
    {
        _arrow = GetComponent<Arrow>();
        _arrowStates = GetComponent<IArrowStates>();
    }
    void Start()
    {
        float x, y;

        if (transform.position.x != 0) x = transform.position.x / -Mathf.Abs(transform.position.x);
        else x = 0;

        if (transform.position.y != 0) y = transform.position.y / -Mathf.Abs(transform.position.y);
        else y = 0;

        _arrow.vectorDirection = new(x, y);
    }

    private void Move(float time)
    {
        if (Tower.IsInBounds(transform.position, Tower.Instance.successBounds) && _arrow.interactionType == InteractionType.Long && !folded && _otherTransform != null)
        {
            _arrowStates.Tweens.Add(_otherTransform.DOMove(transform.position, time / 2).SetEase(Ease.InOutSine));
            folded = true;
        }
        else
        {
            _arrowStates.Tweens.Add(transform.DOMove(transform.position + ((Vector3)_arrow.vectorDirection * _physicalDistance), time / 2).SetEase(Ease.InOutSine));
        }
    }

}