using UnityEngine;

public class Movement : AObject
{
	protected const float LAYER_SIZE = 1f;

	public float maxSpeed;

	public int layer;

	public float speed { get; protected set; }

	public virtual void Move(Vector3 vector)
	{
	}

	public virtual void Stop()
	{
	}

	public virtual void SetLayer(int value)
	{
		layer = value;
	}
}
