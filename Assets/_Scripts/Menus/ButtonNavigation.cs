using System.Collections;
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
    public List<Button> buttons;
    private bool canPress;

    void OnEnable()
    {
        InputManager_Z.DirectionPressed += OnDirectionPressed;
        InputManager_Z.UIInputPressed += TriggerSelectedInMenu;
    }

    void OnDisable()
    {
        InputManager_Z.DirectionPressed -= OnDirectionPressed;
        InputManager_Z.UIInputPressed -= TriggerSelectedInMenu;
    }

    void Start()
    {
        canPress = true;

        audioMenu = GetComponent<AudioMenu>();
        menuScreens = GetComponent<MenuScreens>();

        mainMenuButtons[0].Select();
    }

    private void OnDirectionPressed(Direction direction, InteractionType _)
    {
        if (GameManager.Instance.gameState == GameState.Started) return;

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

            if (menuScreens.menuType == MenuType.Audio)
                audioMenu.ColorSlider(buttonIndex);
        }
        else if (direction == Direction.Down)
        {
            previousIndex = buttonIndex;

            buttonIndex++;

            buttonIndex %= buttons.Count;

            buttons[buttonIndex].Select();
            SFXCollection.Instance.PlaySound(SFXType.SuccessNone);

            if (menuScreens.menuType == MenuType.Audio)
                audioMenu.ColorSlider(buttonIndex);
        }
        else if (direction == Direction.Right || direction == Direction.Left)
        {
            if (menuScreens.menuType == MenuType.Audio)
                audioMenu.MoveSliders(direction, buttonIndex);
        }
    }

    private void TriggerSelectedInMenu(InputType inputType)
    {
        if (inputType == InputType.Confirm)
        {
            TriggerConfirm();
        }
        else if (inputType == InputType.Esc)
        {
            StartCoroutine(TriggerEscape());
        }
    }

    public IEnumerator TriggerEscape()
    {
        if (menuScreens.menuType == MenuType.None && canPress != false)
        {
            GameManager.Instance.ChangeGameState(GameState.Paused);
            menuScreens.SwitchMenus(MenuType.PauseMenu);

            buttons = mainMenuButtons;
            canPress = false;

            yield return new WaitForSeconds(.25f);
        }
        if (menuScreens.menuType != MenuType.None && canPress)
        {
            if (menuScreens.menuType != MenuType.MainMenu)
            // Go back if any menu but MainMenu
            {
                Debug.Log(buttons[^1].name);

                buttons[^1].onClick.Invoke();
            }
            else if (menuScreens.menuType == MenuType.MainMenu && GameManager.Instance.gameState != GameState.PreStart)
            // Press play if MainMenu
            {
                Debug.Log("Resumed " + buttons[0].name);

                buttons[0].onClick.Invoke();
            }

            yield return new WaitForSeconds(.25f);
        }

        canPress = true;
        yield return null;
    }

    private void TriggerConfirm()
    {
        if (buttonIndex <= buttons.Count - 1)
            buttons[buttonIndex].onClick.Invoke();
    }
}
