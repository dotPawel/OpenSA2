using UnityEngine;

public class PointsController : MonoBehaviour
{
	public Target target;

	public ScoreController.FragType fragType;

	public int hitPoints;

	public int fragPoints;

	private Player player;

	private ScoreController scoreController;

	private void Start()
	{
		scoreController = ScoreController.Find();
		if (scoreController != null)
		{
			target.onHit += OnTargetHit;
			player = Player.Find();
		}
	}

	private void OnTargetHit(Target target, int id)
	{
		if (player.id == id)
		{
			if (target.healthRate > 0f)
			{
				scoreController.AddPoints(fragType, hitPoints);
			}
			else
			{
				scoreController.AddFrag(fragType, fragPoints);
			}
		}
	}
}
