public enum InputType
{
    Confirm,
    Back,
    Esc
}

public delegate void UIInputPress(InputType inputType);
