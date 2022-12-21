public class UIControlButtonView : UIButtonView
{
	public UIIndicator normal;

	public UIIndicator pressed;

	private bool visible;

	public override void Show(bool value)
	{
		visible = value;
		normal.show = visible;
		pressed.show = false;
	}

	public override void Press(bool value)
	{
		normal.show = visible && !value;
		pressed.show = visible && value;
	}

	public void SetIcon(int index)
	{
		normal.SetState(index);
		pressed.SetState(index);
	}
}
