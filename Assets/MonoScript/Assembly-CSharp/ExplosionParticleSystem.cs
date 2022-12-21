using UnityEngine;

public class ExplosionParticleSystem : AObject
{
	private struct ExplosionParticle
	{
		public Transform location;

		public Vector3 direction;

		public float rotate;

		public float mass;

		public bool active;

		public Vector3 impulse { get; private set; }

		public Quaternion rotation { get; private set; }

		public Vector3 defaultPosition { get; private set; }

		public Quaternion defaultRotation { get; private set; }

		public ExplosionParticle(Transform location, Vector3 direction, float rotate, float mass = 1f)
			: this()
		{
			this.location = location;
			this.direction = direction;
			this.mass = mass;
			this.rotate = rotate;
			active = false;
			impulse = Vector3.zero;
			defaultPosition = location.localPosition;
			defaultRotation = location.localRotation;
		}

		public void ApplyForce(float force)
		{
			impulse = direction * force * 1f / mass;
			rotation = Quaternion.Euler(impulse / mass * rotate);
			active = true;
		}

		public void Update(Vector3 gravity, float groundHeight, float deltaTime)
		{
			if (active)
			{
				if (location.position.y > groundHeight)
				{
					impulse += gravity * deltaTime;
					location.localPosition += impulse * deltaTime;
					location.rotation *= rotation;
				}
				else
				{
					active = false;
				}
			}
		}

		public void ResetLocation()
		{
			location.localPosition = defaultPosition;
			location.localRotation = defaultRotation;
		}
	}

	private const float RADIUS = 0.05f;

	public Vector3 gravity = new Vector3(0f, -9.8f, 0f);

	public float minForce = 0.1f;

	public float maxForce = 10f;

	public float minLifeTime = 1f;

	public float maxLifeTime = 2f;

	public float explosionRange = 10f;

	public float rotateForce = 1f;

	public float groundHeight;

	public Vector3[] explosionPoints;

	public Vector3 bounds;

	public bool useMass;

	public bool loop;

	public bool emitAtStart;

	private Vector3 localGravity;

	private ExplosionParticle[] particles;

	private Surface surfaceCache;

	private float randomForce
	{
		get
		{
			return Random.Range(minForce, maxForce);
		}
	}

	private Surface surface
	{
		get
		{
			if (surfaceCache == null)
			{
				surfaceCache = Surface.Find();
			}
			return surfaceCache;
		}
	}

	private void OnEnable()
	{
		groundHeight = surface.GetHeight(base.position);
		localGravity = base.goTransform.InverseTransformDirection(gravity);
		if (emitAtStart && particles != null)
		{
			Emit();
		}
	}

	private void Start()
	{
		particles = new ExplosionParticle[base.transform.childCount];
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			Vector3 direction = CalculateImpulseVector(child.localPosition, explosionPoints);
			if (useMass)
			{
				Renderer component = child.GetComponent<Renderer>();
				float mass = component.bounds.size.magnitude / bounds.magnitude;
				particles[i] = new ExplosionParticle(child, direction, rotateForce, mass);
			}
			else
			{
				particles[i] = new ExplosionParticle(child, direction, rotateForce);
			}
		}
		if (emitAtStart)
		{
			Emit();
		}
	}

	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		int num = 0;
		for (int i = 0; i < particles.Length; i++)
		{
			if (particles[i].active)
			{
				particles[i].Update(localGravity, groundHeight, deltaTime);
				num++;
			}
		}
		if (num == 0 && loop)
		{
			Emit();
		}
	}

	public void Emit()
	{
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].ResetLocation();
			particles[i].ApplyForce(randomForce);
		}
	}

	private Vector3 CalculateImpulseVector(Vector3 target, Vector3[] points)
	{
		Vector3 result = Vector3.zero;
		if (points.Length > 0)
		{
			foreach (Vector3 vector in points)
			{
				Vector3 vector2 = target - vector;
				result += vector2.normalized * Mathf.Clamp01(1f - vector2.magnitude / explosionRange);
			}
		}
		else
		{
			result = target.normalized * Mathf.Clamp01(1f - target.magnitude / explosionRange);
		}
		return result;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		if (explosionPoints != null && explosionPoints.Length > 0)
		{
			for (int i = 0; i < explosionPoints.Length; i++)
			{
				Gizmos.DrawSphere(base.transform.position + explosionPoints[i], 0.05f);
				Gizmos.DrawWireSphere(base.transform.position + explosionPoints[i], explosionRange);
			}
		}
		else
		{
			Gizmos.DrawSphere(base.transform.position, 0.05f);
			Gizmos.DrawWireSphere(base.transform.position, explosionRange);
		}
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(base.transform.position, bounds);
	}
}
