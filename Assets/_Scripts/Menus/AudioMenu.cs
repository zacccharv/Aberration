using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public List<Slider> sliders;
    public List<Image> sliderFills;
    [SerializeField] private Color _sliderFillColor, _highlightedFillColor;
    private MenuScreens _menuScreens;
    private ButtonNavigation _buttonNavigation;

    void Start()
    {
        _buttonNavigation = GetComponent<ButtonNavigation>();
        _menuScreens = GetComponent<MenuScreens>();

        sliders[0].value = MusicManager.Instance.volumeSliders.MasterVolume;
        sliders[1].value = MusicManager.Instance.volumeSliders.MusicVolume;
        sliders[2].value = MusicManager.Instance.volumeSliders.SFXVolume;
    }

    public void MoveSliders(Direction direction, int index)
    {
        if (index == 3) return;

        if (direction == Direction.Right)
        {
            if (_buttonNavigation.buttonIndex != 3)
            {
                sliders[_buttonNavigation.buttonIndex].value += 1f;

                // if SFX make sound
                if (_buttonNavigation.buttonIndex == 2 || _buttonNavigation.buttonIndex == 0) SFXCollection.Instance.PlaySound(SFXType.Success);
            }
        }
        else if (direction == Direction.Left)
        {
            if (_buttonNavigation.buttonIndex != 3)
            {
                sliders[_buttonNavigation.buttonIndex].value -= 1f;

                // if SFX make sound
                if (_buttonNavigation.buttonIndex == 2 || _buttonNavigation.buttonIndex == 0) SFXCollection.Instance.PlaySound(SFXType.Success);
            }
        }
    }

    public void PressBack()
    {
        MenuType menuType;

        if (GameManager.Instance.gameState == GameState.Paused)
        {
            menuType = MenuType.PauseMenu;
        }
        else
        {
            menuType = MenuType.MainMenu;
        }

        _menuScreens.SwitchMenus(menuType);
    }

    public void ColorSlider(int index)
    {
        if (_buttonNavigation.previousIndex == 0 && index != _buttonNavigation.previousIndex)
        {
            sliderFills[0].color = _sliderFillColor;
        }
        else if (_buttonNavigation.previousIndex == 1 && index != _buttonNavigation.previousIndex)
        {
            sliderFills[1].color = _sliderFillColor;
        }
        else if (_buttonNavigation.previousIndex == 2 && index != _buttonNavigation.previousIndex)
        {
            sliderFills[2].color = _sliderFillColor;
        }

        if (_buttonNavigation.buttonIndex == 0)
        {
            sliderFills[0].color = _highlightedFillColor;
        }
        else if (_buttonNavigation.buttonIndex == 1)
        {
            sliderFills[1].color = _highlightedFillColor;
        }
        else if (_buttonNavigation.buttonIndex == 2)
        {
            sliderFills[2].color = _highlightedFillColor;
        }
    }
}
