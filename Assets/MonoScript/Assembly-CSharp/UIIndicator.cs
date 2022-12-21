using System.Collections.Generic;

public class UIIndicator : UIElement
{
	public List<UIElement> states;

	public int state;

	public override bool show
	{
		get
		{
			return base.show;
		}
		set
		{
			base.show = value;
			SetVisible(value);
		}
	}

	public void SetState(int state)
	{
		for (int i = 0; i < states.Count; i++)
		{
			if (states[i] != null)
			{
				states[i].show = show && i == state;
			}
		}
		this.state = state;
	}

	protected void SetVisible(bool flag)
	{
		if (flag)
		{
			SetState(state);
		}
		else
		{
			Hide();
		}
	}

	protected void Hide()
	{
		for (int i = 0; i < states.Count; i++)
		{
			if (states[i] != null)
			{
				states[i].show = false;
			}
		}
	}
}
