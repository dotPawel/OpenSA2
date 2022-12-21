using UnityEngine;

public class FollowCamera : AObject
{
	public float maxHeight;

	public float minHeight;

	public Faction faction;

	public float cooldown;

	public TargetType targetType;

	public Target target;

	private float timer;

	private void LateUpdate()
	{
		if (target != null)
		{
			Vector3 vector = target.position;
			vector.y = Mathf.Clamp(vector.y, minHeight, maxHeight);
			base.position = vector;
		}
	}

	private void FixedUpdate()
	{
		if (!(target == null))
		{
			return;
		}
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			return;
		}
		target = FindTarget(targetType);
		if (target != null)
		{
			target.onDestroyed += OnTargetDestroyed;
		}
	}

	private void OnTargetDestroyed(Target target)
	{
		target.onDestroyed -= OnTargetDestroyed;
		this.target = null;
		timer = cooldown;
	}

	private Target FindTarget(TargetType type)
	{
		for (int i = 0; i < faction.units.Count; i++)
		{
			if (faction.units[i].type == type)
			{
				return faction.units[i];
			}
		}
		return null;
	}
}
