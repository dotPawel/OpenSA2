using System.Collections.Generic;
using UnityEngine;

public class AirForceSpawner : Spawner, ScenarioListener
{
	public AirplaneType[] types;

	public float cooldown;

	public int maxLimit;

	public int unitCount;

	public Transform[] spawnPoints;

	public bool deactive;

	public int fighterDamage;

	public int fighterArmor;

	public int fighterMobility;

	private float timer;

	private int limit;

	private List<Fighter> fighters = new List<Fighter>();

	private int level;

	private int spawnPointIndex;

	private void FixedUpdate()
	{
		if (deactive || !(faction != null))
		{
			return;
		}
		if (timer < cooldown)
		{
			timer += Time.deltaTime;
			return;
		}
		timer = 0f;
		if (limit < maxLimit && unitCount > 0)
		{
			AirplaneType airplaneIndex = types[Random.Range(0, types.Length)];
			Transform transform = spawnPoints[spawnPointIndex++ % spawnPoints.Length];
			Airplane airplane = faction.SpawnAirplane((int)airplaneIndex, transform.position, transform.rotation);
			airplane.onDestroyed += OnUnitDestroyed;
			limit++;
			Fighter freeFighter = GetFreeFighter();
			freeFighter.Init(airplane);
			unitCount--;
		}
	}

	private void OnUnitDestroyed(Target airplane)
	{
		airplane.onDestroyed -= OnUnitDestroyed;
		limit--;
		if (limit == 0 && unitCount == 0)
		{
			Finish();
		}
	}

	public void OnScenarioStart(Scenario scenario)
	{
		deactive = false;
	}

	public void OnScenarioFinish(Scenario scenario)
	{
		deactive = true;
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

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			if (spawnPoints[i] != null)
			{
				Gizmos.DrawWireSphere(spawnPoints[i].position, 0.1f);
			}
		}
	}
}
