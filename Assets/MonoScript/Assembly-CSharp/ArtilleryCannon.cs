using UnityEngine;

public class ArtilleryCannon : Cannon
{
	public float animTime;

	public Interval pathHeight;

	public float maxRange;

	public Vector3 recoilAnimationPoint;

	private Vector3 targetPoint;

	private Vector3 centerPoint;

	private Vector3 defaultPlatformPosition;

	public override bool ready
	{
		get
		{
			return (double)timer <= 0.0;
		}
	}

	private void OnEnable()
	{
		defaultPlatformPosition = platform.localPosition;
	}

	public override void Aim(Target target)
	{
		targetPoint = target.aimPoint + target.forward;
		centerPoint = Vector3.Lerp(targetPoint, base.position, 0.5f);
		centerPoint.y = pathHeight.Lerp(Vector3.Distance(targetPoint, base.position) / maxRange);
	}

	public override void Fire()
	{
		if (ready)
		{
			ExplosionShell explosionShell = mount.Instantiate(firePoint.position, firePoint.rotation, damage, id) as ExplosionShell;
			if (explosionShell != null)
			{
				explosionShell.SetPath(firePoint.position, centerPoint, targetPoint);
			}
			platform.localPosition = recoilAnimationPoint;
			timer = cooldown;
			if (audioSource != null)
			{
				audioSource.Play();
			}
		}
	}

	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		if (timer > 0f)
		{
			timer -= deltaTime;
		}
		float num = cooldown - timer;
		if (num <= animTime)
		{
			platform.localPosition = Utils.Vector3Lerp(recoilAnimationPoint, defaultPlatformPosition, num / animTime);
		}
	}
}
