using UnityEngine;

public class SocialNetwork : MonoBehaviour
{
	public bool authenticateAtStart;

	public bool isUserAuthenticated
	{
		get
		{
			return false;
		}
	}

	public static SocialNetwork Find()
	{
		return (SocialNetwork)Object.FindObjectOfType(typeof(SocialNetwork));
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		if (authenticateAtStart)
		{
			Authenticate();
		}
	}

	public void Authenticate()
	{
	}

	private void ProcessAuthentication(bool success)
	{
		if (!success)
		{
			Debug.Log("Failed to authenticate");
		}
	}

	public void ShowLeaderboardUI()
	{
	}

	public void ReportScore(string leaderboard, long score)
	{
	}

	private void ReportProgress(bool success)
	{
		if (success)
		{
			Debug.Log("Successfully reported progress");
		}
		else
		{
			Debug.Log("Failed to report progress");
		}
	}
}
