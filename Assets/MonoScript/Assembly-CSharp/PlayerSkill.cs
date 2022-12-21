using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
	public enum Type
	{
		damage = 0,
		armor = 1,
		mobility = 2,
		strike = 3,
		multiplier = 4
	}

	public class LevelComparer : Comparer<PlayerSkill>
	{
		public override int Compare(PlayerSkill skillA, PlayerSkill skillB)
		{
			return (skillA.level != skillB.level) ? ((skillA.level >= skillB.level) ? 1 : (-1)) : 0;
		}
	}

	public Type type;

	public float rate;

	public string title;

	public string description;

	private string LEVEL_KEY
	{
		get
		{
			return string.Format("{0}.skills.{1}.level", Campaign.id, type);
		}
	}

	public int level
	{
		get
		{
			return PlayerPrefs.GetInt(LEVEL_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(LEVEL_KEY, value);
		}
	}

	public string text
	{
		get
		{
			return string.Format("+{0}%", Mathf.RoundToInt(rate * 100f));
		}
	}

	public void Reset()
	{
		PlayerPrefs.DeleteKey(LEVEL_KEY);
	}

	public void LevelUp()
	{
		level++;
	}
}
