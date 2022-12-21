using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AirplaneSpawner : DeltaSpawner, ScenarioListener
{
	public AirplaneType[] types;

	public Structure airport;

	public Transform[] spawnPoints;

	public int fighterDamage;

	public int fighterArmor;

	public int fighterMobility;

	private int spawnPointIndex;

	private List<Fighter> fighters = new List<Fighter>();

	[method: MethodImpl(32)]
	public event Action<AirplaneSpawner, Airplane> onAirplaneSpawned;

	private void Start()
	{
		if (airport != null)
		{
			airport.onDestroyed += OnAirportDestroyed;
		}
	}

	private void OnAirportDestroyed(Target airport)
	{
		airport.onDestroyed -= OnAirportDestroyed;
		SetActive(false);
	}

	private Structure.SpawnLocation GetSpawnLocation()
	{
		if (airport != null)
		{
			return airport.spawnLocation;
		}
		Transform transform = spawnPoints[spawnPointIndex++ % spawnPoints.Length];
		return new Structure.SpawnLocation(transform.position, transform.rotation);
	}

	protected override void SpawnUnit(float offset = 0f)
	{
		base.SpawnUnit();
		AirplaneType airplaneIndex = types[UnityEngine.Random.Range(0, types.Length)];
		Structure.SpawnLocation spawnLocation = GetSpawnLocation();
		Airplane airplane = faction.SpawnAirplane((int)airplaneIndex, spawnLocation.position, spawnLocation.rotation);
		airplane.onDestroyed += OnUnitDestroyed;
		Fighter freeFighter = GetFreeFighter();
		freeFighter.Init(airplane);
		if (this.onAirplaneSpawned != null)
		{
			this.onAirplaneSpawned(this, airplane);
		}
	}

	private void OnUnitDestroyed(Target airplane)
	{
		airplane.onDestroyed -= OnUnitDestroyed;
		ReleaseUnit();
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

	private Fighter GetFreeFighter()
	{
		for (int i = 0; i < fighters.Count; i++)
		{
			if (fighters[i].airplane == null)
			{
				return fighters[i];
			}
		}
		Fighter fighter = base.gameObject.AddComponent<Fighter>();
		fighter.damage = fighterDamage;
		fighter.armor = fighterArmor;
		fighter.mobility = fighterMobility;
		fighters.Add(fighter);
		return fighter;
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
