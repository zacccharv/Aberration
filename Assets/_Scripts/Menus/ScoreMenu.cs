using UnityEngine;

public class ScoreMenu : MonoBehaviour
{
    private MenuScreens _menuScreens;

    void Awake()
    {
        _menuScreens = GetComponent<MenuScreens>();
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
}
