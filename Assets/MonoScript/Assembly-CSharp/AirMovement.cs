using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AirMovement : Movement
{
	public enum State
	{
		Takeoff = 0,
		Flying = 1,
		Landing = 2,
		Falling = 3,
		Landed = 4
	}

	private const float RADUIS = 0.2f;

	private const float ACCELERATION = 2f;

	private const float TAKEOFF_ANGLE = 15f;

	private const float FUEL_PENALTY = 3f;

	private const float FALLING_GRAVITY = 0.3f;

	private const float FALLING_SPEED = 4f;

	private const float FALLING_ROTATE_ANGLE = 30f;

	private const float FLY_HEIGHT = 1f;

	private const float START_FALLING_SPEED = 2f;

	private const float MIN_LANDING_SPEED = 0.1f;

	private const float MOBILITY_SPEED = 0.1f;

	private const float MOBILITY_ROTATE = 10f;

	private const float ROTATE_ACCELERATION = 4f;

	public float rotateAngle;

	public float fuelCapacity;

	public float landingAngle;

	public State state;

	public bool availabelLanding;

	private FlyingArea flyingArea;

	private Surface surface;

	private int mobility;

	private float surfaceHeight;

	private float rotateAcceleration;

	private float rotateVelocity;

	public Vector3 direction { get; private set; }

	public Vector3 projectionPoint { get; private set; }

	public float fuel { get; private set; }

	public float height { get; private set; }

	public float fuelRate
	{
		get
		{
			return fuel / fuelCapacity;
		}
	}

	public float speedRate
	{
		get
		{
			return base.speed / totalMaxSpeed;
		}
	}

	private float totalMaxSpeed
	{
		get
		{
			return maxSpeed + (float)mobility * 0.1f;
		}
	}

	private float totalRotateAngle
	{
		get
		{
			return rotateAngle + (float)mobility * 10f;
		}
	}

	[method: MethodImpl(32)]
	public event Action<AirMovement> onFuelUpdated;

	[method: MethodImpl(32)]
	public event Action<AirMovement, float> onRitchUpdate;

	[method: MethodImpl(32)]
	public event Action<AirMovement, State> onStateChanged;

	[method: MethodImpl(32)]
	public event Action<AirMovement> onRoll;

	[method: MethodImpl(32)]
	public event Action<AirMovement> onSmashed;

	private void Start()
	{
		flyingArea = FlyingArea.Find();
		surface = Surface.Find();
	}

	private void OnEnable()
	{
		fuel = fuelCapacity;
		Vector3 vector = base.position;
		vector.z = 1f * (float)layer;
		base.position = vector;
		Takeoff();
	}

	private void Update()
	{
		Vector3 point = base.position;
		float deltaTime = Time.deltaTime;
		switch (state)
		{
		case State.Takeoff:
			base.speed = Utils.Lerp(base.speed, totalMaxSpeed, deltaTime * 2f);
			point += base.forward * base.speed * deltaTime;
			if (height > 1f)
			{
				SetState(State.Flying);
			}
			break;
		case State.Flying:
			if (fuel < 0f)
			{
				base.speed = Utils.Lerp(base.speed, 0f, deltaTime * 2f);
				if (base.speed < 2f)
				{
					SetState(State.Falling);
				}
			}
			else
			{
				if (flyingArea.CheckBorderCollide(base.position))
				{
					fuel -= deltaTime + deltaTime * 3f * Mathf.Clamp01(base.forward.y);
				}
				else
				{
					fuel -= deltaTime;
				}
				base.speed = Utils.Lerp(base.speed, totalMaxSpeed, deltaTime * 2f);
				if (this.onFuelUpdated != null)
				{
					this.onFuelUpdated(this);
				}
			}
			if (direction != Vector3.zero)
			{
				Rotate(direction.y * -base.goTransform.right * totalRotateAngle * deltaTime, Space.World);
			}
			point += base.forward * base.speed * deltaTime;
			break;
		case State.Falling:
			base.speed = Utils.Lerp(base.speed, 4f, deltaTime);
			if (direction != Vector3.zero)
			{
				Rotate(direction.y * -base.goTransform.right * 30f * deltaTime, Space.World);
			}
			base.forward = Vector3.Slerp(base.forward, Vector3.down, 0.3f * deltaTime);
			point += base.forward * base.speed * deltaTime;
			break;
		case State.Landing:
			base.speed = Utils.Lerp(base.speed, 0f, deltaTime * 2f);
			point.y = surfaceHeight + 0.2f;
			point += base.forward * base.speed * deltaTime;
			base.forward = Vector3.Slerp(base.forward, (!(base.forward.x > 0f)) ? Vector3.left : Vector3.right, deltaTime);
			if (base.speed < 0.1f)
			{
				SetState(State.Landed);
			}
			break;
		}
		point.z = 1f * (float)layer;
		point = flyingArea.CheckPosition(point);
		base.position = flyingArea.CheckPosition(point);
	}

	private void LateUpdate()
	{
		surfaceHeight = surface.GetHeight(base.position);
		projectionPoint = new Vector3(base.position.x, surfaceHeight, base.position.z);
		height = base.position.y - surfaceHeight;
		if ((state != State.Flying && state != State.Falling) || !(height < 0.2f))
		{
			return;
		}
		if (Vector3.Angle(Vector3.down, base.forward) > landingAngle)
		{
			if (availabelLanding)
			{
				SetState(State.Landing);
			}
		}
		else if (this.onSmashed != null)
		{
			this.onSmashed(this);
		}
	}

	public override void Move(Vector3 vector)
	{
		if (state != State.Flying && state != State.Falling)
		{
			return;
		}
		direction = vector;
		if (this.onRitchUpdate != null)
		{
			this.onRitchUpdate(this, vector.y);
		}
		if (vector == Vector3.zero && base.normal.y < -0.1f)
		{
			base.rotation *= Quaternion.Euler(Vector3.forward * 180f);
			if (this.onRoll != null)
			{
				this.onRoll(this);
			}
		}
	}

	public void SetFuel(int value)
	{
		fuel = (fuelCapacity = value);
		if (this.onFuelUpdated != null)
		{
			this.onFuelUpdated(this);
		}
	}

	public void FillFuel()
	{
		SetFuel((int)fuelCapacity);
	}

	public void SetMobility(int value)
	{
		mobility = value;
	}

	public void Takeoff()
	{
		if (fuel > 0f)
		{
			if (Vector3.Dot(base.forward, Vector3.right) > 0f)
			{
				base.rotation = Quaternion.LookRotation(Quaternion.Euler(Vector3.forward * 15f) * Vector3.right, Vector3.up);
			}
			else
			{
				base.rotation = Quaternion.LookRotation(Quaternion.Euler(Vector3.back * 15f) * Vector3.left, Vector3.up);
			}
			direction = Vector3.zero;
			base.speed = 0f;
			height = 0f;
			SetState(State.Takeoff);
		}
	}

	private void SetState(State value)
	{
		state = value;
		if (this.onStateChanged != null)
		{
			this.onStateChanged(this, state);
		}
	}
}
