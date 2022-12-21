using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainMenu : UIController
{
	private const string ALLIES_FACTION = "FactionA";

	private const string AXIS_FACTION = "FactionB";

	private const int ALLIES_CAMPAIGN_ID = 0;

	private const int AXIS_CAMPAIGN_ID = 1;

	private const int ALLIES_AREA_COUNT = 50;

	private const int AXIS_AREA_COUNT = 48;

	public UIButton alliesCampaignButton;

	public UIButton axisCampaignButton;

	public UIButton soundButton;

	public UIButton moreGamesButton;

	private UISoundButtonView soundButtonView;

	private bool isFocused;

	[CompilerGenerated]
	private static Action<UIButton> _003C_003Ef__am_0024cache6;

	[CompilerGenerated]
	private static Action<UIButton> _003C_003Ef__am_0024cache7;

	[CompilerGenerated]
	private static Action<UIButton> _003C_003Ef__am_0024cache8;

	private void Awake()
	{
		Game.sound = Game.sound;
	}

	private void Start()
	{
		UIButton uIButton = alliesCampaignButton;
		if (_003C_003Ef__am_0024cache6 == null)
		{
			_003C_003Ef__am_0024cache6 = _003CStart_003Em__F;
		}
		uIButton.onClicked += _003C_003Ef__am_0024cache6;
		UIButton uIButton2 = axisCampaignButton;
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = _003CStart_003Em__10;
		}
		uIButton2.onClicked += _003C_003Ef__am_0024cache7;
		soundButtonView = (UISoundButtonView)soundButton.view;
		soundButtonView.SetTurnOn(Game.sound);
		soundButton.onClicked += _003CStart_003Em__11;
		UIButton uIButton3 = moreGamesButton;
		if (_003C_003Ef__am_0024cache8 == null)
		{
			_003C_003Ef__am_0024cache8 = _003CStart_003Em__12;
		}
		uIButton3.onClicked += _003C_003Ef__am_0024cache8;
		UICampaignButtonView uICampaignButtonView = (UICampaignButtonView)alliesCampaignButton.view;
		uICampaignButtonView.rank = Player.GetRankById(0);
		uICampaignButtonView.progress = (float)Campaign.GetCapturedAreasCount(0) / 50f;
		UICampaignButtonView uICampaignButtonView2 = (UICampaignButtonView)axisCampaignButton.view;
		uICampaignButtonView2.rank = Player.GetRankById(1);
		uICampaignButtonView2.progress = (float)Campaign.GetCapturedAreasCount(1) / 48f;
	}

	private void Update()
	{
		if (base.show)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				isFocused = true;
			}
			if (isFocused && Input.GetKeyUp(KeyCode.Escape))
			{
				isFocused = false;
				Application.Quit();
			}
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__F(UIButton P_0)
	{
		Campaign.Load(0, "FactionA");
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__10(UIButton P_0)
	{
		Campaign.Load(1, "FactionB");
	}

	[CompilerGenerated]
	private void _003CStart_003Em__11(UIButton P_0)
	{
		bool flag2 = (Game.sound = !Game.sound);
		bool turnOn = flag2;
		soundButtonView.SetTurnOn(turnOn);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__12(UIButton P_0)
	{
		AppStoreController.OpenMoreGames();
	}
}
