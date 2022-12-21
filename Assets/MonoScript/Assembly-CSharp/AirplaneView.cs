using UnityEngine;

public class AirplaneView : AObject
{
	public Airplane airplane;

	public Transform model;

	public float pitchAngle = 20f;

	public float smooth = 1f;

	public UnitAnimation wheelsAnimation;

	public UnitAnimation engineAnimation;

	public float wheelsHeight = 1f;

	public float wheelsAnimationSpeed = 0.5f;

	public float landingAngle = 15f;

	public ParticleSystem smokeEffect;

	public float startSmokeRatio = 0.7f;

	public Interval smokeRate = new Interval(2f, 5f);

	public AirplaneShadow airplaneShadowPrefab;

	public FalldownAnimation falldownAnimation;

	public Transform titleTransform;

	public AudioSource audioSource;

	public AudioClip[] clips;

	public float audioFadeTime = 1f;

	public float audioPitchEffect = 0.1f;

	public bool availabelLanding;

	private float wheelsAnimationRate;

	private Quaternion modelRotation;

	private Quaternion landingRotation;

	private Vector3 titleOffset = new Vector3(0f, 0.3f, 0f);

	private Quaternion titleRotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

	private AirplaneShadow shadow;

	private AirMovement movement;

	private float audioTimer;

	private float audioLastTime;

	public bool title
	{
		set
		{
			titleTransform.GetComponent<Renderer>().enabled = value;
		}
	}

	private void OnEnable()
	{
		modelRotation = Quaternion.identity;
		model.localRotation = modelRotation;
		wheelsAnimationRate = 0f;
		wheelsAnimation.SetRate(wheelsAnimationRate);
		shadow = airplaneShadowPrefab.Instantiate(base.position, Quaternion.identity);
		smokeEffect.Clear();
		smokeEffect.Play();
		smokeEffect.emissionRate = 0f;
		if (audioSource != null)
		{
			audioSource.volume = 0f;
			audioTimer = 0f;
		}
	}

	private void Start()
	{
		landingRotation = Quaternion.Euler(Vector3.left * landingAngle);
		airplane.onHealthUpdated += OnHealthUpdated;
		airplane.onDestroyed += OnAirplaneDestroyed;
		movement = airplane.movement;
		movement.onRitchUpdate += OnRitchUpdate;
		movement.onRoll += OnRoll;
		movement.onStateChanged += OnMovementStateChanged;
	}

	private void OnAirplaneDestroyed(Target target)
	{
		shadow.Remove();
	}

	private void OnHealthUpdated(Target target)
	{
		smokeEffect.emissionRate = ((!(target.healthRate > startSmokeRatio)) ? smokeRate.Lerp(1f - target.healthRate / startSmokeRatio) : 0f);
	}

	private void OnMovementStateChanged(AirMovement movement, AirMovement.State state)
	{
		switch (state)
		{
		case AirMovement.State.Takeoff:
			modelRotation = Quaternion.identity;
			model.localRotation = Quaternion.identity;
			break;
		case AirMovement.State.Landing:
			modelRotation = landingRotation;
			break;
		}
	}

	private void OnRoll(AirMovement movement)
	{
		modelRotation = Quaternion.identity;
		model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles - Vector3.forward * 180f);
	}

	private void OnRitchUpdate(AirMovement movement, float value)
	{
		if (value != 0f)
		{
			modelRotation = Quaternion.Euler(Vector3.back * pitchAngle * value);
		}
		else
		{
			modelRotation = Quaternion.identity;
		}
	}

	private void Update()
	{
		if (!(audioSource != null))
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (deltaTime == 0f)
		{
			if (audioTimer > 0f)
			{
				deltaTime = Time.realtimeSinceStartup - audioLastTime;
				audioTimer -= deltaTime;
				audioSource.volume = Utils.Lerp(0f, 1f, audioTimer / audioFadeTime);
			}
		}
		else if (audioTimer < audioFadeTime)
		{
			audioTimer += deltaTime;
			audioSource.volume = Utils.Lerp(0f, 1f, audioTimer / audioFadeTime);
		}
		audioSource.pitch = Mathf.Max(movement.speedRate, 0.1f) + Vector3.Dot(base.forward, Vector3.up) * audioPitchEffect;
		audioLastTime = Time.realtimeSinceStartup;
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			audioLastTime = Time.realtimeSinceStartup;
		}
	}

	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		engineAnimation.SetRate(deltaTime * movement.speed);
		model.localRotation = Quaternion.Slerp(model.localRotation, modelRotation, smooth * deltaTime);
		if (movement.height > wheelsHeight)
		{
			if (wheelsAnimationRate < 1f)
			{
				wheelsAnimationRate += deltaTime * wheelsAnimationSpeed;
			}
		}
		else if (availabelLanding && wheelsAnimationRate > 0f)
		{
			wheelsAnimationRate -= deltaTime * wheelsAnimationSpeed;
		}
		wheelsAnimation.SetRate(wheelsAnimationRate);
	}

	private void LateUpdate()
	{
		shadow.position = movement.projectionPoint;
		Vector3 vector = new Vector3(1f, 1f, Utils.Lerp(1f, 0.1f, Vector3.Angle(base.normal, Vector3.up) / 180f));
		shadow.localScale = vector * Utils.Lerp(1f, 0f, base.position.y / 10f);
		titleTransform.rotation = titleRotation;
		titleTransform.position = base.position + titleOffset;
	}

	public void CriticalHit()
	{
	}

	public void Shotdown(bool explode = false)
	{
		if (falldownAnimation != null)
		{
			if (explode)
			{
				falldownAnimation.Explode(base.position, base.rotation);
			}
			else
			{
				falldownAnimation.Instantiate(base.position, base.rotation);
			}
		}
	}

	public void SetSoundClip(int index)
	{
		if (audioSource != null)
		{
			audioSource.clip = clips[index];
			audioSource.Play();
		}
	}

	private Quaternion GetShadowRotation()
	{
		return Quaternion.LookRotation((!(Vector3.Dot(base.forward, Vector3.right) > 0f)) ? Vector3.left : Vector3.right, Vector3.up);
	}
}
