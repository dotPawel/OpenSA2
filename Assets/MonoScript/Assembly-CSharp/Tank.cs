using UnityEngine;

public class Tank : Unit
{
	private const int TICK = 10;

	private const float OBSTACLE_STOPPING_DISTANCE = 1.4f;

	public Cannon cannon;

	public LandMovement movement;

	public float vision;

	public Target target;

	public TargetType targetType = TargetType.Ground;

	public Effect exposionEffect;

	public Target obstacle;

	public AudioSource audioSource;

	public float audioFadeTime = 1f;

	private Faction faction;

	private Vector3 targetPoint;

	private float audioTimer;

	private float audioLastTime;

	private void OnEnable()
	{
		RestoreHealth();
		target = null;
		cannon.ResetAim();
		if (audioSource != null)
		{
			audioSource.volume = 0f;
			audioTimer = 0f;
		}
	}

	private void Start()
	{
		faction = Faction.Find(base.tag);
		Scenario scenario = Scenario.Find();
		targetPoint = scenario.GetTargetPoint(faction);
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
			audioSource.volume = Mathf.Min(Utils.Lerp(0f, 1f, audioTimer / audioFadeTime), movement.speed / movement.maxSpeed);
		}
		else
		{
			audioSource.volume = movement.speed / movement.maxSpeed;
		}
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
		UpdateBehaviour();
	}

	public override void SetTargetId(int value)
	{
		base.SetTargetId(value);
		cannon.SetId(value);
	}

	public override void Explode()
	{
		if (exposionEffect != null)
		{
			exposionEffect.Instantiate(base.position, base.rotation);
		}
		Destroy();
	}

	private void UpdateBehaviour()
	{
		if (target != null)
		{
			if (target.alive && Utils.IsRangeLow(base.position, target.position, vision))
			{
				movement.Stop();
				cannon.Aim(target);
				if (cannon.ready)
				{
					cannon.Fire();
				}
			}
			else
			{
				target = null;
				cannon.ResetAim();
			}
		}
		else
		{
			if (!GetTick(10))
			{
				return;
			}
			if (ChechObstacle())
			{
				if (movement.state == LandMovement.State.Moving)
				{
					movement.Stop();
				}
			}
			else if (movement.state == LandMovement.State.Stop)
			{
				movement.Move((targetPoint - base.position).normalized);
			}
			target = FindTarget(base.position, vision);
		}
	}

	private bool ChechObstacle()
	{
		for (int i = 0; i < faction.units.Count; i++)
		{
			Target target = faction.units[i];
			if (target != this && target.type == TargetType.Ground && target.position.z == base.position.z)
			{
				float f = target.position.x - base.position.x;
				if (Mathf.Abs(f) < 1.4f && Mathf.Sign(f) == Mathf.Sign(base.forward.x))
				{
					obstacle = target;
					return true;
				}
			}
		}
		obstacle = null;
		return false;
	}

	private Target FindTarget(Vector3 point, float range)
	{
		for (int i = 0; i < faction.enemies.Count; i++)
		{
			Target target = faction.enemies[i];
			if (target.type >= targetType && Utils.IsRangeLow(base.position, target.position, vision))
			{
				return target;
			}
		}
		return null;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.position, vision);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(base.aimPoint, new Vector3(0.5f, 0f, 0.5f));
	}
}
