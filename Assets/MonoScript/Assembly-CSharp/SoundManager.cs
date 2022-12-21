using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public enum State
	{
		None = 0,
		FadeIn = 1,
		FadeOut = 2
	}

	public enum ThemeType
	{
		Main = 0,
		Victory = 1,
		Defeat = 2
	}

	[Serializable]
	public class Theme
	{
		public AudioClip clip;

		public bool loop;

		public float volume;

		public float fadeInTime;

		public float fadeOutTime;
	}

	private const float MIN_VOLUME = 0f;

	public Theme[] themes;

	public AudioSource audioSource;

	public float fadeInTime = 1f;

	public float fadeOutTime = 3f;

	public float volume = 1f;

	private State state;

	private float lastTime;

	private float timer;

	private static SoundManager soundManagerCache;

	public bool mainTheme { get; private set; }

	public bool isPlaying
	{
		get
		{
			return audioSource.isPlaying;
		}
	}

	public static SoundManager Find()
	{
		if (soundManagerCache == null)
		{
			soundManagerCache = (SoundManager)UnityEngine.Object.FindObjectOfType(typeof(SoundManager));
		}
		return soundManagerCache;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		float num = Time.realtimeSinceStartup - lastTime;
		switch (state)
		{
		case State.FadeIn:
			timer -= num;
			audioSource.volume = Utils.Lerp(0f, volume, 1f - timer / fadeInTime);
			if (timer <= 0f)
			{
				state = State.None;
			}
			break;
		case State.FadeOut:
			timer -= num;
			audioSource.volume = Utils.Lerp(0f, volume, timer / fadeOutTime);
			if (timer <= 0f)
			{
				state = State.None;
				audioSource.Stop();
			}
			break;
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

	public void FadeIn(ThemeType themeType = ThemeType.Main)
	{
		audioSource.clip = themes[(int)themeType].clip;
		audioSource.loop = themes[(int)themeType].loop;
		volume = themes[(int)themeType].volume;
		fadeInTime = themes[(int)themeType].fadeInTime;
		fadeOutTime = themes[(int)themeType].fadeOutTime;
		audioSource.volume = 0f;
		audioSource.Play();
		state = State.FadeIn;
		timer = fadeInTime;
		mainTheme = themeType == ThemeType.Main;
	}

	public void FadeOut()
	{
		state = State.FadeOut;
		timer = fadeOutTime;
		mainTheme = false;
	}
}
