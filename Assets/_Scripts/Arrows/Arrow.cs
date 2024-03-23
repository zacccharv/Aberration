using UnityEngine;

public class Arrow : MonoBehaviour
{
    /// <summary>
    /// Interaction type of arrow 
    /// </summary>
    public InteractionType interactionType;

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

    public int pressCount;

    public SpriteRenderer arrowSpriteRenderer, numberSpriteRenderer;

    void Awake()
    {
        arrowSpriteRenderer.color = ArrowManager.Instance.arrowColors[(int)direction];
        if (numberSpriteRenderer) numberSpriteRenderer.color = ArrowManager.Instance.arrowColors[(int)direction];
        ArrowManager.Instance.interactableArrows.Add(this);
    }
}