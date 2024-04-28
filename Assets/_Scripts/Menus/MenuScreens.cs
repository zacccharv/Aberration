using Unity.VisualScripting;
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
    private ButtonNavigation buttonNavigation;

    void Awake()
    {
        buttonNavigation = GetComponent<ButtonNavigation>();
        MainMenu.SetActive(true);
    }

    public void SwitchMenus(MenuType menu)
    {
        if (menu == MenuType.MainMenu)
        {
            MainMenu.SetActive(true);
            Audio.SetActive(false);
            HighScores.SetActive(false);
            menuType = MenuType.MainMenu;
            buttonNavigation.mainMenuButtons[0].Select();
        }
        else if (menu == MenuType.Audio)
        {
            MainMenu.SetActive(false);
            Audio.SetActive(true);
            HighScores.SetActive(false);
            menuType = MenuType.Audio;
            buttonNavigation.audioMenuButtons[0].Select();
        }
        else if (menu == MenuType.HighScores)
        {
            HighScores.SetActive(true);
            MainMenu.SetActive(false);
            Audio.SetActive(false);
            menuType = MenuType.HighScores;
            buttonNavigation.scoreMenuButtons[0].Select();
        }

        ButtonNavigation.buttonIndex = 0;
    }
}
