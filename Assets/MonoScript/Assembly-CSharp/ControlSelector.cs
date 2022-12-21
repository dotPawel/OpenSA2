using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ControlSelector : UIController
{
	public List<UIButton> buttons;

	[method: MethodImpl(32)]
	public event Action<ControlSelector, int> onControlSelected;

	private void Start()
	{
		foreach (UIButton button in buttons)
		{
			button.onClicked += OnButtonClicked;
		}
	}

	private void OnButtonClicked(UIButton button)
	{
		if (this.onControlSelected != null)
		{
			this.onControlSelected(this, buttons.IndexOf(button));
		}
	}

	public void SetVisible(bool value)
	{
		base.show = value;
	}
}
