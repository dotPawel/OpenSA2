using UnityEngine;

public class Rifle : Weapon
{
	public Mount mount;

	public Transform firePoint;

	public AudioSource audioSource;

	public override void Aim(Target target)
	{
		firePoint.LookAt(target.aimPoint);
	}

	public override void ResetAim()
	{
		firePoint.localRotation = Quaternion.identity;
	}

	public override void Fire()
	{
		if (ready)
		{
			mount.Instantiate(firePoint.position, firePoint.rotation, damage, id);
			if (audioSource != null)
			{
				audioSource.Play();
			}
			base.Fire();
		}
	}
}
