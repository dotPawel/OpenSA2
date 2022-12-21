public class UIMissionButtonView : UIButtonView
{
	public UIIndicator normal;

	public UIView pressed;

	public UIIndicator icons;

	public override void Show(bool value)
	{
		normal.show = value;
		icons.show = value;
		pressed.show = false;
	}

	public override void Press(bool value)
	{
		normal.show = !value;
		pressed.show = value;
	}

	public void SetIcon(int index)
	{
		icons.SetState(index);
	}

	public void SetBackground(int index)
	{
		normal.SetState(index);
	}
}
