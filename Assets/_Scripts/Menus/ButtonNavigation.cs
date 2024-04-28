using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{
    public List<Button> mainMenuButtons, audioMenuButtons, scoreMenuButtons;
    public AudioMenu audioMenu;
    private MenuScreens menuScreens;
    public static int previousIndex;
    public static int buttonIndex = 0;


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
        menuScreens = GetComponent<MenuScreens>();
        mainMenuButtons[0].Select();
    }

    private void OnDirectionPressed(Direction direction, InteractionType _)
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.gameState == GameState.Started) return;
        }

        List<Button> buttons = mainMenuButtons;

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
            mainMenuButtons[buttonIndex].onClick.Invoke();
        }
    }
}
