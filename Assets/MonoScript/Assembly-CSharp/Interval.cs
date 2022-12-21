using System;
using UnityEngine;

[Serializable]
public class Interval
{
	public float min;

	public float max;

	private float timer;

	public float random
	{
		get
		{
			return UnityEngine.Random.Range(min, max);
		}
	}

	public Interval(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	public Interval()
		: this(0f, 0f)
	{
	}

	public float Lerp(float t)
	{
		return Utils.Lerp(min, max, t);
	}

	public float Rate(float value)
	{
		return (max - Mathf.Max(Mathf.Min(value, max), min)) / (max - min);
	}

	public float Lerp(float delta, float delay, int direction)
	{
		timer += ((direction <= 0) ? (0f - delta) : delta);
		if (timer >= delay)
		{
			timer = delay;
			return max;
		}
		if (timer <= 0f)
		{
			timer = 0f;
			return min;
		}
		return Utils.Lerp(min, max, timer / delay);
	}
}
