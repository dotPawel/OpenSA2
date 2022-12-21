public class UIIconButtonView : UIButtonView
{
	public enum State
	{
		Normal = 0,
		Pressed = 1,
		Disabled = 2
	}

	private const int STATES = 3;

	public UIView icon;

	public UIElement[] states = new UIElement[3];

	public State state;

	private bool visible;

	public override void Show(bool value)
	{
		visible = value;
		if (value)
		{
			SetState(state);
			return;
		}
		UIElement[] array = states;
		foreach (UIElement uIElement in array)
		{
			if (uIElement != null)
			{
				uIElement.show = false;
			}
		}
		icon.show = false;
	}

	public override void Press(bool value)
	{
		SetState(value ? State.Pressed : State.Normal);
	}

	public override void Disable(bool value)
	{
		SetState(value ? State.Disabled : State.Normal);
	}

	public void SetState(State value)
	{
		state = value;
		for (int i = 0; i < states.Length; i++)
		{
			if (states[i] != null)
			{
				states[i].show = visible && i == (int)state;
			}
		}
		if (visible)
		{
			icon.show = state != State.Disabled;
		}
	}
}
