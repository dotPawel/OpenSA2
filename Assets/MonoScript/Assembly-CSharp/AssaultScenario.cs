using System.Collections.Generic;
using UnityEngine;

public class AssaultScenario : Scenario
{
	public ScenarioScheme[] schemes;

	private Faction playerFraction;

	private Dictionary<Faction, Vector3> targetPointMap = new Dictionary<Faction, Vector3>();

	private ScenarioScheme activeScheme;

	private List<Target> sideAStructures = new List<Target>();

	private List<Target> sideBStructures = new List<Target>();

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
		playerSpawner.SetAirport(activeScheme.playerAirport);
		activeScheme.sideA.SetFaction(playerFraction);
		activeScheme.sideB.SetFaction(playerFraction.enemy);
		for (int i = 0; i < activeScheme.sideA.structures.Length; i++)
		{
			Structure structure = activeScheme.sideA.structures[i];
			structure.onDestroyed += OnSideAStructureDestroyed;
			sideAStructures.Add(structure);
		}
		for (int j = 0; j < activeScheme.sideB.structures.Length; j++)
		{
			Structure structure2 = activeScheme.sideB.structures[j];
			structure2.onDestroyed += OnSideBStructureDestroyed;
			sideBStructures.Add(structure2);
		}
		StartScenario();
	}

	private void OnSideBStructureDestroyed(Target structure)
	{
		structure.onDestroyed -= OnSideBStructureDestroyed;
		sideBStructures.Remove(structure);
		if (sideBStructures.Count == 0)
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

	public override Vector3 GetTargetPoint(Faction faction)
	{
		return targetPointMap[faction];
	}
}
