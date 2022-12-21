using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
	public enum Type
	{
		Assault = 0,
		AirFight = 1,
		Defense = 2,
		Capture = 3,
		Scout = 4,
		Caravan = 5,
		DogFight = 6,
		Tutorial = 7
	}

	public enum Setting
	{
		Forest = 0,
		Desert = 1,
		Winter = 2
	}

	public class SubmissionComparer : Comparer<Mission>
	{
		public override int Compare(Mission missionA, Mission missionB)
		{
			int count = missionA.submissions.Count;
			int count2 = missionA.submissions.Count;
			return (count != count2) ? ((count <= count2) ? 1 : (-1)) : 0;
		}
	}

	public const int TYPE_COUNT = 7;

	protected const string NONE_AREA = "";

	private const float SIZE = 0.5f;

	private static readonly int[] levels = new int[8] { 10, 13, 8, 8, 6, 8, 6, 1 };

	private static readonly string[] descriptions = new string[8] { "DESTROY THE ENEMY BASE", "DESTROY THE ENEMY AIR FORCES", "PROTECT THE BASE", "CAPTURE THE ENEMY FLAG", "SCOUT THE ENEMY BASE LOCATION", "DESTROY THE ENEMY CARAVAN", "DEFEAT THE ENEMY ACE", "COMPLETE TUTORIAL" };

	private static readonly Color[] colors = new Color[8]
	{
		Color.red,
		Color.cyan,
		Color.green,
		Color.blue,
		Color.yellow,
		Color.black,
		Color.grey,
		Color.magenta
	};

	public Type type;

	public Setting setting;

	public bool controlledArea;

	private CampaignArea areaCache;

	public CampaignArea area
	{
		get
		{
			if (areaCache == null)
			{
				areaCache = GetComponent<CampaignArea>();
			}
			return areaCache;
		}
	}

	private static string AREA_KEY
	{
		get
		{
			return string.Format("{0}.mission.area", Campaign.id);
		}
	}

	private static string TYPE_KEY
	{
		get
		{
			return string.Format("{0}.mission.type", Campaign.id);
		}
	}

	protected static string missionArea
	{
		get
		{
			return PlayerPrefs.GetString(AREA_KEY);
		}
		set
		{
			PlayerPrefs.SetString(AREA_KEY, value);
		}
	}

	protected static Type missionType
	{
		get
		{
			return (Type)PlayerPrefs.GetInt(TYPE_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(TYPE_KEY, (int)value);
		}
	}

	public string description
	{
		get
		{
			return descriptions[(int)type];
		}
	}

	public int level
	{
		get
		{
			return PlayerPrefs.GetInt(string.Format("{0}.mission.{1}.level", Campaign.id, type), 0);
		}
	}

	public List<Mission> submissions
	{
		get
		{
			List<Mission> list = new List<Mission>();
			List<CampaignArea> connections = area.connections;
			for (int i = 0; i < connections.Count; i++)
			{
				Mission mission = connections[i].mission;
				if (mission != null)
				{
					list.Add(mission);
				}
			}
			return list;
		}
	}

	public virtual void Load()
	{
		missionArea = ((!controlledArea) ? string.Empty : area.name);
		missionType = type;
		Scenario.difficulty = level;
		Scenario.setting = (int)setting;
		Game.LoadLevel(type.ToString());
	}

	public static void Complete(int score = 0)
	{
		Campaign.AddCapturedArea(missionArea);
		Campaign.IncreaseScore(score);
		IncreaseMissionLevel(missionType);
	}

	public static string GetDescription()
	{
		return descriptions[(int)missionType];
	}

	public static int GetLevelCount(Type type)
	{
		return ((int)type < levels.Length) ? levels[(int)type] : 0;
	}

	private static void IncreaseMissionLevel(Type type)
	{
		int @int = PlayerPrefs.GetInt(string.Format("{0}.mission.{1}.level", Campaign.id, type), 0);
		PlayerPrefs.SetInt(string.Format("{0}.mission.{1}.level", Campaign.id, type), @int + 1);
	}

	public void Reset()
	{
		PlayerPrefs.DeleteKey(string.Format("{0}.mission.{1}.level", Campaign.id, type));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = colors[(int)type];
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}
}
