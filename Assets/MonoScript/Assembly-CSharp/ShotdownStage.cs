using UnityEngine;

public class ShotdownStage : TutorialStage
{
	public TutorialTarget tutorialTargetPrefab;

	public Transform spawnPoint;

	public override void Init(Tutorial tutorial)
	{
		Faction faction = Player.faction;
		TutorialTarget tutorialTarget = (TutorialTarget)tutorialTargetPrefab.Instantiate(spawnPoint.position, spawnPoint.rotation);
		tutorialTarget.SetFaction(faction.enemy);
		tutorialTarget.onDestroyed += OnTargetDestroyed;
		tutorial.SetTitle(titleIndex);
		tutorial.SetMark(tutorialTarget);
	}

	private void OnTargetDestroyed(Target target)
	{
		target.onDestroyed -= OnTargetDestroyed;
		Complete();
	}

	private void OnDrawGizmosSelected()
	{
		if (spawnPoint != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(spawnPoint.position, 0.1f);
		}
	}
}
