using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    public float moveInterval;
    [SerializeField] float _timer;

    public static Vector2 GetDirection(Direction direction) => direction switch
    {
        Direction.Up => Vector2.up,
        Direction.Right => Vector2.up,
        Direction.Down => Vector2.up,
        Direction.Left => Vector2.up,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
    };

}
