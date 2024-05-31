using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaneInputIndicator : MonoBehaviour
{
    [Serializable]
    public struct ColorDuo
    {
        public Color color_0;
        public Color color_1;
    }
    public SpriteRenderer interactionIndicator;
    public List<ColorDuo> colorDuos;

    [SerializeField] private Sprite[] _interactionSprites;
    [SerializeField] Color _interactionColor;
    private Material _material;

    void OnEnable()
    {
        Tower.TowerChangeEvent += NewLoopAnimation;
    }
    void OnDisable()
    {
        Tower.TowerChangeEvent -= NewLoopAnimation;
    }

    void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    public void NewLoopAnimation(Direction direction, InteractionType interactionType)
    {
        int directionIndex = GetDirectionIndex(direction);
        int interactionIndex = GetInteractionIndex(interactionType);

        _material.DOColor(colorDuos[directionIndex].color_0, "_Color01", .2f).SetEase(Ease.InOutSine).SetDelay(.2f);
        _material.DOColor(colorDuos[directionIndex].color_1, "_Color02", .2f).SetEase(Ease.InOutSine).SetDelay(.2f);

        transform.DOScale(2.55f, .1f).OnComplete(() => transform.DOScale(2.5f, .2f)).SetDelay(.2f);

        interactionIndicator.sprite = _interactionSprites[interactionIndex];
        interactionIndicator.color = _interactionColor;

        static int GetDirectionIndex(Direction direction)
        {
            int result = direction switch
            {
                Direction.Up => 0,
                Direction.Right => 1,
                Direction.Down => 2,
                Direction.Left => 3,
                Direction.None => 4,
                Direction.Null => 4,
                _ => default,
            };

            return result;
        }

        static int GetInteractionIndex(InteractionType interactionType)
        {
            int result = interactionType switch
            {
                InteractionType.Single => 0,
                InteractionType.Double => 1,
                InteractionType.Long => 2,
                _ => default,
            };

            return result;
        }
    }
}
