using DG.Tweening;
using UnityEngine;

public class ComboPopUp : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image image;
    [SerializeField] Color color;
    void Start()
    {
        float randX = Random.Range(0, 361);
        float randY = Random.Range(0, 361);

        Vector2 pos = new(Mathf.Sin(randX) * 1.75f, Mathf.Cos(randY) * 1.75f);

        transform.DOMove(pos, 1.5f);
        transform.DOScale(.3f, 1.5f).SetEase(Ease.OutQuad);

        Color clear = new(color.a, color.g, color.b, 0);

        DOVirtual.Color(color, clear, 1.5f, (value) =>
        {
            image.color = value;
        }).SetEase(Ease.InSine).OnComplete(() => Destroy(gameObject));
    }
}
