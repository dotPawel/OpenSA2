public class AlertIndicator : UIContainer
{
	public UIView normal;

	public UIView highlight;

	private bool isBlinking;

	public bool blink
	{
		set
		{
			isBlinking = value;
			if (!isBlinking && show)
			{
				normal.show = false;
				highlight.show = false;
			}
		}
	}

	public override bool show
	{
		get
		{
			return base.show;
		}
		set
		{
			normal.show = value && isBlinking;
			highlight.show = false;
			base.gameObject.layer = ((!value) ? 31 : 30);
		}
	}

	public bool UpdateState(bool on)
	{
		if (isBlinking)
		{
			normal.show = !on;
			highlight.show = on;
			return true;
		}
		return false;
	}
}
