using System;
using System.Runtime.CompilerServices;

public class RateDialog : UIController
{
	public UIButton rateButton;

	public UIButton laterButton;

	public UIButton skipButton;

	public UIIndicator background;

	[method: MethodImpl(32)]
	public event Action<RateDialog> onSkipClicked;

	[method: MethodImpl(32)]
	public event Action<RateDialog> onRateClicked;

	[method: MethodImpl(32)]
	public event Action<RateDialog> onLaterClicked;

	private void Start()
	{
		rateButton.onClicked += _003CStart_003Em__30;
		laterButton.onClicked += _003CStart_003Em__31;
		skipButton.onClicked += _003CStart_003Em__32;
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		if (uIStatesController != null)
		{
			uIStatesController.Show("RateDialog");
		}
	}

	public void Close()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		if (uIStatesController != null)
		{
			uIStatesController.Back();
		}
	}

	public override void ViewWillAppear()
	{
		background.SetState(Player.factionIndex);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__30(UIButton P_0)
	{
		if (this.onRateClicked != null)
		{
			this.onRateClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__31(UIButton P_0)
	{
		if (this.onLaterClicked != null)
		{
			this.onLaterClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__32(UIButton P_0)
	{
		if (this.onSkipClicked != null)
		{
			this.onSkipClicked(this);
		}
	}
}
