using UnityEngine;

public class ExplosionShell : Mount
{
	public float splashRange = 1f;

	public float speed = 2f;

	public LayerMask collideMask;

	public Effect explosionEffect;

	private QuadBez quadBez = new QuadBez();

	private Surface surfaceReference;

	private float time;

	private float timer;

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

	public void SetPath(Vector3 startPoint, Vector3 middlePoint, Vector3 endPoint)
	{
		endPoint.y = surface.GetHeight(endPoint);
		quadBez.Init(startPoint, middlePoint, endPoint);
		time = Vector3.Distance(startPoint, endPoint) / speed;
		timer = 0f;
	}

	private void Update()
	{
		if (timer < time)
		{
			timer += Time.deltaTime;
			Vector3 vector = quadBez.Interp(timer / time);
			base.forward = vector - base.position;
			base.position = vector;
			if (timer >= time)
			{
				Explosion();
			}
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
}
