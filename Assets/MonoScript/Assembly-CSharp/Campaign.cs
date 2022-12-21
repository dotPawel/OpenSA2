using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Campaign : MonoBehaviour
{
	private const char SEPARATOR = ',';

	private const string DEFAULT_FACTION = "FactionA";

	private const int TOTAL_AREA_COUNT = 50;

	public CampaignFaction[] factions;

	public MapControl mapControl;

	public CampaignArea[] areas;

	public int maxMissionCount = 3;

	public float minMissionRange = 3f;

	public MapArrowController mapArrowController;

	public RateAppController rateAppController;

	private List<CampaignArea> controlAreas = new List<CampaignArea>();

	private List<CampaignArea> missionAreas = new List<CampaignArea>();

	private CampaignArea[] centerAreas;

	private MapScreen mapScreen;

	private AirplaneHangar airplaneHangar;

	private PlayerProfile playerProfile;

	private ConfirmDialog confirmDialog;

	private MissionScreen missionScreen;

	private Mission selectedMission;

	private CampaignFaction campaignFaction;

	private static string FACTION_KEY
	{
		get
		{
			return string.Format("{0}.campaign.faction", id);
		}
	}

	private static string SCORE_KEY
	{
		get
		{
			return string.Format("{0}.campaign.score", id);
		}
	}

	private static string AREAS_KEY
	{
		get
		{
			return string.Format("{0}.campaign.areas", id);
		}
	}

	private static string CONGRATULATION_KEY
	{
		get
		{
			return string.Format("{0}.campaign.congratulation", id);
		}
	}

	public static int id
	{
		get
		{
			return PlayerPrefs.GetInt("campaign.id", 0);
		}
		set
		{
			PlayerPrefs.SetInt("campaign.id", value);
		}
	}

	public static string faction
	{
		get
		{
			return PlayerPrefs.GetString(FACTION_KEY, "FactionA");
		}
		set
		{
			PlayerPrefs.SetString(FACTION_KEY, value);
		}
	}

	public static int score
	{
		get
		{
			return PlayerPrefs.GetInt(SCORE_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(SCORE_KEY, value);
		}
	}

	public static bool congratulation
	{
		get
		{
			return PlayerPrefs.GetInt(CONGRATULATION_KEY, 0) > 0;
		}
		set
		{
			PlayerPrefs.SetInt(CONGRATULATION_KEY, value ? 1 : 0);
		}
	}

	public static Campaign Find()
	{
		return (Campaign)Object.FindObjectOfType(typeof(Campaign));
	}

	public static void Load(int campaignId, string campaignFaction)
	{
		id = campaignId;
		faction = campaignFaction;
		Player.SetFaction(campaignFaction);
		Game.LoadCampaign();
	}

	private void Start()
	{
		campaignFaction = GetCampaignFaction();
		controlAreas.AddRange(campaignFaction.controlAreas);
		centerAreas = campaignFaction.controlAreas;
		List<string> list = new List<string>(GetCapturedAreas());
		CampaignArea[] array = areas;
		foreach (CampaignArea campaignArea in array)
		{
			if (list.Contains(campaignArea.name) && !controlAreas.Contains(campaignArea))
			{
				controlAreas.Add(campaignArea);
			}
		}
		CampaignArea[] array2 = areas;
		foreach (CampaignArea campaignArea2 in array2)
		{
			campaignArea2.SetFaction((!controlAreas.Contains(campaignArea2)) ? campaignFaction.enemy : campaignFaction);
		}
		missionAreas = GetMissionAreas(campaignFaction, maxMissionCount, minMissionRange);
		if (missionAreas.Count > 0)
		{
			mapControl.SetPosition(missionAreas[0].position);
			if (rateAppController.Check())
			{
				rateAppController.onDialogClosed += _003CStart_003Em__0;
			}
			else
			{
				ShowMapScreen();
			}
		}
		else if (congratulation)
		{
			ShowMapScreen(true);
		}
		else
		{
			CongratulationDialog congratulationDialog = UIController.Find<CongratulationDialog>();
			congratulationDialog.Show();
			congratulationDialog.onDoneClicked += _003CStart_003Em__1;
			congratulationDialog.onRestartClicked += _003CStart_003Em__2;
			congratulation = true;
		}
		SoundManager soundManager = SoundManager.Find();
		if (soundManager != null && !soundManager.mainTheme)
		{
			soundManager.FadeIn();
		}
	}

	private void ShowMapScreen(bool randomMission = false)
	{
		playerProfile = UIController.Find<PlayerProfile>();
		playerProfile.rank = Player.rank;
		playerProfile.score = Player.credits;
		playerProfile.SetTheme(Player.factionIndex);
		mapScreen = UIController.Find<MapScreen>();
		mapScreen.onMissionSelected += OnMissionSelected;
		mapScreen.onHangarClicked += OnHangarClicked;
		mapScreen.onBackClicked += OnBackClicked;
		mapScreen.enableRandomMission = randomMission;
		missionScreen = UIController.Find<MissionScreen>();
		missionScreen.onCloseClicked += OnMissionClose;
		missionScreen.onStartClicked += OnMissionStart;
		for (int i = 0; i < missionAreas.Count; i++)
		{
			CampaignArea campaignArea = missionAreas[i];
			CampaignArea connectedAreaInRange = campaignArea.GetConnectedAreaInRange(campaignFaction);
			if (connectedAreaInRange != null)
			{
				mapArrowController.AddArrow(connectedAreaInRange.position, campaignArea.position);
			}
			mapScreen.AddMissionButton(campaignArea.mission);
		}
		airplaneHangar = UIController.Find<AirplaneHangar>();
		airplaneHangar.onBackClicked += OnHangarBackClicked;
		airplaneHangar.onPurchased += OnHangarPurchased;
		mapScreen.Show();
	}

	private void OnBackClicked(MapScreen mapScreen)
	{
		mapArrowController.show = false;
		Game.LoadMainMenu();
	}

	private void OnConfirmDialogConfirmed(ConfirmDialog confirmDialog)
	{
		ResetProgress();
		Game.RestartLevel();
	}

	private void OnConfirmDialogCanceled(ConfirmDialog confirmDialog)
	{
		confirmDialog.Close();
	}

	private void OnHangarPurchased(AirplaneHangar airplaneHangar)
	{
		playerProfile.score = Player.credits;
	}

	private void OnHangarBackClicked(AirplaneHangar airplaneHangar)
	{
		mapScreen.Show();
		mapArrowController.show = true;
	}

	private void OnHangarClicked(MapScreen mapScreen)
	{
		airplaneHangar.Show();
		mapArrowController.show = false;
	}

	private void OnMissionStart(MissionScreen missionScreen)
	{
		selectedMission.Load();
	}

	private void OnMissionClose(MissionScreen missionScreen)
	{
		selectedMission = null;
		mapArrowController.show = true;
		mapScreen.Show();
	}

	private void OnMissionSelected(Mission mission)
	{
		selectedMission = mission;
		mapArrowController.show = false;
		missionScreen.Show(mission);
	}

	private CampaignFaction GetCampaignFaction()
	{
		CampaignFaction[] array = factions;
		foreach (CampaignFaction campaignFaction in array)
		{
			if (campaignFaction.tag == faction)
			{
				return campaignFaction;
			}
		}
		return null;
	}

	private List<CampaignArea> GetMissionAreas(CampaignFaction faction, int maxCount, float minRange)
	{
		List<CampaignArea> list = new List<CampaignArea>();
		List<CampaignArea> list2 = new List<CampaignArea>();
		CampaignArea[] array = centerAreas;
		foreach (CampaignArea startArea in array)
		{
			List<CampaignArea> attackAreas = GetAttackAreas(startArea, faction);
			foreach (CampaignArea item in attackAreas)
			{
				if (!list2.Contains(item))
				{
					list2.Add(item);
				}
			}
		}
		list2.Sort(new CampaignArea.ConnectionComparer(faction.enemy));
		while (list.Count < maxCount && list2.Count > 0)
		{
			CampaignArea campaignArea = list2[0];
			list2.Remove(campaignArea);
			foreach (CampaignArea item2 in list)
			{
				if (Utils.IsDistanceLow(item2.position, campaignArea.position, minRange))
				{
					campaignArea = null;
					break;
				}
			}
			if (campaignArea != null)
			{
				list.Add(campaignArea);
			}
		}
		return list;
	}

	private List<CampaignArea> GetAttackAreas(CampaignArea startArea, CampaignFaction faction)
	{
		Queue<CampaignArea> queue = new Queue<CampaignArea>();
		HashSet<CampaignArea> hashSet = new HashSet<CampaignArea>();
		List<CampaignArea> list = new List<CampaignArea>();
		queue.Clear();
		hashSet.Clear();
		queue.Enqueue(startArea);
		while (queue.Count > 0)
		{
			CampaignArea campaignArea = queue.Dequeue();
			hashSet.Add(campaignArea);
			if (campaignArea.faction != faction)
			{
				list.Add(campaignArea);
				continue;
			}
			foreach (CampaignArea connection in campaignArea.connections)
			{
				if (!hashSet.Contains(connection))
				{
					queue.Enqueue(connection);
				}
			}
		}
		return list;
	}

	public static string[] GetCapturedAreas()
	{
		string @string = PlayerPrefs.GetString(AREAS_KEY, string.Empty);
		return (@string.Length <= 0) ? new string[0] : @string.Split(',');
	}

	public static void AddCapturedArea(string name)
	{
		if (name.Length > 0)
		{
			string @string = PlayerPrefs.GetString(AREAS_KEY, string.Empty);
			PlayerPrefs.SetString(value: (@string.Length <= 0) ? name : (@string + string.Format("{0}{1}", ',', name)), key: AREAS_KEY);
		}
	}

	public static void IncreaseScore(int value)
	{
		score += value;
	}

	public static void ResetProgress(int id)
	{
		PlayerPrefs.DeleteKey(string.Format("{0}.areas", id));
	}

	public static int GetCapturedAreasCount(int id)
	{
		string @string = PlayerPrefs.GetString(string.Format("{0}.campaign.areas", id), string.Empty);
		return (@string.Length > 0) ? @string.Split(',').Length : 0;
	}

	[ContextMenu("Get All Areas")]
	private void GetAllAreas()
	{
		areas = (CampaignArea[])Object.FindObjectsOfType(typeof(CampaignArea));
	}

	[ContextMenu("Reset Progress")]
	public void ResetProgress()
	{
		PlayerPrefs.DeleteKey(CONGRATULATION_KEY);
		PlayerPrefs.DeleteKey(AREAS_KEY);
		PlayerPrefs.DeleteKey(SCORE_KEY);
		Mission[] array = (Mission[])Object.FindObjectsOfType(typeof(Mission));
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reset();
		}
		Player.Reset();
		AirplaneProfiler airplaneProfiler = AirplaneProfiler.Load();
		airplaneProfiler.ResetData();
	}

	[ContextMenu("Randomize missions")]
	public void RandomizeMissions()
	{
		List<Mission> list = new List<Mission>((Mission[])Object.FindObjectsOfType(typeof(Mission)));
		list.Sort(new Mission.SubmissionComparer());
		HashSet<Mission> hashSet = new HashSet<Mission>();
		Dictionary<Mission.Type, int> dictionary = new Dictionary<Mission.Type, int>();
		for (int i = 0; i < 7; i++)
		{
			Mission.Type type = (Mission.Type)i;
			dictionary.Add(type, Mission.GetLevelCount(type));
		}
		foreach (Mission item in list)
		{
			List<Mission.Type> list2 = new List<Mission.Type>();
			foreach (Mission.Type key3 in dictionary.Keys)
			{
				if (dictionary[key3] > 0)
				{
					list2.Add(key3);
				}
			}
			foreach (Mission submission in item.submissions)
			{
				if (hashSet.Contains(submission))
				{
					list2.Remove(submission.type);
				}
			}
			Mission.Type type2 = (item.type = ((list2.Count <= 0) ? ((Mission.Type)Random.Range(0, 7)) : list2[Random.Range(0, list2.Count)]));
			Dictionary<Mission.Type, int> dictionary2;
			Dictionary<Mission.Type, int> dictionary3 = (dictionary2 = dictionary);
			Mission.Type key;
			Mission.Type key2 = (key = type2);
			int num = dictionary2[key];
			dictionary3[key2] = num - 1;
			hashSet.Add(item);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		if (missionAreas == null)
		{
			return;
		}
		foreach (CampaignArea missionArea in missionAreas)
		{
			Gizmos.DrawWireSphere(missionArea.position, 0.5f);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__0(RateAppController P_0)
	{
		ShowMapScreen();
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1(CongratulationDialog P_0)
	{
		ShowMapScreen(true);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__2(CongratulationDialog P_0)
	{
		ResetProgress();
		Game.RestartLevel();
	}
}
