using UnityEngine;

public class BombStage : TutorialStage
{
	public TutorialTarget tutorialTargetPrefab;

	public Transform spawnPoint;

	public override void Init(Tutorial tutorial)
	{
		Faction faction = Player.faction;
		Vector3 position = spawnPoint.position;
		Surface surface = Surface.Find();
		if (surface != null)
		{
			position.y = surface.GetHeight(position);
		}
		TutorialTarget tutorialTarget = (TutorialTarget)tutorialTargetPrefab.Instantiate(position, spawnPoint.rotation);
		tutorialTarget.SetFaction(faction.enemy);
		tutorialTarget.onDestroyed += OnTargetDestroyed;
		tutorial.SetTitle(titleIndex);
		tutorial.SetMark(tutorialTarget);
		HUD hUD = UIController.Find<HUD>();
		hUD.bombButton.gameObject.SetActiveRecursively(true);
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
