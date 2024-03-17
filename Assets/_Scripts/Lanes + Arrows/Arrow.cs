using UnityEngine;

public class Arrow : MonoBehaviour
{
    /// <summary>
    /// Direction arrow is facing
    /// </summary>
    public Direction direction;

    /// <summary>
    /// 0 not in any bounds, 1 in animation bounds, 2 in success bounds
    /// </summary>
    public int boundsIndex = 0;

    /// <summary>
    /// True if in success bounds and input pressed once and index 0 in list
    /// </summary>
    public bool isPressed;
}