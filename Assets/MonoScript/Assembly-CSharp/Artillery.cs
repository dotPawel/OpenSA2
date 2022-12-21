using UnityEngine;

public class Artillery : Turret
{
	public float minRange;

	private void FixedUpdate()
	{
		if (!(faction != null) || !GetTick(5) || !cannon.ready)
		{
			return;
		}
		base.target = null;
		float num = range;
		for (int i = 0; i < faction.enemies.Count; i++)
		{
			Target target = faction.enemies[i];
			if (target.type >= attackType)
			{
				float num2 = Mathf.Abs(target.position.x - base.position.x);
				if (num2 < num && num2 > minRange)
				{
					num = num2;
					base.target = target;
				}
			}
		}
		if (base.target != null)
		{
			cannon.Aim(base.target);
			cannon.Fire();
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.position, range);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.position, minRange);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(base.aimPoint, new Vector3(0.5f, 0f, 0.5f));
	}
}
