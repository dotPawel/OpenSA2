using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HUD : UIController
{
	public enum Type
	{
		None = 0,
		Clear = 1,
		Air = 2,
		Land = 3
	}

	public UIButton pauseButton;

	public UIRepeatButton moveUpButton;

	public UIRepeatButton moveDownButton;

	public UIRepeatButton fireButton;

	public UIButton bombButton;

	public UIButton takeoffButton;

	public FuelBar fuelBar;

	public TimerBar timerBar;

	public ScoreBar scoreBar;

	public MarkController markController;

	public AirplaneStatusBar airplaneStatusBar;

	public Type type;

	public bool showTimer;

	private bool isFocused;

	[method: MethodImpl(32)]
	public event Action<HUD> onPauseButtonClicked;

	private void Start()
	{
		pauseButton.onClicked += _003CStart_003Em__E;
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("HUD");
	}

	public override void ViewDidAppear()
	{
		SetType(type);
	}

	public void SetType(Type value)
	{
		type = value;
		if (base.show)
		{
			moveUpButton.show = type == Type.Air;
			moveDownButton.show = type == Type.Air;
			fireButton.show = type == Type.Air;
			bombButton.show = type == Type.Air;
			takeoffButton.show = type == Type.Land;
			fuelBar.show = type == Type.Land;
			airplaneStatusBar.show = type > Type.None;
			markController.show = type > Type.None;
			pauseButton.show = type > Type.None;
			scoreBar.show = type > Type.None;
			timerBar.show = showTimer && type > Type.None;
		}
	}

	public void SetTimer(float value)
	{
		showTimer = true;
		timerBar.SetTime(value);
		if (base.show)
		{
			timerBar.show = type > Type.None;
		}
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
			if (this.onPauseButtonClicked != null)
			{
				this.onPauseButtonClicked(this);
			}
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__E(UIButton P_0)
	{
		if (this.onPauseButtonClicked != null)
		{
			this.onPauseButtonClicked(this);
		}
	}
}
