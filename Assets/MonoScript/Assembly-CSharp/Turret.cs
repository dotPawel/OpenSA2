using UnityEngine;

public class Turret : Target
{
	protected const int TICK = 5;

	public Faction faction;

	public TargetType attackType;

	public Target target;

	public Cannon cannon;

	public float range;

	public Mount[] mounts;

	public DynamicLinearBar progressBar;

	public Transform explosionLocation;

	public Effect explosionEffect;

	public void SetFaction(Faction value)
	{
		faction = value;
		if (!(faction != null))
		{
			return;
		}
		base.tag = faction.tag;
		base.gameObject.layer = faction.gameObject.layer;
		progressBar.firstColor = faction.color;
		progressBar.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
		progressBar.SetProgress(1f);
		faction.AddUnit(this);
		for (int i = 0; i < mounts.Length; i++)
		{
			Mount mount = mounts[i];
			if (mount.tag == base.tag)
			{
				cannon.mount = mount;
				break;
			}
		}
	}

	public override void Hit(float damage, int targetId)
	{
		base.Hit(damage, targetId);
		progressBar.SetProgress(base.healthRate);
	}

	public override void Explode()
	{
		if (explosionEffect != null)
		{
			explosionEffect.Instantiate(explosionLocation.position, explosionLocation.rotation);
		}
		base.show = false;
		Destroy();
	}

	private void FixedUpdate()
	{
		if (target != null)
		{
			if (target.alive && Utils.IsRangeLow(base.position, target.position, range))
			{
				cannon.Aim(target);
				cannon.Fire();
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

	protected virtual void UpdateTarget()
	{
		if (!(faction != null))
		{
			return;
		}
		Target target = ((!(this.target != null) || !this.target.alive) ? null : this.target);
		for (int i = 0; i < faction.enemies.Count; i++)
		{
			Target target2 = faction.enemies[i];
			if (target2.type >= attackType && Utils.IsRangeLow(base.position, target2.position, range) && (target == null || target2.type < target.type))
			{
				target = target2;
			}
		}
		this.target = target;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.position, range);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(base.aimPoint, new Vector3(0.5f, 0f, 0.5f));
	}
}
