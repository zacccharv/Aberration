using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{
    public List<Button> buttons;
    public List<Slider> sliders;
    public MainMenu mainMenu;
    public int _buttonIndex = 0;
    public bool _selected;

    void OnEnable()
    {
        InputMan.DirectionPressed += OnDirectionPressed;
        InputMan.UIInputPressed += TriggerSelected;
    }

    void OnDisable()
    {
        InputMan.DirectionPressed -= OnDirectionPressed;
        InputMan.UIInputPressed -= TriggerSelected;
    }

    void Start()
    {
        buttons[0].Select();
    }

    private void OnDirectionPressed(Direction direction)
    {
        if (_buttonIndex == 3 && _selected)
        {
            if (direction == Direction.Up || direction == Direction.Down)
            {
                TriggerSelected(InputType.Confirm);
                ChangeSelected(direction);
            }
        }
        else if (_buttonIndex == 1 || _buttonIndex == 2)
        {
            _selected = true;

            if (direction == Direction.Right || direction == Direction.Left)
            {
                if (direction == Direction.Right)
                {
                    sliders[_buttonIndex - 1].value += 1f;

                }
                else if (direction == Direction.Left)
                {
                    sliders[_buttonIndex - 1].value -= 1f;
                }

                if (_buttonIndex - 1 == 1)
                {
                    SFXCollection.Instance.PlaySound(SFXType.Success);
                }
            }
            else if ((direction == Direction.Down && _buttonIndex == 1) || (direction == Direction.Up && _buttonIndex == 2))
            {
                ChangeSelected(direction);
            }
            else if ((direction == Direction.Up && _buttonIndex == 1) || (direction == Direction.Down && _buttonIndex == 2))
            {
                mainMenu._audioSelected = false;
                _selected = false;
                ChangeSelected(direction);
            }
        }
        else
        {
            ChangeSelected(direction);
        }
    }

    private void ChangeSelected(Direction direction)
    {

        if (direction == Direction.Up)
        {
            _buttonIndex--;

            if (_buttonIndex == -1)
            {
                _buttonIndex = buttons.Count - 1;
            }
        }
        else if (direction == Direction.Down)
        {
            _buttonIndex++;
        }

        _buttonIndex %= buttons.Count;

        buttons[_buttonIndex].Select();
        SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
    }

    private void TriggerSelected(InputType inputType)
    {
        if (inputType == InputType.Confirm)
        {
            if (!_selected)
            {
                _selected = true;
                SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
                buttons[_buttonIndex].onClick.Invoke();
            }
            else
            {
                _selected = false;
                buttons[_buttonIndex].Select();
            };
        }
    }
}
