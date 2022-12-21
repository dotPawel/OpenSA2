public class UIToggle : UIElement
{
	public UIElement normalState;

	public UIElement selectedState;

	public UIElement disableState;

	private bool isDisable;

	public bool selected { get; private set; }

	public bool disable
	{
		set
		{
			isDisable = value;
			disableState.show = isDisable;
			normalState.show = !isDisable && !selected;
			selectedState.show = !isDisable && selected;
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
			base.show = value;
			if (value)
			{
				SetSelect(selected);
				return;
			}
			if (normalState != null)
			{
				normalState.show = false;
			}
			if (selectedState != null)
			{
				selectedState.show = false;
			}
			if (disableState != null)
			{
				disableState.show = false;
			}
		}
	}

	public void SetSelect(bool value)
	{
		selected = value;
		if (show && !isDisable)
		{
			if (normalState != null)
			{
				normalState.show = !value;
			}
			if (selectedState != null)
			{
				selectedState.show = value;
			}
		}
	}
}
