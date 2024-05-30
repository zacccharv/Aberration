using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LaneInputIndicator : MonoBehaviour
{
    [Serializable]
    public struct ColorDuo
    {
        public Color color_0;
        public Color color_1;
    }
    public List<ColorDuo> colorDuos;
    private Material _material;
    [SerializeField] private GameObject _center;
    private DG.Tweening.Sequence _sequence;
    private int _previousIndex;

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

        _previousIndex = 4;
    }

    public void NewLoopAnimation(Direction direction)
    {
        int index = GetIndex(direction);

        _material.DOColor(colorDuos[index].color_0, "_Color01", .2f).SetEase(Ease.InOutSine).SetDelay(.2f);
        _material.DOColor(colorDuos[index].color_1, "_Color02", .2f).SetEase(Ease.InOutSine).SetDelay(.2f);

        transform.DOScale(2.55f, .1f).OnComplete(() => transform.DOScale(2.5f, .2f)).SetDelay(.2f);


        static int GetIndex(Direction direction)
        {
            int result = direction switch
            {
                Direction.Up => 0,
                Direction.Right => 1,
                Direction.Down => 2,
                Direction.Left => 3,
                Direction.None => -1,
                Direction.Null => -1,
                _ => default,
            };

            return result;
        }
    }
}
