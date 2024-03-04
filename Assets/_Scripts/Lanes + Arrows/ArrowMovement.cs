using UnityEngine;

[RequireComponent(typeof(Arrow))]
public class ArrowMovement : MonoBehaviour
{
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

        if (IsInBounds(transform.position, Tower.Instance.bounds))
        {
            Destroy(gameObject);
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
