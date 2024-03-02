using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Right,
    Down,
    Left,
    None
}

public class ArrowMovement : MonoBehaviour
{
    public Direction direction;
    public Vector2 VectorDirection { get { return GetDirection(direction); } set { } }
    [SerializeField] Vector2 _vecDirTest;
    [SerializeField] float _physicalDistance = 0.5f;


    void OnEnable()
    {
        LaneManager.MoveArrows += Move;
    }
    void OnDisable()
    {
        LaneManager.MoveArrows -= Move;
    }

    void Awake()
    {
        _vecDirTest = VectorDirection;
    }
    public void Move()
    {
        transform.position += (Vector3)VectorDirection * _physicalDistance;

        if (InBounds(transform.position, Tower.Instance.bounds))
        {
            Destroy(gameObject);
        }
    }

    public bool InBounds(Vector2 position, Bounds bounds)
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

    public static Vector2 GetDirection(Direction direction) => direction switch
    {
        Direction.Up => Vector2.up,
        Direction.Right => Vector2.right,
        Direction.Down => Vector2.down,
        Direction.Left => Vector2.left,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
    };

}
