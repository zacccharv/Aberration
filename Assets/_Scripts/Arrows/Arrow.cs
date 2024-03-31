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
    public Vector2 vectorDirection;

    public float spawnTime;
    public float moveSpeed;

    /// <summary>
    /// 0 not in any bounds, 1 in animation bounds, 2 in success bounds
    /// </summary>
    public int boundsIndex = 0;

    /// <summary>
    /// True if in success bounds and input pressed once at index 0 in list
    /// </summary>
    public bool inputTriggered;

    public int pressCount;

    public SpriteRenderer arrowSpriteRenderer, numberSpriteRenderer;

    void Awake()
    {
        ArrowManager.Instance.interactableArrows.Add(this);

        arrowSpriteRenderer.color = ArrowManager.Instance.arrowColors[(int)direction];
        if (numberSpriteRenderer) numberSpriteRenderer.color = ArrowManager.Instance.arrowColors[(int)direction];
    }
}