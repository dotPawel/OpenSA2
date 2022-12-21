using UnityEngine;

public class LandMovement : Movement
{
	public enum State
	{
		Moving = 0,
		Stop = 1
	}

	private const float STOPPING_SPEED = 0.05f;

	public bool pitch;

	public float acceleration;

	public State state;

	private Surface surface;

	private void OnEnable()
	{
		state = State.Stop;
		base.speed = 0f;
		Vector3 vector = base.position;
		vector.z = 1f * (float)layer;
		base.position = vector;
	}

	private void Start()
	{
		surface = Surface.Find();
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		Vector3 vector = base.position;
		switch (state)
		{
		case State.Moving:
			if (acceleration > 0f)
			{
				base.speed = Utils.Lerp(base.speed, maxSpeed, deltaTime * acceleration);
			}
			else
			{
				base.speed = maxSpeed;
			}
			vector += base.forward * base.speed * Time.deltaTime;
			vector.y = surface.GetHeight(vector);
			vector.z = 1f * (float)layer;
			if (pitch && base.position != vector)
			{
				base.rotation = Quaternion.LookRotation(vector - base.position, Vector3.up);
			}
			base.position = vector;
			break;
		case State.Stop:
			if (base.speed > 0.05f)
			{
				if (acceleration > 0f)
				{
					base.speed = Utils.Lerp(base.speed, 0f, deltaTime * acceleration);
				}
				else
				{
					base.speed = 0f;
				}
				vector += base.forward * base.speed * Time.deltaTime;
				vector.y = surface.GetHeight(vector);
				vector.z = 1f * (float)layer;
				if (pitch && base.position != vector)
				{
					base.rotation = Quaternion.LookRotation(vector - base.position, Vector3.up);
				}
				base.position = vector;
			}
			break;
		}
	}

	public override void Move(Vector3 vector)
	{
		state = State.Moving;
		base.rotation = Quaternion.LookRotation((!(Vector3.Dot(vector.normalized, Vector3.right) > 0f)) ? Vector3.left : Vector3.right, Vector3.up);
	}

	public override void Stop()
	{
		state = State.Stop;
	}
}
