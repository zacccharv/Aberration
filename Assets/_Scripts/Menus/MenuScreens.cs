using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MenuType
{
    MainMenu,
    Audio,
    HighScores,
    Username
}

public class MenuScreens : MonoBehaviour
{
    public MenuType menuType, previousMenuType;
    [SerializeField] private GameObject MainMenu, Audio, HighScores, Username;
    private ButtonNavigation buttonNavigation;
    private Selectable _selectable;

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

            _selectable = buttonNavigation.mainMenuButtons[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.Audio)
        {
            MainMenu.SetActive(false);
            Audio.SetActive(true);
            HighScores.SetActive(false);
            menuType = MenuType.Audio;

            _selectable = buttonNavigation.audioMenuButtons[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.HighScores)
        {
            HighScores.SetActive(true);
            MainMenu.SetActive(false);
            Audio.SetActive(false);
            menuType = MenuType.HighScores;

            _selectable = buttonNavigation.scoreMenuButtons[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.Username && Username != null)
        {
            HighScores.SetActive(false);
            MainMenu.SetActive(false);
            Audio.SetActive(false);
            Username.SetActive(true);

            _selectable = buttonNavigation.userName;

            Invoke(nameof(DelayedSelection), .15f);
        }
    }

    public void DelayedSelection()
    {
        _selectable.Select();
    }
}
