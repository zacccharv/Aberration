using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public List<Slider> sliders;
    public List<Image> sliderFills;
    [SerializeField] private Color _sliderFillColor, _highlightedFillColor;

    public void MoveSliders(Direction direction, int index)
    {
        if (index == 2) return;

        if (direction == Direction.Right)
        {
            if (ButtonNavigation.buttonIndex == 1 || ButtonNavigation.buttonIndex == 2)
            {
                sliders[ButtonNavigation.buttonIndex - 1].value += 1f;

                if (ButtonNavigation.buttonIndex == 2) SFXCollection.Instance.PlaySound(SFXType.Success);
            }
        }
        else if (direction == Direction.Left)
        {
            if (ButtonNavigation.buttonIndex == 1 || ButtonNavigation.buttonIndex == 2)
            {
                sliders[ButtonNavigation.buttonIndex - 1].value -= 1f;

                if (ButtonNavigation.buttonIndex == 2) SFXCollection.Instance.PlaySound(SFXType.Success);
            }
        }

        ColorSlider(index);
    }

    public void PressBack()
    {

    }

    private void ColorSlider(int index)
    {
        if (ButtonNavigation.previousIndex != 0 && index != ButtonNavigation.previousIndex)
        {
            sliderFills[0].color = _sliderFillColor;
        }
        else if (ButtonNavigation.previousIndex != 1 && index != ButtonNavigation.previousIndex)
        {
            sliderFills[1].color = _sliderFillColor;
        }

        if (ButtonNavigation.buttonIndex == 0)
        {
            sliderFills[0].color = _highlightedFillColor;
        }
        else if (ButtonNavigation.buttonIndex == 1)
        {
            sliderFills[1].color = _highlightedFillColor;
        }
    }
}
