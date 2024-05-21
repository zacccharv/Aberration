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

    void Start()
    {
        sliders[0].value = MusicManager.Instance.masterVolume;
        sliders[1].value = MusicManager.Instance.musicVolume;
        sliders[2].value = MusicManager.Instance.SFXVolume;
    }

    public void MoveSliders(Direction direction, int index)
    {
        if (index == 3) return;

        if (direction == Direction.Right)
        {
            if (buttonNavigation.buttonIndex != 3)
            {
                sliders[buttonNavigation.buttonIndex].value += 1f;

                // if SFX make sound
                if (buttonNavigation.buttonIndex == 2 || buttonNavigation.buttonIndex == 0) SFXCollection.Instance.PlaySound(SFXType.Success);
            }
        }
        else if (direction == Direction.Left)
        {
            if (buttonNavigation.buttonIndex != 3)
            {
                sliders[buttonNavigation.buttonIndex].value -= 1f;

                // if SFX make sound
                if (buttonNavigation.buttonIndex == 2 || buttonNavigation.buttonIndex == 0) SFXCollection.Instance.PlaySound(SFXType.Success);
            }
        }
    }

    public void PressBack()
    {
        GetComponent<MenuScreens>().SwitchMenus(MenuType.MainMenu);
    }

    public void ColorSlider(int index)
    {
        if (buttonNavigation.previousIndex == 0 && index != buttonNavigation.previousIndex)
        {
            sliderFills[0].color = _sliderFillColor;
        }
        else if (buttonNavigation.previousIndex == 1 && index != buttonNavigation.previousIndex)
        {
            sliderFills[1].color = _sliderFillColor;
        }
        else if (buttonNavigation.previousIndex == 2 && index != buttonNavigation.previousIndex)
        {
            sliderFills[2].color = _sliderFillColor;
        }

        if (buttonNavigation.buttonIndex == 0)
        {
            sliderFills[0].color = _highlightedFillColor;
        }
        else if (buttonNavigation.buttonIndex == 1)
        {
            sliderFills[1].color = _highlightedFillColor;
        }
        else if (buttonNavigation.buttonIndex == 2)
        {
            sliderFills[2].color = _highlightedFillColor;
        }
    }
}
