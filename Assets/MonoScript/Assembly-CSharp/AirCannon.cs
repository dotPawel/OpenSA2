using UnityEngine;

public class AirCannon : Cannon
{
	public Interval aimOffset = new Interval(0.1f, 1f);

	public float aimRange = 6f;

	public override bool ready
	{
		get
		{
			return (double)timer <= 0.0 && Quaternion.Angle(aimRotation, platform.rotation) < aimAngle;
		}
	}

	private void OnEnable()
	{
		aimRotation = platform.rotation;
	}

	public override void Aim(Target target)
	{
		float t = Mathf.Clamp01(Vector3.Distance(target.position, base.position) / aimRange);
		aimRotation = Quaternion.LookRotation(target.aimPoint + target.forward * aimOffset.Lerp(t) - base.position);
	}

	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		if (timer > 0f)
		{
			timer -= deltaTime;
		}
		if (platform.rotation != aimRotation)
		{
			float num = Quaternion.Angle(platform.rotation, aimRotation);
			platform.rotation = Quaternion.Lerp(platform.rotation, aimRotation, rotateSpeed / num * Time.deltaTime);
		}
	}
}
