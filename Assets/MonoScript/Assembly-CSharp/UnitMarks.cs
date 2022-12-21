using UnityEngine;

public class UnitMarks : AObject
{
	private const int TICK = 10;

	public Mark markPrefab;

	public TargetType targetType;

	private Mark mark;

	private MarkController markController;

	private Player player;

	private Faction playerFaction;

	private void Start()
	{
		player = Player.Find();
		playerFaction = Player.faction;
		markController = MarkController.Find();
		mark = markPrefab.InstantiateFromResource(Vector3.zero, Quaternion.identity);
		mark.SetDistanceTo(player);
		markController.AddMark(mark);
	}

	private void FixedUpdate()
	{
		if (GetTick(10))
		{
			mark.target = GetNearestTarget(player.position);
		}
	}

	private Target GetNearestTarget(Vector3 point)
	{
		Target result = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < playerFaction.enemies.Count; i++)
		{
			Target target = playerFaction.enemies[i];
			if (target.type == targetType)
			{
				float sqrMagnitude = (target.position - point).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = target;
				}
			}
		}
		return result;
	}
}
