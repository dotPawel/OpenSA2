using UnityEngine;

public class Fighter : AObject
{
	private const int DEFAULT_EQUIPMENT = 3;

	private const float OVERHEAT_PENALTY = 5f;

	private const float MIN_HEIGTH_RATE = 2f;

	private const float DIRECTION_ANGLE = 3f;

	private const float BOMB_OFFSET = 2f;

	private const float BOMB_RANGE = 1f;

	private const float MAX_HEIGHT = 10f;

	private const float MIN_BOMB_TARGET_HEALTH = 25f;

	private const int TICK = 5;

	public Airplane airplane;

	public Target target;

	public float vision = 20f;

	public float aimRange = 10f;

	public float attackRange = 7f;

	public float attackAngle = 15f;

	public float avoidRange = 6f;

	public float minHeight = 0.5f;

	public float maxHeight = 7f;

	public float bombCooldown = 10f;

	public int armor;

	public int mobility;

	public int damage;

	public float heat = 0.1f;

	public float cooling = 0.1f;

	public bool availabelLanding;

	public TargetType targetType = TargetType.Air;

	private float rotateRatio;

	private float overheat;

	private float cooldown;

	private Faction faction;

	private float bombTimer;

	private float optimalHeight;

	public void Init(Airplane airplane)
	{
		this.airplane = airplane;
		target = null;
		faction = Faction.Find(airplane.tag);
		airplane.SetArmor(armor);
		airplane.movement.SetMobility(mobility);
		airplane.machineGun.SetPower(damage);
		airplane.SetAvailabelLanding(availabelLanding);
		airplane.view.title = true;
		airplane.SetEquipment(3);
		airplane.movement.onStateChanged += OnAirplaneMovementStateChanged;
		airplane.onDestroyed += OnAirplaneDestroyed;
		airplane.SetSoundType(1);
		avoidRange = Random.Range(1f, 4f);
		optimalHeight = (float)Random.Range(14, 24) * 0.25f;
		rotateRatio = Random.Range(0.8f, 1f);
	}

	private void OnAirplaneMovementStateChanged(AirMovement movement, AirMovement.State state)
	{
		if (state == AirMovement.State.Landed)
		{
			movement.Takeoff();
		}
	}

	private void OnAirplaneDestroyed(Target airplane)
	{
		this.airplane.movement.onStateChanged -= OnAirplaneMovementStateChanged;
		airplane.onDestroyed -= OnAirplaneDestroyed;
		this.airplane = null;
	}

	private void Update()
	{
		if (airplane != null)
		{
			Vector3 airplanePosition = airplane.position;
			Vector3 airplaneDirection = airplane.forward;
			if (GetTick(5))
			{
				UpdateTarget(airplanePosition, airplaneDirection);
			}
			UpdateBehaviour(airplanePosition, airplaneDirection);
			MachinGunAttack(airplanePosition, airplaneDirection);
			if (bombTimer <= 0f)
			{
				BombAttack(airplanePosition, airplaneDirection);
			}
			else
			{
				bombTimer -= Time.deltaTime;
			}
		}
	}

	private void UpdateBehaviour(Vector3 airplanePosition, Vector3 airplaneDirection)
	{
		Vector3 vector = Vector3.zero;
		float y = airplanePosition.y;
		if (airplaneDirection.y > 0f && y > maxHeight - airplaneDirection.y)
		{
			vector = airplanePosition + ((!(airplaneDirection.x > 0f)) ? Vector3.left : Vector3.right) + Vector3.down;
		}
		else if (airplaneDirection.y < 0f && y < minHeight + Mathf.Abs(airplaneDirection.y) * 2f)
		{
			vector = airplanePosition + ((!(airplaneDirection.x > 0f)) ? Vector3.left : Vector3.right) + Vector3.up;
		}
		else if (target != null && target.alive)
		{
			Vector3 vector2 = target.position;
			if (Utils.IsRangeLow(vector2, airplanePosition, avoidRange) && Vector3.Dot(airplaneDirection, target.forward) < 0.3f)
			{
				vector = airplanePosition + airplaneDirection;
				if (vector.y > maxHeight || vector.y < minHeight)
				{
					vector.y = airplanePosition.y;
				}
			}
			if (Utils.IsRangeHigh(vector2, airplanePosition, aimRange))
			{
				vector2.y = optimalHeight;
				vector = vector2;
			}
			else
			{
				vector = vector2;
			}
		}
		Vector3 normalized = (vector - airplanePosition).normalized;
		if (Vector3.Angle(normalized, airplaneDirection) > 3f)
		{
			airplane.Move(Vector3.up * Vector3.Dot(airplane.normal, normalized) * rotateRatio);
		}
		else
		{
			airplane.Move(Vector3.zero);
		}
	}

