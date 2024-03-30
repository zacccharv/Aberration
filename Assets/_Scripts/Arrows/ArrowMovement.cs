using DG.Tweening;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField] private float _physicalDistance = 1;
    [SerializeField] private Transform _otherTransform;
    private IArrowStates _arrowStates;
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
        _arrowStates = GetComponent<IArrowStates>();
        Debug.Log(_arrowStates);
        _arrow = GetComponent<Arrow>();
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
        if (Tower.IsInBounds(transform.position, Tower.Instance.successBounds) && _arrow.interactionType == InteractionType.Long && !folded)
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