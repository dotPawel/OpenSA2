using UnityEngine;

public class Bomb : Mount
{
	public float gravity = 3f;

	public float splashRange = 1f;

	public float speed = 2f;

	public float acceleration = 4f;

	public LayerMask collideMask;

	public Effect explosionEffect;

	private Surface surfaceReference;

	private Vector3 direction;

	private float height;

	private Surface surface
	{
		get
		{
			if (surfaceReference == null)
			{
				surfaceReference = Surface.Find();
			}
			return surfaceReference;
		}
	}

	private void OnEnable()
	{
		Vector3 point = base.position;
		point.y = surface.GetHeight(point);
		float num = (base.position.y - point.y) / gravity;
		direction = base.forward;
		point += direction * num * speed;
		height = surface.GetHeight(point);
	}

	private void Update()
	{
		if (base.position.y > height)
		{
			float deltaTime = Time.deltaTime;
			float num = Vector3.Angle(base.forward, Vector3.down) / 180f;
			base.position += (Vector3.down * gravity + base.forward * (1f - num) * speed) * deltaTime;
			base.forward = Vector3.Lerp(base.forward, Vector3.down, num * acceleration * deltaTime);
		}
		else
		{
			Explosion();
		}
	}

	private void Explosion()
	{
		Collider[] array = Physics.OverlapSphere(base.position, splashRange, collideMask);
		for (int i = 0; i < array.Length; i++)
		{
			Target component = array[i].GetComponent<Target>();
			if (component != null)
			{
				component.Hit(damage, targetId);
			}
		}
		if (explosionEffect != null)
		{
			explosionEffect.Instantiate(base.position, Quaternion.identity);
		}
		Remove();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.position, splashRange);
	}
}
