using UnityEngine;

public class AirTurret : Turret
{
	public float visionAngle = 60f;

	private void FixedUpdate()
	{
		if (target != null)
		{
			if (target.alive && Utils.IsRangeLow(base.position, target.position, range))
			{
				Vector3 from = target.position - base.position;
				if (Vector3.Angle(from, Vector3.up) < visionAngle)
				{
					cannon.Aim(target);
					cannon.Fire();
				}
				else
				{
					target = null;
				}
			}
			else
			{
				target = null;
			}
		}
		else if (GetTick(5))
		{
			UpdateTarget();
		}
	}

	protected override void UpdateTarget()
	{
		if (!(faction != null) || !(base.target == null))
		{
			return;
		}
		for (int i = 0; i < faction.enemies.Count; i++)
		{
			Target target = faction.enemies[i];
			if (target.type == attackType && Utils.IsRangeLow(base.position, target.position, range))
			{
				Vector3 from = target.position - base.position;
				if (Vector3.Angle(from, Vector3.up) < visionAngle)
				{
					base.target = target;
					break;
				}
			}
		}
	}
}
