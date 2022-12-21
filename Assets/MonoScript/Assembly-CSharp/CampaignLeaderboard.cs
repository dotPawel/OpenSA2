using UnityEngine;

public class CampaignLeaderboard : MonoBehaviour
{
	public string[] leaderboards;

	private void Start()
	{
		SocialNetwork socialNetwork = SocialNetwork.Find();
		if (socialNetwork != null && socialNetwork.isUserAuthenticated)
		{
			string leaderboard = leaderboards[Campaign.id];
			int score = Campaign.score;
			socialNetwork.ReportScore(leaderboard, score);
		}
	}
}
