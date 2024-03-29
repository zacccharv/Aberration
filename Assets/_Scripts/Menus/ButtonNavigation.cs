using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{
    public List<Button> buttons;
    public List<Slider> sliders;
    public List<Image> sliderFills;
    public MainMenu mainMenu;
    public int _buttonIndex = 0;
    public bool _selected;
    [SerializeField] private Color _sliderFillColor, _highlightedFillColor;

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
        buttons[0].Select();
    }

    private void OnDirectionPressed(Direction direction, InteractionType _)
    {
        if (GameManager.Instance.gameState == GameState.Started) return;

        if (_buttonIndex == 3)
        {
            if (direction == Direction.Up || direction == Direction.Down)
            {
                ChangeSelected(direction);
                CheckSlider();
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
                CheckSlider();

            }
            else if ((direction == Direction.Up && _buttonIndex == 1) || (direction == Direction.Down && _buttonIndex == 2))
            {
                mainMenu._audioSelected = false;
                _selected = false;
                ChangeSelected(direction);
                CheckSlider();
            }

        }
        else
        {
            ChangeSelected(direction);
            CheckSlider();
        }

    }

    private void CheckSlider()
    {
        if (_buttonIndex != 1)
        {
            sliderFills[0].color = _sliderFillColor;
        }
        else if (_buttonIndex != 2)
        {
            sliderFills[1].color = _sliderFillColor;
        }

        if (_buttonIndex == 1)
        {
            sliderFills[0].color = _highlightedFillColor;
        }
        else if (_buttonIndex == 2)
        {
            sliderFills[1].color = _highlightedFillColor;
        }
        else if (_buttonIndex == 3)
        {
            sliderFills[1].color = _sliderFillColor;
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
            if (_buttonIndex != 1 && _buttonIndex != 2)
            {
                buttons[_buttonIndex].onClick.Invoke();
            }
        }
    }
}
