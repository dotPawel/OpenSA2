using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MachineGun : Weapon
{
	private const float DAMAGE_BONUS = 1f;

	public Mount mount;

	public Transform firePoint;

	public FlameEffect flameEffect;

	public int maxAmmoCount = 100;

	private int ammoCount;

	private int power;

	public float ammoRate
	{
		get
		{
			return (float)ammoCount / (float)maxAmmoCount;
		}
	}

	public override bool ready
	{
		get
		{
			return base.ready && ammoCount > 0;
		}
	}

	private float totalDamage
	{
		get
		{
			return damage + (float)power * 1f;
		}
	}

	[method: MethodImpl(32)]
	public event Action<MachineGun> onAmmoUpdated;

	private void OnEnable()
	{
		Reload();
	}

	public override void Fire()
	{
		if (ready)
		{
			if (flameEffect != null)
			{
				flameEffect.Play();
			}
			mount.Instantiate(firePoint.position, firePoint.rotation, totalDamage, id);
			ammoCount--;
			if (this.onAmmoUpdated != null)
			{
				this.onAmmoUpdated(this);
			}
			base.Fire();
		}
	}

	public void Reload()
	{
		SetAmmo(maxAmmoCount);
	}

	public void SetPower(int value)
	{
		power = value;
	}

	public void SetAmmo(int value)
	{
		ammoCount = (maxAmmoCount = value);
		if (this.onAmmoUpdated != null)
		{
			this.onAmmoUpdated(this);
		}
	}

	public void SetSoundType(int index)
	{
		flameEffect.SetSoundClip(index);
	}
}
