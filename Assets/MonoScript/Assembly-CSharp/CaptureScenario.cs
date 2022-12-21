using System.Collections.Generic;
using UnityEngine;

public class CaptureScenario : Scenario
{
	public ScenarioScheme[] schemes;

	private Faction playerFraction;

	private Dictionary<Faction, Vector3> targetPointMap = new Dictionary<Faction, Vector3>();

	private ScenarioScheme activeScheme;

	private List<Spawner> sideASpawners = new List<Spawner>();

	private List<FlagPlatform> sideBFlags = new List<FlagPlatform>();

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
		for (int i = 0; i < activeScheme.sideA.spawners.Length; i++)
		{
			Spawner spawner = activeScheme.sideA.spawners[i];
			spawner.onFinished += OnSideAScenarioFinished;
			sideASpawners.Add(spawner);
		}
		for (int j = 0; j < activeScheme.sideB.flags.Length; j++)
		{
			FlagPlatform flagPlatform = activeScheme.sideB.flags[j];
			flagPlatform.onCaptured += OnSideBFlagCaptured;
			sideBFlags.Add(flagPlatform);
		}
		StartScenario();
	}

	private void OnSideBFlagCaptured(FlagPlatform flag)
	{
		flag.onCaptured -= OnSideBFlagCaptured;
		sideBFlags.Remove(flag);
		if (sideBFlags.Count == 0)
		{
			FinishScenario();
		}
	}

	private void OnSideAScenarioFinished(Spawner spawner)
	{
		spawner.onFinished -= OnSideAScenarioFinished;
		sideASpawners.Remove(spawner);
		if (sideASpawners.Count == 0)
		{
			FinishScenario(false);
		}
	}

	public override Vector3 GetTargetPoint(Faction faction)
	{
		return targetPointMap[faction];
	}
}
