using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{
    public List<Button> buttons;
    public List<Slider> sliders;
    public List<Image> sliderFills;
    public MainMenu mainMenu;
    public int _buttonIndex = 0, _previousIndex;
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
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.gameState == GameState.Started) return;
        }

        _previousIndex = _buttonIndex;

        if (direction == Direction.Up)
        {
            _buttonIndex--;
            _buttonIndex %= buttons.Count;

            buttons[_buttonIndex].Select();
            SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
        }
        else if (direction == Direction.Down)
        {
            _buttonIndex++;
            _buttonIndex %= buttons.Count;

            buttons[_buttonIndex].Select();
            SFXCollection.Instance.PlaySound(SFXType.SuccessNone);
        }
        else if (direction == Direction.Right)
        {
            if (_buttonIndex == 1 || _buttonIndex == 2)
                sliders[_buttonIndex - 1].value += 1f;
        }
        else if (direction == Direction.Left)
        {
            if (_buttonIndex == 1 || _buttonIndex == 2)
                sliders[_buttonIndex - 1].value -= 1f;
        }

        ColorSlider();
    }

    private void ColorSlider()
    {
        if (_previousIndex != 1 && _buttonIndex != _previousIndex)
        {
            sliderFills[0].color = _sliderFillColor;
        }
        else if (_previousIndex != 2 && _buttonIndex != _previousIndex)
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
