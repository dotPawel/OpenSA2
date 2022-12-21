using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MissionScreen : UIController
{
	private const int MISSION_COUNT = 8;

	public UIIndicator background;

	public UIButton startButton;

	public UIButton closeButton;

	public UIText descriptionLabel;

	public UIIndicator icons;

	private bool isFocused;

	public string description
	{
		set
		{
			descriptionLabel.text = value;
		}
	}

	public int icon
	{
		set
		{
			icons.SetState(value);
		}
	}

	[method: MethodImpl(32)]
	public event Action<MissionScreen> onStartClicked;

	[method: MethodImpl(32)]
	public event Action<MissionScreen> onCloseClicked;

	private void Start()
	{
		startButton.onClicked += _003CStart_003Em__17;
		closeButton.onClicked += _003CStart_003Em__18;
	}

	public void Show(Mission mission)
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("MissionScreen");
		icon = (int)((int)mission.setting * 8 + mission.type);
		description = mission.description;
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
			if (this.onCloseClicked != null)
			{
				this.onCloseClicked(this);
			}
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__17(UIButton P_0)
	{
		if (this.onStartClicked != null)
		{
			this.onStartClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__18(UIButton P_0)
	{
		if (this.onCloseClicked != null)
		{
			this.onCloseClicked(this);
		}
	}
}
