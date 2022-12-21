using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Faction : MonoBehaviour
{
	private const string FACTION_A_TAG = "FactionA";

	public Faction enemy;

	public Airplane[] aiplanePrefabs;

	public Unit[] unitPrefabs;

	public Material flag;

	public Color color;

	public List<Target> units;

	private static int targetIndex;

	public List<Target> enemies
	{
		get
		{
			return enemy.units;
		}
	}

	[method: MethodImpl(32)]
	public event Action<Faction, Target> onUnitAdded;

	[method: MethodImpl(32)]
	public event Action<Faction, Target> onUnitRemoved;

	public static Faction Find(string tag)
	{
		Faction[] array = (Faction[])UnityEngine.Object.FindObjectsOfType(typeof(Faction));
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].tag == tag)
			{
				return array[i];
			}
		}
		return null;
	}

	public static int GetIndexByTag(string tag)
	{
		return (!tag.Equals("FactionA")) ? 1 : 0;
	}

	public Unit SpawnUnit(UnitType unitType, Vector3 position, Quaternion rotation)
	{
		return SpawnUnit((int)unitType, position, rotation);
	}

	public Unit SpawnUnit(int unitIndex, Vector3 position, Quaternion rotation)
	{
		Unit unit = unitPrefabs[unitIndex].Instantiate(position, rotation);
		AddUnit(unit);
		return unit;
	}

	public Airplane SpawnAirplane(int airplaneIndex, Vector3 position, Quaternion rotation)
	{
		Airplane airplane = aiplanePrefabs[airplaneIndex].Instantiate(position, rotation) as Airplane;
		AddUnit(airplane);
		return airplane;
	}

	public Airplane[] GetAirplanes()
	{
		return aiplanePrefabs;
	}

	public void AddUnit(Target unit)
	{
		RegsiterUnit(unit);
		units.Add(unit);
		unit.SetTargetId(targetIndex++);
		if (this.onUnitAdded != null)
		{
			this.onUnitAdded(this, unit);
		}
	}

	public void RemoveUnit(Target unit)
	{
		UnregisterUnit(unit);
		units.Remove(unit);
		if (this.onUnitRemoved != null)
		{
			this.onUnitRemoved(this, unit);
		}
	}

	private void RegsiterUnit(Target unit)
	{
		unit.onDestroyed += OnUnitDestroyed;
	}

	private void UnregisterUnit(Target unit)
	{
		unit.onDestroyed -= OnUnitDestroyed;
	}

	private void OnUnitDestroyed(Target unit)
	{
		RemoveUnit(unit);
	}
}
