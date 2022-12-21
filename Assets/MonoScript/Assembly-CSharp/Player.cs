using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : AObject
{
	private const string REFUELING = "REFUELING IS COMPLETE";

	private const string COMPLETE = "MISSION COMPLETED";

	private const string FAIL = "MISSION FAILED";

	private const string DEFAUL_FACTION = "FactionA";

	private const float STATISTICS_DELAY = 2f;

	private const float THEME_DELAY = 2f;

	private const float AIRPORT_RANGE = 3f;

	public List<PlayerListener> listeners;

	public ScoreController scoreController;

	public DynamicCamera dynamicCamera;

	public Autopilot autopilot;

	public Airplane airplane;

	public AudioSource audioSource;

	private static Faction factionReference;

	private Scenario scenario;

	private AirplaneScreen airplaneScreen;

	private PauseMenu pauseMenu;

	private AirplaneProfiler airplaneProfiler;

	private SoundManager soundManager;

	private Vector2 gameMessageLocation = new Vector2(0f, 220f);

	private static Player playerReference;

	private HUD hudReference;

	[CompilerGenerated]
	private static Action<PauseMenu> _003C_003Ef__am_0024cacheF;

	[CompilerGenerated]
	private static Action<PauseMenu> _003C_003Ef__am_0024cache10;

	[CompilerGenerated]
	private static Action<PauseMenu, bool> _003C_003Ef__am_0024cache11;

	private static string VICTORIES_KEY
	{
		get
		{
			return string.Format("{0}.player.victories", Campaign.id);
		}
	}

	private static string GAMES_KEY
	{
		get
		{
			return string.Format("{0}.player.games", Campaign.id);
		}
	}

	private static string RANK_KEY
	{
		get
		{
			return string.Format("{0}.player.rank", Campaign.id);
		}
	}

	private static string CREDITS_KEY
	{
		get
		{
			return string.Format("{0}.player.credits", Campaign.id);
		}
	}

	private static string CONTROL_KEY
	{
		get
		{
			return string.Format("player.control");
		}
	}

	private static string AIRPLANE_KEY
	{
		get
		{
			return string.Format("{0}.player.airplane", Campaign.id);
		}
	}

	private static string EQUIPMENT_KEY
	{
		get
		{
			return string.Format("{0}.player.equipment", Campaign.id);
		}
	}

	private static string FACTION_KEY
	{
		get
		{
			return string.Format("{0}.player.faction", Campaign.id);
		}
	}

	private static string TUTORIAL_KEY
	{
		get
		{
			return "player.tutorial";
		}
	}

	private static string UPGRADE_TIP_KEY
	{
		get
		{
			return "player.tips.upgrade";
		}
	}

	public static int rank
	{
		get
		{
			return PlayerPrefs.GetInt(RANK_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(RANK_KEY, value);
		}
	}

	public static int credits
	{
		get
		{
			return PlayerPrefs.GetInt(CREDITS_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(CREDITS_KEY, value);
		}
	}

	public static int victories
	{
		get
		{
			return PlayerPrefs.GetInt(VICTORIES_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(VICTORIES_KEY, value);
		}
	}

	public static int games
	{
		get
		{
			return PlayerPrefs.GetInt(GAMES_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(GAMES_KEY, value);
		}
	}

	public static ControlType controlType
	{
		get
		{
			return (ControlType)PlayerPrefs.GetInt(CONTROL_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(CONTROL_KEY, (int)value);
		}
	}

	public static AirplaneType airplaneType
	{
		get
		{
			return (AirplaneType)PlayerPrefs.GetInt(AIRPLANE_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(AIRPLANE_KEY, (int)value);
		}
	}

	public static int airplaneEquipment
	{
		get
		{
			return PlayerPrefs.GetInt(EQUIPMENT_KEY, 1);
		}
		set
		{
			PlayerPrefs.SetInt(EQUIPMENT_KEY, value);
		}
	}

	public static bool tutorial
	{
		get
		{
			return PlayerPrefs.GetInt(TUTORIAL_KEY, 0) > 0;
		}
		set
		{
			PlayerPrefs.SetInt(TUTORIAL_KEY, value ? 1 : 0);
		}
	}

	public static bool upgradeTip
	{
		get
		{
			return PlayerPrefs.GetInt(UPGRADE_TIP_KEY, 0) > 0;
		}
		set
		{
			PlayerPrefs.SetInt(UPGRADE_TIP_KEY, value ? 1 : 0);
		}
	}

	public static Faction faction
	{
		get
		{
			if (factionReference == null)
			{
				factionReference = Faction.Find(PlayerPrefs.GetString(FACTION_KEY, "FactionA"));
			}
			return factionReference;
		}
		set
		{
			PlayerPrefs.GetString(FACTION_KEY, faction.tag);
		}
	}

	public static int factionIndex
	{
		get
		{
			return Faction.GetIndexByTag(PlayerPrefs.GetString(FACTION_KEY, "FactionA"));
		}
	}

	private HUD hud
	{
		get
		{
			if (hudReference == null)
			{
				hudReference = UIController.Find<HUD>();
			}
			return hudReference;
		}
	}

	public int id
	{
		get
		{
			return (!(airplane != null)) ? (-1) : airplane.id;
		}
	}

	public static Player Find()
	{
		if (playerReference == null)
		{
			playerReference = UnityEngine.Object.FindObjectOfType(typeof(Player)) as Player;
		}
		return playerReference;
	}

	private void Awake()
	{
		listeners.AddRange(GetComponents<PlayerListener>());
		scenario = Scenario.Find();
		scenario.onScenarioStarted += OnScenarioScenarioStarted;
		scenario.onScenarioFinished += OnScenarioScenarioFinished;
	}

	private void Start()
	{
		scoreController.onScoreUpdated += OnScoreUpdated;
		hud.onPauseButtonClicked += OnPauseButtonClicked;
		hud.scoreBar.icon = factionIndex;
		soundManager = SoundManager.Find();
		if (soundManager != null && soundManager.isPlaying && soundManager.mainTheme)
		{
			soundManager.FadeOut();
		}
	}

	private void OnPauseButtonClicked(HUD hud)
	{
		ShowPauseMenu();
	}

	private void OnScoreUpdated(ScoreController scoreController, int score)
	{
		hud.scoreBar.score = score;
	}

	private void OnScenarioScenarioFinished(Scenario scenario)
	{
		hud.SetType(HUD.Type.None);
		autopilot.SetAirplane(airplane);
		GameMessenger.Message((!scenario.completed) ? "MISSION FAILED" : "MISSION COMPLETED", gameMessageLocation);
		if (soundManager != null)
		{
			soundManager.FadeIn(scenario.completed ? SoundManager.ThemeType.Victory : SoundManager.ThemeType.Defeat);
		}
		Invoke("ShowStatictics", 2f);
	}

	private void OnScenarioScenarioStarted(Scenario scenario)
	{
		dynamicCamera.SetPosition(scenario.playerSpawner.airport.position);
		if (scenario.supplyAvailable)
		{
			scenario.playerSpawner.onSupplyDelivered += OnSupplyDelivered;
		}
		if (scenario.airplaneSelectionAvailable)
		{
			airplaneScreen = UIController.Find<AirplaneScreen>();
			airplaneScreen.onCompleted += _003COnScenarioScenarioStarted_003Em__20;
			airplaneScreen.Show();
		}
	}

	private void OnSupplyDelivered(PlayerSpawner playerSpawner, Vector3 point)
	{
		if (airplane != null && airplane.movement.state == AirMovement.State.Landed && Mathf.Abs(airplane.position.x - point.x) < 1f)
		{
			GameMessenger.Message("REFUELING IS COMPLETE", gameMessageLocation);
			airplane.Fill();
			audioSource.Play();
		}
	}

	public void Takeoff()
	{
		Airplane airplane = scenario.playerSpawner.SpawnAirplane(airplaneType);
		SetAirplane(airplane);
		hud.SetType(HUD.Type.Clear);
		hud.Show();
	}

	public void SetAirplane(Airplane airplane)
	{
		if (this.airplane != null)
		{
			this.airplane.onDestroyed -= OnAirplaneDestroyed;
			this.airplane.movement.onStateChanged -= OnAirplaneMovementStateChanged;
		}
		this.airplane = airplane;
		if (airplane != null)
		{
			airplane.onDestroyed += OnAirplaneDestroyed;
			airplane.movement.onStateChanged += OnAirplaneMovementStateChanged;
			airplane.SetEquipment(airplaneEquipment);
			if (airplaneProfiler == null)
			{
				airplaneProfiler = AirplaneProfiler.Load();
			}
			AirplaneProfiler.AirplaneInfo airplaneInfo = airplaneProfiler.GetAirplaneInfo(factionIndex, airplaneType);
			airplane.SetArmor(airplaneInfo.armor);
			airplane.movement.SetMobility(airplaneInfo.mobility);
			airplane.machineGun.SetPower(airplaneInfo.damage);
			airplane.SetAvailabelLanding(true);
			airplane.view.title = false;
			airplane.immortal = true;
			airplane.SetSoundType(0);
		}
		for (int i = 0; i < listeners.Count; i++)
		{
			listeners[i].OnAirplaneSet(airplane);
		}
	}

	public void SetControlType(ControlType value)
	{
		controlType = value;
		for (int i = 0; i < listeners.Count; i++)
		{
			listeners[i].OnControlTypeUpdated(value);
		}
	}

	public void RemoveAirplane()
	{
		if (airplane != null)
		{
			airplane.onDestroyed -= OnAirplaneDestroyed;
			airplane.movement.onStateChanged -= OnAirplaneMovementStateChanged;
			for (int i = 0; i < listeners.Count; i++)
			{
				listeners[i].OnAirplaneSet(null);
			}
			airplane.Destroy();
		}
	}

	public static bool Payment(int value, bool test = false)
	{
		if (test)
		{
			return credits >= value;
		}
		if (credits >= value)
		{
			credits -= value;
			return true;
		}
		return false;
	}

	public static void Reset()
	{
		PlayerPrefs.DeleteKey(RANK_KEY);
		PlayerPrefs.DeleteKey(CREDITS_KEY);
		PlayerPrefs.DeleteKey(GAMES_KEY);
		PlayerPrefs.DeleteKey(VICTORIES_KEY);
		PlayerPrefs.DeleteKey(UPGRADE_TIP_KEY);
	}

	public static void SetFaction(string value)
	{
		PlayerPrefs.SetString(FACTION_KEY, value);
	}

	public static int GetRankById(int id)
	{
		return PlayerPrefs.GetInt(string.Format("{0}.player.rank", id), 0);
	}

	private void OnAirplaneDestroyed(Target airplane)
	{
		SetAirplane(null);
		scenario.FinishScenario(false);
	}

	private void OnAirplaneMovementStateChanged(AirMovement movement, AirMovement.State state)
	{
		switch (state)
		{
		case AirMovement.State.Takeoff:
		case AirMovement.State.Landing:
			hud.SetType(HUD.Type.Clear);
			break;
		case AirMovement.State.Landed:
			if (scenario.finished)
			{
				break;
			}
			if (Utils.IsXRangeLow(movement.position, scenario.playerSpawner.airport.position, 3f))
			{
				if (!scenario.SessionComplete())
				{
					if (scenario.airplaneSelectionAvailable)
					{
						RemoveAirplane();
						airplaneScreen.Show();
					}
					else
					{
						hud.SetType(HUD.Type.Land);
					}
				}
			}
			else
			{
				if (scenario.supplyAvailable)
				{
					float fillAnimation = scenario.playerSpawner.AskSupply(airplane.position);
					hud.fuelBar.SetFillAnimation(fillAnimation);
				}
				hud.SetType(HUD.Type.Land);
			}
			break;
		case AirMovement.State.Flying:
			if (airplane.immortal)
			{
				airplane.immortal = false;
			}
			hud.SetType(HUD.Type.Air);
			break;
		case AirMovement.State.Falling:
			break;
		}
	}

	private void ShowPauseMenu()
	{
		if (pauseMenu == null)
		{
			pauseMenu = UIController.Find<PauseMenu>();
			PauseMenu obj = pauseMenu;
			if (_003C_003Ef__am_0024cacheF == null)
			{
				_003C_003Ef__am_0024cacheF = _003CShowPauseMenu_003Em__21;
			}
			obj.onExitClicked += _003C_003Ef__am_0024cacheF;
			pauseMenu.onResumeClicked += _003CShowPauseMenu_003Em__22;
			PauseMenu obj2 = pauseMenu;
			if (_003C_003Ef__am_0024cache10 == null)
			{
				_003C_003Ef__am_0024cache10 = _003CShowPauseMenu_003Em__23;
			}
			obj2.onRestartClicked += _003C_003Ef__am_0024cache10;
			pauseMenu.onControlUpdated += _003CShowPauseMenu_003Em__24;
			PauseMenu obj3 = pauseMenu;
			if (_003C_003Ef__am_0024cache11 == null)
			{
				_003C_003Ef__am_0024cache11 = _003CShowPauseMenu_003Em__25;
			}
			obj3.onSoundUpdated += _003C_003Ef__am_0024cache11;
			pauseMenu.description = Mission.GetDescription();
			pauseMenu.SetControl((int)controlType);
			pauseMenu.SetSound(Game.sound);
		}
		pauseMenu.Show();
	}

	private void ShowStatictics()
	{
		StatisticsScreen statisticsScreen = UIController.Find<StatisticsScreen>();
		statisticsScreen.onDoneButtonClicked += _003CShowStatictics_003Em__26;
		statisticsScreen.onExitButtonClicked += _003CShowStatictics_003Em__27;
		statisticsScreen.onRestartButtonClicked += _003CShowStatictics_003Em__28;
		statisticsScreen.SetStatictics(rank, scoreController.score, scoreController.GetPointsCount(ScoreController.FragType.Infantry), scoreController.GetPointsCount(ScoreController.FragType.Tank), scoreController.GetPointsCount(ScoreController.FragType.Airplane), scoreController.GetPointsCount(ScoreController.FragType.Structure));
		games++;
		if (scenario.completed)
		{
			victories++;
			if (rank == PlayerRankSystem.maxRank)
			{
				statisticsScreen.SetProgress(1f);
				credits += scoreController.score;
				Mission.Complete(scoreController.score);
			}
			else
			{
				float playerRankProgress = PlayerRankSystem.GetPlayerRankProgress(rank, Campaign.score);
				credits += scoreController.score;
				Mission.Complete(scoreController.score);
				float playerRankProgress2 = PlayerRankSystem.GetPlayerRankProgress(rank, Campaign.score);
				if (playerRankProgress2 == 1f)
				{
					rank = PlayerRankSystem.GetPlayerRank(Campaign.score);
					playerRankProgress2 = PlayerRankSystem.GetPlayerRankProgress(rank, Campaign.score);
					statisticsScreen.SetProgress(playerRankProgress, playerRankProgress2, rank);
				}
				else
				{
					statisticsScreen.SetProgress(playerRankProgress, playerRankProgress2);
				}
			}
		}
		else
		{
			victories = 0;
			statisticsScreen.SetProgress(PlayerRankSystem.GetPlayerRankProgress(rank, Campaign.score));
		}
		statisticsScreen.Show((!scenario.completed) ? StatisticsScreen.Type.Defeat : StatisticsScreen.Type.Victory);
	}

	private void OnDrawGizmosSelected()
	{
		if (scenario != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(scenario.playerSpawner.airport.position, 3f);
		}
	}

	[ContextMenu("Set credits")]
	private void SetCredits()
	{
		credits = 12000;
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause && scenario != null && !scenario.finished && (pauseMenu == null || !pauseMenu.show))
		{
			ShowPauseMenu();
		}
	}

	[CompilerGenerated]
	private void _003COnScenarioScenarioStarted_003Em__20(AirplaneScreen P_0)
	{
		Takeoff();
	}

	[CompilerGenerated]
	private static void _003CShowPauseMenu_003Em__21(PauseMenu P_0)
	{
		Game.LoadCampaign();
	}

	[CompilerGenerated]
	private void _003CShowPauseMenu_003Em__22(PauseMenu P_0)
	{
		pauseMenu.Close();
	}

	[CompilerGenerated]
	private static void _003CShowPauseMenu_003Em__23(PauseMenu P_0)
	{
		Game.RestartLevel();
	}

	[CompilerGenerated]
	private void _003CShowPauseMenu_003Em__24(PauseMenu menu, int index)
	{
		SetControlType((ControlType)index);
	}

	[CompilerGenerated]
	private static void _003CShowPauseMenu_003Em__25(PauseMenu menu, bool sound)
	{
		Game.sound = sound;
	}

	[CompilerGenerated]
	private void _003CShowStatictics_003Em__26(StatisticsScreen P_0)
	{
		if (soundManager != null && soundManager.isPlaying)
		{
			soundManager.FadeOut();
		}
		Game.LoadCampaign();
	}

	[CompilerGenerated]
	private void _003CShowStatictics_003Em__27(StatisticsScreen P_0)
	{
		if (soundManager != null && soundManager.isPlaying)
		{
			soundManager.FadeOut();
		}
		Game.LoadCampaign();
	}

	[CompilerGenerated]
	private void _003CShowStatictics_003Em__28(StatisticsScreen P_0)
	{
		if (soundManager != null && soundManager.isPlaying)
		{
			soundManager.FadeOut();
		}
		Game.RestartLevel();
	}
}
