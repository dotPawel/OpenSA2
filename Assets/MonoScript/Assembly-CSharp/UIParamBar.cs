using UnityEngine;

public class UIParamBar : UIContainer
{
	public UIToggle[] elements;

	public int value;

	public override bool show
	{
		get
		{
			return base.show;
		}
		set
		{
			base.show = value;
			SetValue(this.value);
		}
	}

	[ContextMenu("Update state")]
	private void UpdateValue()
	{
		SetValue(value);
	}

	public void SetValue(int value)
	{
		this.value = value;
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i].SetSelect(i < value);
		}
	}

	public void SetAvaible(int value)
	{
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i].disable = i >= value;
		}
	}
}
