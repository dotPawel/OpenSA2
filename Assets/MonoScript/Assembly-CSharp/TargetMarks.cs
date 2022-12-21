using System.Collections.Generic;
using UnityEngine;

public class TargetMarks : AObject
{
	private const int TICK = 10;

	public Mark markPrefab;

	public List<Target> targets;

	private Mark mark;

	private MarkController markController;

	private Player player;

	private void Start()
	{
		if (targets.Count > 0)
		{
			player = Player.Find();
			markController = MarkController.Find();
			mark = markPrefab.InstantiateFromResource(Vector3.zero, Quaternion.identity);
			mark.SetDistanceTo(player);
			markController.AddMark(mark);
			UpdateMark();
			for (int i = 0; i < targets.Count; i++)
			{
				targets[i].onDestroyed += OnTargetDestroyed;
			}
		}
	}

	private void OnTargetDestroyed(Target target)
	{
		target.onDestroyed -= OnTargetDestroyed;
		targets.Remove(target);
		if (mark != null && mark.target == target)
		{
			UpdateMark();
		}
	}

	private void FixedUpdate()
	{
		if (mark != null && GetTick(10))
		{
			UpdateMark();
		}
	}

	private void UpdateMark()
	{
		Target nearestTarget = GetNearestTarget(player.position);
		if (nearestTarget != null)
		{
			mark.target = nearestTarget;
			return;
		}
		markController.RemoveMark(mark);
		mark.Remove();
		mark = null;
	}

	private Target GetNearestTarget(Vector3 point)
	{
		Target result = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < targets.Count; i++)
		{
			Target target = targets[i];
			float sqrMagnitude = (target.position - point).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				result = target;
			}
		}
		return result;
	}
}
