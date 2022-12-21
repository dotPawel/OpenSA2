using UnityEngine;

public class SnapshotObjective : Objective
{
	public float range = 1f;

	public Effect flashEffect;

	public int points = 750;

	private Player player;

	private ScoreController scoreController;

	private ObjectiveScenario scenario;

	private Vector2 gameMessageLocation = new Vector2(0f, 220f);

	private void Start()
	{
		player = Player.Find();
	}

	private void FixedUpdate()
	{
		if (player.airplane != null && Utils.IsDistanceLow(base.position, player.airplane.position, range))
		{
			Complete();
		}
	}

	public override void Init(Scenario scenario)
	{
		this.scenario = (ObjectiveScenario)scenario;
	}

	public override void Complete()
	{
		base.Complete();
		player.scoreController.AddPoints(ScoreController.FragType.Structure, points);
		if (flashEffect != null)
		{
			flashEffect.Instantiate(base.position, base.rotation);
		}
		int num = scenario.totalObjectiveCount - scenario.objectiveCount;
		GameMessenger.Message(string.Format("{0}/{1} BASES SCOUTED", num, scenario.totalObjectiveCount), gameMessageLocation);
		base.show = false;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.position, range);
	}
}
