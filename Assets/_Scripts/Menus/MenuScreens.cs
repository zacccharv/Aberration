using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MenuType
{
    MainMenu,
    Audio,
    HighScores
}

public class MenuScreens : MonoBehaviour
{
    public MenuType menuType, previousMenuType;
    private Button button;
    [SerializeField] private GameObject MainMenu, Audio, HighScores;
    private ButtonNavigation buttonNavigation;

    void Awake()
    {
        buttonNavigation = GetComponent<ButtonNavigation>();
        MainMenu.SetActive(true);
    }

    public void SwitchMenus(MenuType menu)
    {
        previousMenuType = menuType;
        buttonNavigation.buttonIndex = 0;

        if (menu == MenuType.MainMenu)
        {
            MainMenu.SetActive(true);
            Audio.SetActive(false);
            HighScores.SetActive(false);
            menuType = MenuType.MainMenu;

            button = buttonNavigation.mainMenuButtons[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.Audio)
        {
            MainMenu.SetActive(false);
            Audio.SetActive(true);
            HighScores.SetActive(false);
            menuType = MenuType.Audio;

            button = buttonNavigation.audioMenuButtons[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.HighScores)
        {
            HighScores.SetActive(true);
            MainMenu.SetActive(false);
            Audio.SetActive(false);
            menuType = MenuType.HighScores;

            button = buttonNavigation.scoreMenuButtons[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
    }

    public void DelayedSelection()
    {
        button.Select();
    }
}
