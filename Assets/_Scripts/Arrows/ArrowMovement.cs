using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField] private float _physicalDistance = 1;
    [SerializeField] private Transform _otherTransform;
    private List<Tween> tween = new();
    private Arrow _arrow;
    private ArrowStateMachines _arrowStateMachine;
    private bool folded;

    void OnEnable()
    {
        ArrowManager.MoveArrows += Move;
        _arrowStateMachine.KillAllTweens += ArrowManager.KillAllTweens;
    }
    void OnDisable()
    {
        ArrowManager.MoveArrows -= Move;
        _arrowStateMachine.KillAllTweens -= ArrowManager.KillAllTweens;
    }

    void Awake()
    {
        _arrow = GetComponent<Arrow>();
        _arrowStateMachine = GetComponent<ArrowStateMachines>();
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
            tween.Add(_otherTransform.DOMove(transform.position, time / 2).SetEase(Ease.InOutSine));
            folded = true;
        }
        else
            tween.Add(transform.DOMove(transform.position + ((Vector3)_arrow.vectorDirection * _physicalDistance), time / 2).SetEase(Ease.InOutSine));
    }
}