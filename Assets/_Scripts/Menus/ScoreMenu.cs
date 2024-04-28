using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMenu : MonoBehaviour
{
    private MenuScreens menuScreens;

    void Awake()
    {
        menuScreens = GetComponent<MenuScreens>();
    }

    public void PressBack()
    {
        menuScreens.SwitchMenus(MenuType.MainMenu);
    }
}
