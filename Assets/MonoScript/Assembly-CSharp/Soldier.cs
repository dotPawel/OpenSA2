using UnityEngine;

public class Soldier : Unit
{
	private const int TICK = 10;

	public LandMovement movement;

	public Weapon weapon;

	public Transform leftLeg;

	public Transform rightLeg;

	public float stepAngle = 15f;

	public float animSpeed = 1f;

	public float vision;

	public Target target;

	public TargetType targetType = TargetType.Ground;

	public Effect dieEffect;

	private float time;

	private Quaternion forwardRotation;

	private Quaternion backRotation;

	private Faction faction;

	private Vector3 targetPoint;

	private void OnEnable()
	{
		RestoreHealth();
		target = null;
	}

	private void Start()
	{
		forwardRotation = Quaternion.Euler(Vector3.right * stepAngle);
		backRotation = Quaternion.Euler(Vector3.left * stepAngle);
		faction = Faction.Find(base.tag);
		Scenario scenario = Scenario.Find();
		targetPoint = scenario.GetTargetPoint(faction);
	}

	private void FixedUpdate()
	{
		UpdateBehaviour();
		if (movement.speed > 0f)
		{
			time += Time.deltaTime * animSpeed;
			float t = Mathf.PingPong(time, 1f);
			rightLeg.localRotation = Quaternion.Lerp(forwardRotation, backRotation, t);
			leftLeg.localRotation = Quaternion.Lerp(backRotation, forwardRotation, t);
		}
		else
		{
			rightLeg.localRotation = Quaternion.identity;
			leftLeg.localRotation = Quaternion.identity;
		}
	}

	public override void SetTargetId(int value)
	{
		base.SetTargetId(value);
		weapon.SetId(value);
	}

	public override void Explode()
	{
		if (dieEffect != null)
		{
			dieEffect.Instantiate(base.position, base.rotation);
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
				Vector3 normalized = (target.position - base.position).normalized;
				normalized.y = 0f;
				base.rotation = Quaternion.LookRotation(normalized, Vector3.up);
				if (weapon.ready)
				{
					weapon.Aim(target);
					weapon.Fire();
				}
			}
			else
			{
				target = null;
			}
		}
		else
		{
			if (movement.state == LandMovement.State.Stop)
			{
				movement.Move((targetPoint - base.position).normalized);
			}
			if (GetTick(10))
			{
				target = FindTarget(base.position, vision);
			}
		}
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
