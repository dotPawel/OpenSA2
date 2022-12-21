using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ForcesSpawner : DeltaSpawner, ScenarioListener
{
	public UnitType unitType;

	public Structure structure;

	public Transform[] spawnPoints;

	private int spawnPointIndex;

	[method: MethodImpl(32)]
	public event Action<Unit> onUnitSpawned;

	private void Start()
	{
		if (structure != null)
		{
			structure.onDestroyed += OnStructureDestroyed;
		}
	}

	private void OnStructureDestroyed(Target structure)
	{
		structure.onDestroyed -= OnStructureDestroyed;
		SetActive(false);
	}

	private Structure.SpawnLocation GetSpawnLocation()
	{
		if (structure != null)
		{
			return structure.spawnLocation;
		}
		Transform transform = spawnPoints[spawnPointIndex++ % spawnPoints.Length];
		return new Structure.SpawnLocation(transform.position, transform.rotation);
	}

	protected override void SpawnUnit(float offset)
	{
		base.SpawnUnit();
		Structure.SpawnLocation spawnLocation = GetSpawnLocation();
		Vector3 vector = spawnLocation.rotation * Vector3.forward;
		Unit unit = faction.SpawnUnit(unitType, spawnLocation.position + vector * offset, spawnLocation.rotation);
		unit.onDestroyed += OnUnitDestroyed;
		if (this.onUnitSpawned != null)
		{
			this.onUnitSpawned(unit);
		}
	}

	public void OnScenarioStart(Scenario scenario)
	{
		SetActive(true);
		Prespawn(prespawn);
	}

	public void OnScenarioFinish(Scenario scenario)
	{
		SetActive(false);
	}

	private void OnUnitDestroyed(Target unit)
	{
		unit.onDestroyed -= OnUnitDestroyed;
		ReleaseUnit();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			Gizmos.DrawSphere(spawnPoints[i].position, 0.5f);
		}
	}
}
