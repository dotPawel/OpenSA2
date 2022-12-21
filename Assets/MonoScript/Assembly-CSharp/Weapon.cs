using UnityEngine;

public class Weapon : AObject
{
	public int id;

	public float damage;

	public float cooldown;

	protected float timer;

	public virtual bool ready
	{
		get
		{
			return (double)timer <= 0.0;
		}
	}

	public virtual void Aim(Target target)
	{
	}

	public virtual void ResetAim()
	{
	}

	public virtual void Fire()
	{
		timer = cooldown;
	}

	public virtual void SetId(int value)
	{
		id = value;
	}

	private void FixedUpdate()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
		}
	}
}
