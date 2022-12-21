using UnityEngine;

public class ScenarioSide : MonoBehaviour
{
	public Faction faction;

	public Spawner[] spawners;

	public FlagPlatform[] flags;

	public Structure[] structures;

	public Turret[] turrets;

	public Obstacle[] obstacles;

	public Objective[] objectives;

	public Vector3 targetPoint;

	private int structuresDestroyed;

	private int spawnersFinished;

	public void SetFaction(Faction value)
	{
		faction = value;
		for (int i = 0; i < flags.Length; i++)
		{
			flags[i].SetFaction(value);
		}
		for (int j = 0; j < spawners.Length; j++)
		{
			spawners[j].SetFaction(value);
		}
		for (int k = 0; k < structures.Length; k++)
		{
			structures[k].SetFaction(value);
		}
		for (int l = 0; l < turrets.Length; l++)
		{
			turrets[l].SetFaction(value);
		}
		for (int m = 0; m < obstacles.Length; m++)
		{
			obstacles[m].SetFaction(value);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(targetPoint, 1f);
	}
}
