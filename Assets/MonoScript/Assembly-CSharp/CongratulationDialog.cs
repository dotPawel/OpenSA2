using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CongratulationDialog : UIController
{
	public UIButton doneButton;

	public UIButton moreGamesButton;

	public UIButton restartButton;

	public UIIndicator background;

	private bool isFocused;

	[CompilerGenerated]
	private static Action<UIButton> _003C_003Ef__am_0024cache7;

	[method: MethodImpl(32)]
	public event Action<CongratulationDialog> onDoneClicked;

	[method: MethodImpl(32)]
	public event Action<CongratulationDialog> onRestartClicked;

	private void Start()
	{
		doneButton.onClicked += _003CStart_003Em__B;
		UIButton uIButton = moreGamesButton;
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = _003CStart_003Em__C;
		}
		uIButton.onClicked += _003C_003Ef__am_0024cache7;
		restartButton.onClicked += _003CStart_003Em__D;
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("CongratulationDialog");
	}

	public override void ViewWillAppear()
	{
		background.SetState(Player.factionIndex);
	}

	private void Update()
	{
		if (!base.show)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isFocused = true;
		}
		if (isFocused && Input.GetKeyUp(KeyCode.Escape))
		{
			isFocused = false;
			if (this.onDoneClicked != null)
			{
				this.onDoneClicked(this);
			}
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__B(UIButton P_0)
	{
		if (this.onDoneClicked != null)
		{
			this.onDoneClicked(this);
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__C(UIButton P_0)
	{
		AppStoreController.OpenMoreGames();
	}

	[CompilerGenerated]
	private void _003CStart_003Em__D(UIButton P_0)
	{
		if (this.onRestartClicked != null)
		{
			this.onRestartClicked(this);
		}
	}
}
