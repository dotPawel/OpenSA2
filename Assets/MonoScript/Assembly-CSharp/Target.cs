using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Target : AObject
{
	private const float ARMOR_BONUS = 10f;

	public TargetType type;

	public int id;

	public float health;

	public Vector3 aimCenter;

	public bool captureArea;

	public bool immortal;

	protected float damage;

	protected float armor;

	public float totalHealth
	{
		get
		{
			return health + armor * 10f;
		}
	}

	public float healthRate
	{
		get
		{
			return Mathf.Clamp01(1f - damage / totalHealth);
		}
	}

	public virtual bool alive
	{
		get
		{
			return base.gameObject.active;
		}
	}

	public Vector3 aimPoint
	{
		get
		{
			return base.position + aimCenter;
		}
	}

	[method: MethodImpl(32)]
	public event Action<Target> onDestroyed;

	[method: MethodImpl(32)]
	public event Action<Target, int> onHit;

	[method: MethodImpl(32)]
	public event Action<Target> onHealthUpdated;

	public virtual void Hit(float damage, int targetId)
	{
		if (!immortal && this.damage < totalHealth)
		{
			SetDamage(this.damage + damage);
			if (this.onHit != null)
			{
				this.onHit(this, targetId);
			}
			if (this.damage >= totalHealth)
			{
				Explode();
			}
		}
	}

	public void SetArmor(int value)
	{
		armor = value;
	}

	public virtual void Destroy()
	{
		if (this.onDestroyed != null)
		{
			this.onDestroyed(this);
		}
	}

	public virtual void Explode()
	{
	}

	public virtual void RestoreHealth(float rate = 1f)
	{
		SetDamage(damage - totalHealth * rate);
	}

	public virtual void SetTargetId(int value)
	{
		id = value;
	}

	private void SetDamage(float value)
	{
		damage = Mathf.Clamp(value, 0f, totalHealth);
		if (this.onHealthUpdated != null)
		{
			this.onHealthUpdated(this);
		}
	}
}
