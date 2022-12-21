using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapScreen : UIController
{
	public enum Arrow
	{
		Top = 0,
		Right = 1,
		Bottom = 2,
		Left = 3
	}

	public enum Tip
	{
		None = 0,
		Upgrade = 1
	}

	private const int MIN_UPGRADE_PRICE = 250;

	public UIGameObjectController gameObjectController;

	public UIButton backButton;

	public UIButton hangarButton;

	public UIButton tutorialButton;

	public UIButton missionButton;

	public UIIndicator tipsIndicator;

	public List<MissionButton> missionButtons;

	public MissionButton missionButtonPrefab;

	public float markDepth;

	public Camera uiCamera;

	public AObject[] arrows;

	public Mission tutorialMission;

	public RandomMission randomMission;

	public AudioSource audioSource;

	public bool enableRandomMission;

	private bool isFocused;

	[method: MethodImpl(32)]
	public event Action<Mission> onMissionSelected;

	[method: MethodImpl(32)]
	public event Action<MapScreen> onBackClicked;

	[method: MethodImpl(32)]
	public event Action<MapScreen> onHangarClicked;

	private void Start()
	{
		backButton.onClicked += _003CStart_003Em__13;
		hangarButton.onClicked += _003CStart_003Em__14;
		tutorialButton.onClicked += _003CStart_003Em__15;
		missionButton.onClicked += _003CStart_003Em__16;
	}

	private void LateUpdate()
	{
		for (int i = 0; i < missionButtons.Count; i++)
		{
			MissionButton missionButton = missionButtons[i];
			Vector3 screenPosition = missionButton.GetScreenPosition();
			Vector3 vector = view.transform.InverseTransformPoint(uiCamera.ScreenToWorldPoint(screenPosition));
			missionButton.localPosition = new Vector3(vector.x, vector.y, markDepth);
		}
	}

	public void AddMissionButton(Mission mission)
	{
		MissionButton missionButton = missionButtonPrefab.Instantiate(view.position, view.rotation);
		missionButton.mission = mission;
		missionButton.SetType(mission.type, Player.factionIndex);
		missionButton.parent = view.transform;
		missionButton.show = base.show;
		missionButton.onClicked += OnMissionButtonClicked;
		missionButton.onFocused += OnMissionButtonFocused;
		missionButtons.Add(missionButton);
	}

	private void OnMissionButtonFocused(UIButton button)
	{
		if (audioSource != null)
		{
			audioSource.Play();
		}
	}

	private void OnMissionButtonClicked(UIButton button)
	{
		MissionButton missionButton = (MissionButton)button;
		if (this.onMissionSelected != null)
		{
			this.onMissionSelected(missionButton.mission);
		}
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("MapScreen");
	}

	public void SetArrows(bool[] list)
	{
		for (int i = 0; i < arrows.Length; i++)
		{
			arrows[i].show = list.Length > i && list[i];
		}
	}

	public override void ViewDidAppear()
	{
		if (!Player.tutorial)
		{
			Player.tutorial = true;
			SelectTutorialMission();
		}
		else if (!Player.upgradeTip && Player.credits == Campaign.score && Player.credits >= 250)
		{
			SetTip(Tip.Upgrade);
		}
		missionButton.show = enableRandomMission;
	}

	private void SelectTutorialMission()
	{
		if (this.onMissionSelected != null)
		{
			this.onMissionSelected(tutorialMission);
		}
	}

	private void SetTip(Tip type)
	{
		tipsIndicator.SetState((int)type);
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
			if (this.onBackClicked != null)
			{
				this.onBackClicked(this);
			}
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__13(UIButton P_0)
	{
		if (this.onBackClicked != null)
		{
			this.onBackClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__14(UIButton P_0)
	{
		if (!Player.upgradeTip)
		{
			SetTip(Tip.None);
			Player.upgradeTip = true;
		}
		if (this.onHangarClicked != null)
		{
			this.onHangarClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__15(UIButton P_0)
	{
		SelectTutorialMission();
	}

	[CompilerGenerated]
	private void _003CStart_003Em__16(UIButton P_0)
	{
		randomMission.Randomize();
		if (this.onMissionSelected != null)
		{
			this.onMissionSelected(randomMission);
		}
	}
}
