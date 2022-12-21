using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Truck : Unit
{
	public enum State
	{
		None = 0,
		Follow = 1,
		Waiting = 2
	}

	private const int TICK = 10;

	public LandMovement movement;

	public State state;

	public float stoppingDistance = 0.5f;

	public float waitingTime = 1f;

	public Effect exposionEffect;

	public AudioSource audioSource;

	public float audioFadeTime = 1f;

	private Queue<Vector3> path = new Queue<Vector3>();

	private Vector3 destinationPoint;

	private float timer;

	private float audioTimer;

	private float audioLastTime;

	[method: MethodImpl(32)]
	public event Action<Truck> onPathComplete;

	[method: MethodImpl(32)]
	public event Action<Truck> onDestinationPointUpdated;

	private void OnEnable()
	{
		RestoreHealth();
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
		switch (state)
		{
		case State.Waiting:
			if (!((timer += Time.deltaTime) >= waitingTime))
			{
				break;
			}
			if (path.Count > 0)
			{
				destinationPoint = path.Dequeue();
				state = State.Follow;
				if (this.onDestinationPointUpdated != null)
				{
					this.onDestinationPointUpdated(this);
				}
			}
			else
			{
				if (this.onPathComplete != null)
				{
					this.onPathComplete(this);
				}
				state = State.None;
			}
			break;
		case State.Follow:
			if (movement.state == LandMovement.State.Stop)
			{
				movement.Move(destinationPoint - base.position);
			}
			else if (Mathf.Abs(destinationPoint.x - base.position.x) < stoppingDistance)
			{
				movement.Stop();
				timer = 0f;
				state = State.Waiting;
			}
			break;
		}
	}

	public void SetPath(Vector3[] points)
	{
		path.Clear();
		for (int i = 0; i < points.Length; i++)
		{
			path.Enqueue(points[i]);
		}
		if (path.Count > 0)
		{
			destinationPoint = path.Dequeue();
			state = State.Follow;
			return;
		}
		if (this.onPathComplete != null)
		{
			this.onPathComplete(this);
		}
		state = State.None;
	}

	public override void Explode()
	{
		if (exposionEffect != null)
		{
			exposionEffect.Instantiate(base.position, base.rotation);
		}
		Destroy();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(base.aimPoint, new Vector3(0.5f, 0f, 0.5f));
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(base.position, destinationPoint);
	}
}
