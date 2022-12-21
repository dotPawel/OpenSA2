using System;
using System.Runtime.CompilerServices;

public class ConfirmDialog : UIController
{
	public UIIndicator background;

	public UIButton confirmButton;

	public UIButton cancelButton;

	public UIText titleLabel;

	public UIText textLabel;

	[method: MethodImpl(32)]
	public event Action<ConfirmDialog> onCanceled;

	[method: MethodImpl(32)]
	public event Action<ConfirmDialog> onConfirmed;

	private void Start()
	{
		confirmButton.onClicked += _003CStart_003Em__9;
		cancelButton.onClicked += _003CStart_003Em__A;
	}

	public void Show(string title, string text)
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		titleLabel.text = title;
		textLabel.text = text;
		uIStatesController.Show("ConfirmDialog");
	}

	public void Close()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Back();
	}

	[CompilerGenerated]
	private void _003CStart_003Em__9(UIButton P_0)
	{
		if (this.onConfirmed != null)
		{
			this.onConfirmed(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__A(UIButton P_0)
	{
		if (this.onCanceled != null)
		{
			this.onCanceled(this);
		}
	}
}
