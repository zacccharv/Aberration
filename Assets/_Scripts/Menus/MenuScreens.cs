using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MenuType
{
    MainMenu,
    Audio,
    HighScores,
    Username,
    PauseMenu,
    None
}

public class MenuScreens : MonoBehaviour
{
    public static event Action ScreenSwitch;
    public MenuType menuType;
    [SerializeField] private GameObject _mainMenu, _audio, _highScores, _username;

    // TODO add play images
    [SerializeField] private Sprite[] _playImages;
    private ButtonNavigation buttonNavigation;
    private Selectable _selectable;

    void Start()
    {
        buttonNavigation = GetComponent<ButtonNavigation>();

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            _mainMenu.SetActive(true);
            SwitchMenus(MenuType.MainMenu);
        }
        else
            menuType = MenuType.None;
    }

    public void SwitchMenus(MenuType menu)
    {
        buttonNavigation.buttonIndex = 0;
        buttonNavigation.previousIndex = 0;

        if (menu == MenuType.MainMenu && (GameManager.Instance.gameState == GameState.Ended || GameManager.Instance.gameState == GameState.PreStart))
        {
            _mainMenu.SetActive(true);
            _audio.SetActive(false);
            _highScores.SetActive(false);
            menuType = MenuType.MainMenu;

            _selectable = buttonNavigation.mainMenuButtons[0];
            buttonNavigation.buttons = buttonNavigation.mainMenuButtons;

            _selectable.image.sprite = _playImages[0];
            _selectable.image.SetNativeSize();

            Invoke(nameof(DelayedSelection), .15f);

            GameManager.Instance.title.SetActive(true);
        }
        else if ((menu == MenuType.MainMenu || menu == MenuType.PauseMenu) && GameManager.Instance.gameState == GameState.Paused)
        {
            _mainMenu.SetActive(true);
            _audio.SetActive(false);
            _highScores.SetActive(false);
            menuType = MenuType.MainMenu;

            _selectable = buttonNavigation.mainMenuButtons[0];
            buttonNavigation.buttons = buttonNavigation.mainMenuButtons;

            _selectable.image.sprite = _playImages[1];
            _selectable.image.SetNativeSize();

            Invoke(nameof(DelayedSelection), .15f);

            GameManager.Instance.title.SetActive(true);
        }
        else if (menu == MenuType.Audio)
        {
            _mainMenu.SetActive(false);
            _audio.SetActive(true);
            _highScores.SetActive(false);
            menuType = MenuType.Audio;

            _selectable = buttonNavigation.audioMenuButtons[0];
            buttonNavigation.buttons = buttonNavigation.audioMenuButtons;

            Invoke(nameof(DelayedSelection), .15f);

            GameManager.Instance.title.SetActive(true);
        }
        else if (menu == MenuType.HighScores)
        {
            _mainMenu.SetActive(false);
            _highScores.SetActive(true);
            _audio.SetActive(false);
            menuType = MenuType.HighScores;

            _selectable = buttonNavigation.scoreMenuButtons[0];
            buttonNavigation.buttons = buttonNavigation.scoreMenuButtons;

            Invoke(nameof(DelayedSelection), .15f);

            GameManager.Instance.title.SetActive(false);
        }
        else if (menu == MenuType.Username && _username != null)
        {
            _highScores.SetActive(false);
            _mainMenu.SetActive(false);
            _audio.SetActive(false);
            _username.SetActive(true);

            _selectable = buttonNavigation.userName;

            Invoke(nameof(DelayedSelection), .15f);

            GameManager.Instance.title.SetActive(false);
        }
        else if (menu == MenuType.None)
        {
            _highScores.SetActive(false);
            _mainMenu.SetActive(false);
            _audio.SetActive(false);

            menuType = MenuType.None;
            return;
        }

        ScreenSwitch?.Invoke();
    }

    public void DelayedSelection()
    {
        _selectable.Select();

        if (menuType == MenuType.Audio)
            GetComponent<AudioMenu>().ColorSlider(0);

    }
}
