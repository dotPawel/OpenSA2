public class UIUpgradeButtonView : UIButtonView
{
	public UIView normalView;

	public UIView pressedView;

	public UIView disableView;

	public UIIndicator iconView;

	public UIText textView;

	private bool isDisable;

	private bool isVisible;

	public int icon
	{
		set
		{
			iconView.SetState(value);
		}
	}

	public string text
	{
		set
		{
			textView.text = value;
		}
	}

	public override void Show(bool value)
	{
		isVisible = value;
		normalView.show = isVisible && !isDisable;
		disableView.show = isVisible && isDisable;
		pressedView.show = false;
		iconView.show = isVisible;
		textView.show = isVisible;
	}

	public override void Press(bool value)
	{
		normalView.show = !value;
		pressedView.show = value;
	}

	public override void Disable(bool value)
	{
		isDisable = value;
		normalView.show = isVisible && !isDisable;
		disableView.show = isVisible && isDisable;
		pressedView.show = false;
	}
}
