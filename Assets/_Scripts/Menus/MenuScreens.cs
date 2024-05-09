using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MenuType
{
    MainMenu,
    Audio,
    HighScores,
    Username,
    PauseMenu
}

public class MenuScreens : MonoBehaviour
{
    public MenuType menuType, previousMenuType;
    [SerializeField] private GameObject _mainMenu, _audio, _highScores, _username;

    // TODO add play images
    [SerializeField] private Sprite[] _playImages;
    private ButtonNavigation buttonNavigation;
    private Selectable _selectable;

    void Awake()
    {
        buttonNavigation = GetComponent<ButtonNavigation>();
        _mainMenu.SetActive(true);
    }

    public void SwitchMenus(MenuType menu)
    {
        previousMenuType = menuType;
        buttonNavigation.buttonIndex = 0;

        if (menu == MenuType.MainMenu)
        {
            _mainMenu.SetActive(true);
            _audio.SetActive(false);
            _highScores.SetActive(false);
            menuType = MenuType.MainMenu;

            _selectable = buttonNavigation.mainMenuButtons[0];
            _selectable.image.sprite = _playImages[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.PauseMenu)
        {
            _mainMenu.SetActive(true);
            _audio.SetActive(false);
            _highScores.SetActive(false);
            menuType = MenuType.MainMenu;

            _selectable = buttonNavigation.mainMenuButtons[0];
            _selectable.image.sprite = _playImages[1];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.Audio)
        {
            _mainMenu.SetActive(false);
            _audio.SetActive(true);
            _highScores.SetActive(false);
            menuType = MenuType.Audio;

            _selectable = buttonNavigation.audioMenuButtons[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.HighScores)
        {
            _highScores.SetActive(true);
            _mainMenu.SetActive(false);
            _audio.SetActive(false);
            menuType = MenuType.HighScores;

            _selectable = buttonNavigation.scoreMenuButtons[0];

            Invoke(nameof(DelayedSelection), .15f);
        }
        else if (menu == MenuType.Username && _username != null)
        {
            _highScores.SetActive(false);
            _mainMenu.SetActive(false);
            _audio.SetActive(false);
            _username.SetActive(true);

            _selectable = buttonNavigation.userName;

            Invoke(nameof(DelayedSelection), .15f);
        }
    }

    public void DelayedSelection()
    {
        _selectable.Select();
    }
}
