using UnityEngine;

[RequireComponent(typeof(Arrow))]
public class ArrowMovement : MonoBehaviour
{
    public static event DirectionPress CurrentDirectionSet;
    private Arrow _arrow;
    public Vector2 vectorDirection;
    [SerializeField] float _physicalDistance = 1;

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
        _arrow = GetComponent<Arrow>();
    }

    public void Move()
    {
        transform.position += (Vector3)vectorDirection * _physicalDistance;


        if (IsInBounds(transform.position, Tower.Instance.destroyBounds))
        {
            Destroy(gameObject);
        }
        else if (IsInBounds(transform.position, Tower.Instance.inputBounds))
        {
            // Slightly change color
            CurrentDirectionSet?.Invoke(_arrow.direction);

            int num = (int)_arrow.direction;
            GetComponent<SpriteRenderer>().color = LaneManager.Instance.arrowHighlightColor[num];
        }
    }

    public bool IsInBounds(Vector2 position, Bounds bounds)
    {
        if (position.x < (bounds.center.x + bounds.extents.x)
            && position.x > (bounds.center.x - bounds.extents.x)
            && position.y < (bounds.center.y + bounds.extents.y)
            && position.y > (bounds.center.y - bounds.extents.y))
        {
            _arrow.inBounds = true;
            return true;
        }
        else
        {
            _arrow.inBounds = false;
            return false;
        }
    }
}
