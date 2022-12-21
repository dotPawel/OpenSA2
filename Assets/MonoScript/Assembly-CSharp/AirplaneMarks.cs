using System.Collections.Generic;
using UnityEngine;

public class AirplaneMarks : MonoBehaviour
{
	public Mark markPrefab;

	public bool friendly;

	private Dictionary<Target, Mark> marks = new Dictionary<Target, Mark>();

	private MarkController markController;

	private Player player;

	private void Awake()
	{
		player = Player.Find();
		markController = MarkController.Find();
		if (friendly)
		{
			Player.faction.onUnitAdded += OnUnitAdded;
		}
		else
		{
			Player.faction.enemy.onUnitAdded += OnUnitAdded;
		}
	}

	private void OnUnitAdded(Faction faction, Target unit)
	{
		if (unit.type == TargetType.Air)
		{
			Mark mark = markPrefab.InstantiateFromResource(Vector3.zero, Quaternion.identity);
			mark.target = unit;
			mark.SetDistanceTo(player);
			marks.Add(unit, mark);
			unit.onDestroyed += OnUnitDestroyed;
			markController.AddMark(mark);
		}
	}

	private void OnUnitDestroyed(Target unit)
	{
		unit.onDestroyed -= OnUnitDestroyed;
		Mark mark = marks[unit];
		markController.RemoveMark(mark);
		marks.Remove(unit);
		mark.Remove();
	}
}
