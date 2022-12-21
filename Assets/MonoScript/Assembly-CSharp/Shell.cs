using UnityEngine;

public class Shell : Mount
{
	public float maxDistance;

	public float speed;

	public Vector3 gravity;

	public Effect explosionEffect;

	private float distance;

	private Surface surface;

	private void OnEnable()
	{
		distance = 0f;
	}

	private void Start()
	{
		surface = Surface.Find();
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		float num = deltaTime * speed;
		Vector3 vector = base.position;
		vector += base.forward * num;
		vector += gravity * deltaTime;
		if (vector != base.position)
		{
			base.forward = vector - base.position;
		}
		base.position = vector;
		if ((distance += num) > maxDistance || base.position.y < surface.GetHeight(base.position))
		{
			Remove();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (base.gameObject.active)
		{
			if (explosionEffect != null)
			{
				explosionEffect.Instantiate(base.position, base.rotation);
			}
			Target component = other.gameObject.GetComponent<Target>();
			if (component != null)
			{
				component.Hit(damage, targetId);
			}
			Remove();
		}
	}
}
