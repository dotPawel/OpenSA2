using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
	public enum FragType
	{
		Infantry = 0,
		Tank = 1,
		Airplane = 2,
		Structure = 3,
		Other = 4
	}

	public int score;

	public float multiplier = 1f;

	private Dictionary<FragType, int> frags = new Dictionary<FragType, int>();

	private Dictionary<FragType, int> points = new Dictionary<FragType, int>();

	private static ScoreController scoreControllerReference;

	[method: MethodImpl(32)]
	public event Action<ScoreController, int> onScoreUpdated;

	[method: MethodImpl(32)]
	public event Action<ScoreController, FragType> onFragAdded;

	public static ScoreController Find()
	{
		if (scoreControllerReference == null)
		{
			scoreControllerReference = (ScoreController)UnityEngine.Object.FindObjectOfType(typeof(ScoreController));
		}
		return scoreControllerReference;
	}

	public void AddPoints(FragType type, int value)
	{
		value = (int)((float)value * multiplier);
		score += value;
		if (points.ContainsKey(type))
		{
			Dictionary<FragType, int> dictionary;
			Dictionary<FragType, int> dictionary2 = (dictionary = points);
			FragType key;
			FragType key2 = (key = type);
			int num = dictionary[key];
			dictionary2[key2] = num + value;
		}
		else
		{
			points.Add(type, value);
		}
		if (this.onScoreUpdated != null)
		{
			this.onScoreUpdated(this, score);
		}
	}

	public void AddFrag(FragType type, int points = 0)
	{
		if (frags.ContainsKey(type))
		{
			Dictionary<FragType, int> dictionary;
			Dictionary<FragType, int> dictionary2 = (dictionary = frags);
			FragType key;
			FragType key2 = (key = type);
			int num = dictionary[key];
			dictionary2[key2] = num + 1;
		}
		else
		{
			frags.Add(type, 1);
		}
		if (this.onFragAdded != null)
		{
			this.onFragAdded(this, type);
		}
		if (points > 0)
		{
			AddPoints(type, points);
		}
	}

	public void SetMultiplier(float value)
	{
		multiplier = 1f + value;
	}

	public int GetFragCount(FragType type)
	{
		return frags.ContainsKey(type) ? frags[type] : 0;
	}

	public int GetPointsCount(FragType type)
	{
		return points.ContainsKey(type) ? points[type] : 0;
	}
}
