using DG.Tweening;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public Vector2 vectorDirection;
    [SerializeField] float _physicalDistance = 1;
    private Tween tween;
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
    void Start()
    {
        float x, y;

        if (transform.parent.position.x != 0) x = transform.parent.position.x / -Mathf.Abs(transform.parent.position.x);
        else x = 0;

        if (transform.parent.position.y != 0) y = transform.parent.position.y / -Mathf.Abs(transform.parent.position.y);
        else y = 0;

        vectorDirection = new(x, y);
    }

    private void Move(float time)
    {
        tween = transform.parent.DOMove(transform.parent.position + ((Vector3)vectorDirection * _physicalDistance), time / 2).SetEase(Ease.InOutSine);
    }

    private void KillAllTweens()
    {
        tween.Kill();
    }

}
