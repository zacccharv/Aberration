using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public List<Slider> sliders;
    public List<Image> sliderFills;
    [SerializeField] private Color _sliderFillColor, _highlightedFillColor;

    private MenuScreens menuScreens;
    private ButtonNavigation buttonNavigation;

    void Awake()
    {
        menuScreens = GetComponent<MenuScreens>();
        buttonNavigation = GetComponent<ButtonNavigation>();
    }

    public void MoveSliders(Direction direction, int index)
    {
        if (index == 2) return;

        if (direction == Direction.Right)
        {
            if (buttonNavigation.buttonIndex == 0 || buttonNavigation.buttonIndex == 1)
            {
                sliders[buttonNavigation.buttonIndex].value += 1f;

                if (buttonNavigation.buttonIndex == 1) SFXCollection.Instance.PlaySound(SFXType.Success);
            }
        }
        else if (direction == Direction.Left)
        {
            if (buttonNavigation.buttonIndex == 0 || buttonNavigation.buttonIndex == 1)
            {
                sliders[buttonNavigation.buttonIndex].value -= 1f;

                if (buttonNavigation.buttonIndex == 1) SFXCollection.Instance.PlaySound(SFXType.Success);
            }
        }

        ColorSlider(index);
    }

    public void PressBack()
    {
        menuScreens.SwitchMenus(MenuType.MainMenu);
    }

    private void ColorSlider(int index)
    {
        if (buttonNavigation.previousIndex != 0 && index != buttonNavigation.previousIndex)
        {
            sliderFills[0].color = _sliderFillColor;
        }
        else if (buttonNavigation.previousIndex != 1 && index != buttonNavigation.previousIndex)
        {
            sliderFills[1].color = _sliderFillColor;
        }

        if (buttonNavigation.buttonIndex == 0)
        {
            sliderFills[0].color = _highlightedFillColor;
        }
        else if (buttonNavigation.buttonIndex == 1)
        {
            sliderFills[1].color = _highlightedFillColor;
        }
    }
}
