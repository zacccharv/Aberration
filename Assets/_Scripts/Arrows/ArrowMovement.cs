using DG.Tweening;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    private Tween tween;
    [SerializeField] float _physicalDistance = 1;
    public Vector2 vectorDirection;
    private ArrowStateMachines _arrowStateMachine;

    void OnEnable()
    {
        ArrowManager.MoveArrows += Move;
        _arrowStateMachine.KillTweens += KillAllTweens;
    }
    void OnDisable()
    {
        ArrowManager.MoveArrows -= Move;
        _arrowStateMachine.KillTweens -= KillAllTweens;
    }

    void Awake()
    {
        _arrowStateMachine = GetComponent<ArrowStateMachines>();
    }

    private void Move(float time)
    {
        tween = transform.DOMove(transform.position + ((Vector3)vectorDirection * _physicalDistance), time / 2).SetEase(Ease.InOutSine);
    }

    private void KillAllTweens()
    {
        tween.Kill();
    }

}
