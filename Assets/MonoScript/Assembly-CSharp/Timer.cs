using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Timer : MonoBehaviour
{
	private bool paused;

	private float time;

	private float timer;

	private float lastTime;

	public float current
	{
		get
		{
			return timer;
		}
	}

	public float rate
	{
		get
		{
			return (time == 0f) ? 1f : (timer / time);
		}
	}

	[method: MethodImpl(32)]
	public event Action<Timer> onTimeUp;

	public static Timer Create()
	{
		return Create(0f);
	}

	public static Timer Create(float time)
	{
		GameObject gameObject = GameObject.Find("Timer");
		if (gameObject == null)
		{
			gameObject = new GameObject("Timer");
		}
		Timer timer = gameObject.AddComponent<Timer>();
		timer.SetTime(time);
		return timer;
	}

	private void Start()
	{
		lastTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		if (!paused && timer < time)
		{
			timer += Time.realtimeSinceStartup - lastTime;
			if (timer >= time)
			{
				OnTimeUp();
			}
		}
		lastTime = Time.realtimeSinceStartup;
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			lastTime = Time.realtimeSinceStartup;
		}
	}

	public void SetTime(float time)
	{
		timer = 0f;
		this.time = time;
	}

	public void Pause(bool flag)
	{
		paused = flag;
	}

	public void Remove()
	{
		UnityEngine.Object.Destroy(this);
	}

	private void OnTimeUp()
	{
		if (this.onTimeUp != null)
		{
			this.onTimeUp(this);
		}
	}
}
