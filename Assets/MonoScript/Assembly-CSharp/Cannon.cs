using UnityEngine;

public class Cannon : Weapon
{
	public Mount mount;

	public Transform platform;

	public Transform firePoint;

	public float rotateSpeed = 30f;

	public float aimAngle = 10f;

	public float aimHeight = 0.4f;

	public AudioSource audioSource;

	protected Quaternion aimRotation = Quaternion.identity;

	public override bool ready
	{
		get
		{
			return base.ready && Quaternion.Angle(aimRotation, platform.localRotation) < aimAngle;
		}
	}

	private void OnEnable()
	{
		aimRotation = Quaternion.identity;
	}

	private void Start()
	{
		Surface surface = Surface.Find();
		Vector3 point = base.position;
		point.y = surface.GetHeight(point);
		base.position = point;
	}

	public override void Aim(Target target)
	{
		Vector3 vector = base.goTransform.InverseTransformDirection(target.aimPoint - base.position);
		vector.y = 0f;
		if (vector != Vector3.zero)
		{
			aimRotation = Quaternion.LookRotation(vector, Vector3.up);
		}
		firePoint.LookAt(target.aimPoint + Vector3.up * aimHeight);
	}

	public override void ResetAim()
	{
		aimRotation = Quaternion.identity;
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

	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		if (timer > 0f)
		{
			timer -= deltaTime;
		}
		if (aimRotation != Quaternion.identity)
		{
			float num = Quaternion.Angle(platform.rotation, aimRotation);
			platform.localRotation = Quaternion.Lerp(platform.localRotation, aimRotation, rotateSpeed / num * Time.deltaTime);
		}
	}
}