	private void BombAttack(Vector3 airplanePosition, Vector3 airplaneDirection)
	{
		if (!airplane.bombLauncher.ready)
		{
			return;
		}
		Vector3 vector = airplanePosition + airplaneDirection * Utils.Lerp(0f, 2f, airplane.movement.height / 10f);
		for (int i = 0; i < faction.enemies.Count; i++)
		{
			Target target = faction.enemies[i];
			if (target.type >= TargetType.Ground && target.health > 25f && Mathf.Abs(target.position.x - vector.x) < 1f)
			{
				airplane.DropBomb();
				bombTimer = bombCooldown;
				break;
			}
		}
	}

	private void MachinGunAttack(Vector3 airplanePosition, Vector3 airplaneDirection)
	{
		if (!airplane.machineGun.ready)
		{
			return;
		}
		if (cooldown <= 0f)
		{
			bool flag = false;
			for (int i = 0; i < faction.enemies.Count; i++)
			{
				Target target = faction.enemies[i];
				Vector3 from = target.position - airplanePosition;
				if (target.type >= targetType && Utils.IsRangeLow(target.position, airplanePosition, attackRange) && Vector3.Angle(from, airplaneDirection) < attackAngle)
				{
					flag = true;
					airplane.Fire();
					if ((overheat += heat) > 1f)
					{
						overheat = 0f;
						cooldown = 5f;
					}
					break;
				}
			}
			if (!flag && overheat > 0f)
			{
				overheat -= cooling * Time.deltaTime;
			}
		}
		else
		{
			cooldown -= Time.deltaTime;
		}
	}

	private void UpdateTarget(Vector3 airplanePosition, Vector3 airplaneDirection)
	{
		Target target = ((!(this.target != null) || !this.target.alive) ? null : this.target);
		float num = ((!(target != null)) ? float.PositiveInfinity : (target.position - airplanePosition).sqrMagnitude);
		float num2 = ((!(target != null)) ? (-1f) : Vector3.Dot((target.position - airplanePosition).normalized, airplaneDirection));
		float num3 = vision * vision;
		for (int i = 0; i < faction.enemies.Count; i++)
		{
			Target target2 = faction.enemies[i];
			if (target2.type < targetType)
			{
				continue;
			}
			if (target == null)
			{
				target = target2;
				num = (target2.position - airplanePosition).sqrMagnitude;
				if (num < num3)
				{
					num2 = Vector3.Dot((target2.position - airplanePosition).normalized, airplaneDirection);
				}
				continue;
			}
			float sqrMagnitude = (target2.position - airplanePosition).sqrMagnitude;
			if (sqrMagnitude < num3)
			{
				if (target2.type < target.type)
				{
					target = target2;
					num = sqrMagnitude;
					num2 = Vector3.Dot((target2.position - airplanePosition).normalized, airplaneDirection);
				}
				else if (target2.type == target.type)
				{
					float num4 = Vector3.Dot((target2.position - airplanePosition).normalized, airplaneDirection);
					if (num4 > num2)
					{
						num2 = num4;
						num = sqrMagnitude;
						target = target2;
					}
				}
			}
			else if (target2.type < target.type || (target2.type == target.type && sqrMagnitude < num))
			{
				target = target2;
				num = sqrMagnitude;
			}
		}
		this.target = target;
	}
}
