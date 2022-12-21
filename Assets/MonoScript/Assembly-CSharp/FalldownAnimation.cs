using UnityEngine;

public class FalldownAnimation : AObject
{
	public float gravity = 9.8f;

	public float smooth = 1f;

	public float centerOffset = 0.1f;

	public Effect explosionEffect;

	public ParticleSystem smokeEffect;

	public Vector3 rotateAngle;

	public Transform modelTransform;

	private Surface surface;

	private void OnEnable()
	{
		smokeEffect.Clear();
		smokeEffect.Play();
	}

	private void Start()
	{
		surface = Surface.Find();
	}

	private void Update()
	{
		float height = surface.GetHeight(base.position);
		if (height < base.position.y - centerOffset)
		{
			float deltaTime = Time.deltaTime;
			base.forward = Vector3.Slerp(base.forward, Vector3.down, smooth * deltaTime);
			base.position += base.forward * gravity * deltaTime;
			modelTransform.Rotate(rotateAngle * deltaTime, Space.Self);
		}
		else
		{
			Explode(base.position, base.rotation);
			Remove();
		}
	}

	public void Instantiate(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = SceneResources.Pop(base.gameObject, position, rotation);
		gameObject.SetActiveRecursively(true);
	}

	public void Explode(Vector3 position, Quaternion rotation)
	{
		if (explosionEffect != null)
		{
			explosionEffect.Instantiate(position, rotation);
		}
	}

	public void Remove()
	{
		smokeEffect.Stop();
		base.gameObject.SetActiveRecursively(false);
		SceneResources.Push(base.gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.position, centerOffset);
	}
}
