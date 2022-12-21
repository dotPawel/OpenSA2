using System.Collections.Generic;
using UnityEngine;

public class DefenseScenario : Scenario
{
	public ScenarioScheme[] schemes;

	private ScenarioScheme activeScheme;

	private Faction playerFraction;

	private Dictionary<Faction, Vector3> targetPointMap = new Dictionary<Faction, Vector3>();

	private List<Target> sideAStructures = new List<Target>();

	private List<Spawner> sideBSpawners = new List<Spawner>();

	private void Awake()
	{
		activeScheme = schemes[Mathf.Clamp(Scenario.difficulty, 0, schemes.Length - 1)];
		activeScheme.SetEnable(true);
	}

	private void Start()
	{
		playerFraction = Player.faction;
		targetPointMap.Add(playerFraction, activeScheme.sideA.targetPoint);
		targetPointMap.Add(playerFraction.enemy, activeScheme.sideB.targetPoint);
		activeScheme.sideA.SetFaction(playerFraction);
		activeScheme.sideB.SetFaction(playerFraction.enemy);
		playerSpawner.SetAirport(activeScheme.playerAirport);
		for (int i = 0; i < activeScheme.sideA.structures.Length; i++)
		{
			Structure structure = activeScheme.sideA.structures[i];
			structure.onDestroyed += OnSideAStructureDestroyed;
			sideAStructures.Add(structure);
		}
		for (int j = 0; j < activeScheme.sideB.spawners.Length; j++)
		{
			Spawner spawner = activeScheme.sideB.spawners[j];
			spawner.onFinished += OnSideBSpawnerFinished;
			sideBSpawners.Add(spawner);
		}
		StartScenario();
	}

	private void OnSideBSpawnerFinished(Spawner spawner)
	{
		spawner.onFinished -= OnSideBSpawnerFinished;
		sideBSpawners.Remove(spawner);
		if (sideBSpawners.Count == 0)
		{
			FinishScenario();
		}
	}

	private void OnSideAStructureDestroyed(Target structure)
	{
		structure.onDestroyed -= OnSideAStructureDestroyed;
		sideAStructures.Remove(structure);
		if (sideAStructures.Count == 0)
		{
			FinishScenario(false);
		}
	}

	private void OnSpawnersFinished(ScenarioSide side)
	{
		FinishScenario();
	}

	private void OnStructuresDestroyed(ScenarioSide side)
	{
		FinishScenario(playerFraction != side.faction);
	}

	public override Vector3 GetTargetPoint(Faction faction)
	{
		return targetPointMap[faction];
	}
}
