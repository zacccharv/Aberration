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
    /// True if in success bounds and input pressed once at index 0 in list
    /// </summary>
    public bool isPressed;

    void Awake()
    {
        GetComponent<SpriteRenderer>().color = ArrowManager.Instance.arrowColors[(int)direction];
        ArrowManager.Instance.interactableArrows.Add(this);
    }
}