using UnityEngine;

public struct UITouch
{
	public int id;

	public Ray ray;

	public TouchPhase phase;

	public Vector2 screenPosition;

	public Vector2 deltaPosition;

	public UITouch(int id, Ray ray, TouchPhase phase, Vector2 screenPosition, Vector2 deltaPosition)
	{
		this.id = id;
		this.ray = ray;
		this.phase = phase;
		this.screenPosition = screenPosition;
		this.deltaPosition = deltaPosition;
	}

	public UITouch(Ray ray, Touch touch)
		: this(touch.fingerId, ray, touch.phase, touch.position, touch.deltaPosition)
	{
	}

	public UITouch(int id, Ray ray, TouchPhase phase)
		: this(id, ray, phase, Vector2.zero, Vector2.zero)
	{
	}
}
