using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType
{
    MainMenu,
    Audio,
    HighScores
}

public class MenuScreens : MonoBehaviour
{
    [SerializeField] public MenuType menuType;

    [SerializeField] private GameObject MainMenu, Audio, HighScores;

    public void SwitchMenus(MenuType menu)
    {
        ButtonNavigation.buttonIndex = 0;

        if (menu == MenuType.MainMenu)
        {
            MainMenu.SetActive(true);
            Audio.SetActive(false);
            HighScores.SetActive(false);
            menuType = MenuType.MainMenu;
        }
        else if (menu == MenuType.Audio)
        {
            MainMenu.SetActive(false);
            Audio.SetActive(true);
            HighScores.SetActive(false);
            menuType = MenuType.Audio;
        }
        else if (menu == MenuType.HighScores)
        {
            MainMenu.SetActive(false);
            Audio.SetActive(false);
            HighScores.SetActive(true);
            menuType = MenuType.HighScores;
        }
    }
}
