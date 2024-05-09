using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{
    public List<Button> mainMenuButtons, audioMenuButtons, scoreMenuButtons;
    public TMP_InputField userName;
    private AudioMenu audioMenu;
    [SerializeField] private GameObject _username;
    private MenuScreens menuScreens;
    public int previousIndex;
    public int buttonIndex = 0;
    private List<Button> buttons;

    void OnEnable()
    {
        InputManager_Z.DirectionPressed += OnDirectionPressed;
        InputManager_Z.UIInputPressed += TriggerSelected;
    }

    void OnDisable()
    {
        InputManager_Z.DirectionPressed -= OnDirectionPressed;
        InputManager_Z.UIInputPressed -= TriggerSelected;
    }

    void Start()
    {
        audioMenu = GetComponent<AudioMenu>();
        menuScreens = GetComponent<MenuScreens>();
        mainMenuButtons[0].Select();
    }

    private void OnDirectionPressed(Direction direction, InteractionType _)
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.gameState == GameState.Started) return;
        }

        if (_username != null)
        {
            if (_username.activeInHierarchy) return;
        }

        buttons = mainMenuButtons;

        switch (menuScreens.menuType)
        {
            case MenuType.MainMenu:
                buttons = mainMenuButtons;
                break;
            case MenuType.Audio:
                buttons = audioMenuButtons;
                break;
            case MenuType.HighScores:
                buttons = scoreMenuButtons;
                break;
            default:
                break;
        }

        if (direction == Direction.Up)
        {
            previousIndex = buttonIndex;

            buttonIndex--;

            if (buttonIndex == -1) buttonIndex = buttons.Count - 1;

            buttonIndex %= buttons.Count;

            buttons[buttonIndex].Select();
            SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
        }
        else if (direction == Direction.Down)
        {
            previousIndex = buttonIndex;

            buttonIndex++;

            buttonIndex %= buttons.Count;

            buttons[buttonIndex].Select();
            SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
        }

        if (menuScreens.menuType == MenuType.Audio)
        {
            if (direction == Direction.Right || direction == Direction.Left)
            {
                audioMenu.MoveSliders(direction, buttonIndex);
            }
        }
    }

    private void TriggerSelected(InputType inputType)
    {
        if (inputType == InputType.Confirm)
        {
            if (buttonIndex <= buttons.Count - 1)
            {
                buttons[buttonIndex].onClick.Invoke();
            }
        }
        else if (inputType == InputType.Esc && GameManager.Instance.gameState != GameState.Paused && GameManager.Instance.gameState != GameState.Ended)
        {
            GameManager.Instance.ChangeGameStateChange(GameState.Paused);
            menuScreens.SwitchMenus(MenuType.PauseMenu);
        }
    }
}
