using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PauseMenu : UIController
{
	public UIButton resumeButton;

	public UIButton restartButton;

	public UIButton exitButton;

	public UIIndicator background;

	public UIText descriptionLabel;

	public List<UITabButton> controlButtons;

	public List<UITabButton> soundButtons;

	private int selectedControl;

	private int selectedSound;

	private bool isFocused;

	public string description
	{
		set
		{
			descriptionLabel.text = value;
		}
	}

	[method: MethodImpl(32)]
	public event Action<PauseMenu> onResumeClicked;

	[method: MethodImpl(32)]
	public event Action<PauseMenu> onRestartClicked;

	[method: MethodImpl(32)]
	public event Action<PauseMenu> onExitClicked;

	[method: MethodImpl(32)]
	public event Action<PauseMenu, int> onControlUpdated;

	[method: MethodImpl(32)]
	public event Action<PauseMenu, bool> onSoundUpdated;

	private void Start()
	{
		resumeButton.onClicked += _003CStart_003Em__19;
		restartButton.onClicked += _003CStart_003Em__1A;
		exitButton.onClicked += _003CStart_003Em__1B;
		for (int i = 0; i < controlButtons.Count; i++)
		{
			controlButtons[i].onClicked += OnControlButtonClicked;
		}
		for (int j = 0; j < soundButtons.Count; j++)
		{
			soundButtons[j].onClicked += OnSoundButtonClicked;
		}
	}

	private void OnSoundButtonClicked(UITabButton button)
	{
		int num = soundButtons.IndexOf(button);
		SetSound(num > 0);
		if (this.onSoundUpdated != null)
		{
			this.onSoundUpdated(this, num > 0);
		}
	}

	private void OnControlButtonClicked(UITabButton button)
	{
		int num = controlButtons.IndexOf(button);
		SetControl(num);
		if (this.onControlUpdated != null)
		{
			this.onControlUpdated(this, num);
		}
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("PauseMenu");
		Game.SetPause(true);
	}

	public void Close()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Back();
		Game.SetPause(false);
	}

	public void SetControl(int value)
	{
		selectedControl = value;
		for (int i = 0; i < controlButtons.Count; i++)
		{
			controlButtons[i].selected = i == selectedControl;
		}
	}

	public void SetSound(bool value)
	{
		selectedSound = (value ? 1 : 0);
		for (int i = 0; i < soundButtons.Count; i++)
		{
			soundButtons[i].selected = i == selectedSound;
		}
	}

	public override void ViewDidAppear()
	{
		background.SetState(Player.factionIndex);
	}

	private void OnDisable()
	{
		Game.SetPause(false);
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
			if (this.onResumeClicked != null)
			{
				this.onResumeClicked(this);
			}
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__19(UIButton P_0)
	{
		if (this.onResumeClicked != null)
		{
			this.onResumeClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1A(UIButton P_0)
	{
		if (this.onRestartClicked != null)
		{
			this.onRestartClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1B(UIButton P_0)
	{
		if (this.onExitClicked != null)
		{
			this.onExitClicked(this);
		}
	}
}
