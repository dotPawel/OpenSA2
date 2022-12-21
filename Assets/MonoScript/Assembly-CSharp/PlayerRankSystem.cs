using UnityEngine;

public class PlayerRankSystem : MonoBehaviour
{
	private static int[] rankPoints = new int[8] { 1000, 3000, 8000, 14000, 22000, 32000, 44000, 58000 };

	public static int maxRank
	{
		get
		{
			return rankPoints.Length;
		}
	}

	public static int GetPlayerRank(int score)
	{
		int num = 0;
		for (int i = 0; i < rankPoints.Length && score >= rankPoints[i]; i++)
		{
			num++;
		}
		return num;
	}

	public static float GetPlayerRankProgress(int rank, int score)
	{
		if (rank >= maxRank)
		{
			return 1f;
		}
		int num = rankPoints[Mathf.Clamp(rank, 0, rankPoints.Length - 1)];
		int num2 = ((rank > 0) ? rankPoints[Mathf.Clamp(rank - 1, 0, rankPoints.Length - 1)] : 0);
		return Mathf.Clamp01((float)(score - num2) / (float)(num - num2));
	}
}
