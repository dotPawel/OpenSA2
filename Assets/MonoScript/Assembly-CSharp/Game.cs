using UnityEngine;

public class Game
{
	private const string FREE_KEY = "game.free";

	private const string TIME_KEY = "game.time";

	private const string DEBUG_KEY = "game.bebug";

	private const string SOUND_KEY = "game.sound";

	public static bool debugMode
	{
		get
		{
			return PlayerPrefs.HasKey("game.bebug") && PlayerPrefs.GetInt("game.bebug") > 0;
		}
		set
		{
			PlayerPrefs.SetInt("game.bebug", value ? 1 : 0);
		}
	}

	public static bool sound
	{
		get
		{
			return PlayerPrefs.GetInt("game.sound", 1) > 0;
		}
		set
		{
			AudioListener.volume = ((!value) ? 0f : 1f);
			PlayerPrefs.SetInt("game.sound", value ? 1 : 0);
		}
	}

	public static float time
	{
		get
		{
			return (!PlayerPrefs.HasKey("game.time")) ? 0f : PlayerPrefs.GetFloat("game.time");
		}
		set
		{
			PlayerPrefs.SetFloat("game.time", value);
		}
	}

	public static void SetPause(bool value)
	{
		Time.timeScale = ((!value) ? 1f : 0f);
	}

	public static void LoadMainMenu()
	{
		LoadingScreen.ShowScreen();
		Application.LoadLevelAsync("MainMenu");
	}

	public static void LoadCampaign()
	{
		LoadingScreen.ShowScreen();
		Application.LoadLevelAsync("Campaign");
	}

	public static void LoadLevel(int levelIndex)
	{
		LoadingScreen.ShowScreen();
		Application.LoadLevelAsync(levelIndex);
	}

	public static void LoadLevel(string levelName)
	{
		LoadingScreen.ShowScreen();
		Application.LoadLevelAsync(levelName);
	}

	public static void RestartLevel()
	{
		LoadingScreen.ShowScreen();
		Application.LoadLevelAsync(Application.loadedLevel);
	}
}
