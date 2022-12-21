using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScenario : Scenario
{
	public ScenarioScheme[] schemes;

	public bool finishWithSession;

	private Faction playerFraction;

	private Dictionary<Faction, Vector3> targetPointMap = new Dictionary<Faction, Vector3>();

	private ScenarioScheme activeScheme;

	private List<Objective> sideAObjectives = new List<Objective>();

	public int totalObjectiveCount { get; private set; }

	public int objectiveCount
	{
		get
		{
			return sideAObjectives.Count;
		}
	}

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
		totalObjectiveCount = activeScheme.sideA.objectives.Length;
		for (int i = 0; i < activeScheme.sideA.objectives.Length; i++)
		{
			Objective objective = activeScheme.sideA.objectives[i];
			objective.onCompleted += OnSideAObjectiveCompleted;
			sideAObjectives.Add(objective);
			objective.Init(this);
		}
		StartScenario();
	}

	private void OnSideAObjectiveCompleted(Objective objective)
	{
		objective.onCompleted -= OnSideAObjectiveCompleted;
		sideAObjectives.Remove(objective);
		if (!finishWithSession && sideAObjectives.Count == 0)
		{
			FinishScenario();
		}
	}

	public override bool SessionComplete()
	{
		if (finishWithSession && sideAObjectives.Count == 0)
		{
			FinishScenario();
			return true;
		}
		return false;
	}

	public override Vector3 GetTargetPoint(Faction faction)
	{
		return targetPointMap[faction];
	}
}
