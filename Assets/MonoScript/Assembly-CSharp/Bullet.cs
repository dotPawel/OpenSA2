using UnityEngine;

public class Bullet : Mount
{
	public float scatter = 0.1f;

	public float maxDistance;

	public float speed;

	public Effect collisionEffect;

	public Effect scorchEffect;

	private float distance;

	private Surface surface;

	private void OnEnable()
	{
		base.forward += base.goTransform.InverseTransformDirection(Vector3.up) * Random.Range(0f - scatter, scatter);
		distance = 0f;
	}

	private void Start()
	{
		surface = Surface.Find();
	}

	private void Update()
	{
		float num = Time.deltaTime * speed;
		base.position += base.forward * num;
		if ((distance += num) > maxDistance)
		{
			Remove();
		}
		else if (base.position.y < surface.GetHeight(base.position))
		{
			if (scorchEffect != null)
			{
				scorchEffect.Instantiate(base.position, base.rotation);
			}
			Remove();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!base.gameObject.active)
		{
			return;
		}
		Target component = other.gameObject.GetComponent<Target>();
		if (component != null)
		{
			component.Hit(damage, targetId);
			if (collisionEffect != null && component.healthRate > 0f)
			{
				collisionEffect.Instantiate(base.position, base.rotation);
			}
		}
		Remove();
	}
}
