using UnityEngine;

public class AppStoreController : MonoBehaviour
{
	private const string APP_URL_KEY = "app.url";

	private const string APP_STORE_KEY = "app.store";

	private const string MORE_GAMES_KEY = "app.url.more.games";

	public string[] appUrls;

	public string[] moreGamesUrls;

	public AppStore appStore;

	private static string appURL
	{
		get
		{
			return PlayerPrefs.GetString("app.url");
		}
		set
		{
			PlayerPrefs.SetString("app.url", value);
		}
	}

	private static string moreGamesURL
	{
		get
		{
			return PlayerPrefs.GetString("app.url.more.games");
		}
		set
		{
			PlayerPrefs.SetString("app.url.more.games", value);
		}
	}

	private void Awake()
	{
		appURL = appUrls[(int)appStore];
		moreGamesURL = moreGamesUrls[(int)appStore];
	}

	public static void OpenMoreGames()
	{
		Application.OpenURL(moreGamesURL);
	}

	public static void OpenApp()
	{
		Application.OpenURL(appURL);
	}
}
