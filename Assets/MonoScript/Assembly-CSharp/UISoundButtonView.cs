public class UISoundButtonView : UIButtonView
{
	public UIView turnOn;

	public UIView turnOff;

	private bool turnOnFlag;

	private bool isVisible;

	public override void Show(bool value)
	{
		isVisible = value;
		turnOn.show = isVisible && turnOnFlag;
		turnOff.show = isVisible && !turnOnFlag;
	}

	public override void Press(bool value)
	{
		if (turnOnFlag)
		{
			turnOn.show = !value;
			turnOff.show = value;
		}
		else
		{
			turnOn.show = value;
			turnOff.show = !value;
		}
	}

	public void SetTurnOn(bool value)
	{
		turnOnFlag = value;
		turnOn.show = isVisible && turnOnFlag;
		turnOff.show = isVisible && !turnOnFlag;
	}
}
