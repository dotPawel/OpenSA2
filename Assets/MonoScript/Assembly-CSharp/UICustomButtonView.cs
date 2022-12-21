public class UICustomButtonView : UIButtonView
{
	public UIView normal;

	public UIView pressed;

	public UIElement[] elements;

	private bool isVisible;

	public override void Show(bool value)
	{
		if (normal != null)
		{
			normal.show = value;
		}
		if (pressed != null)
		{
			pressed.show = false;
		}
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i].show = value;
		}
	}

	public override void Press(bool value)
	{
		if (normal != null)
		{
			normal.show = !value;
		}
		if (pressed != null)
		{
			pressed.show = value;
		}
	}
}
