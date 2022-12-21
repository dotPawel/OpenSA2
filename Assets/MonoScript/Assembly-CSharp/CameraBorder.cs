using UnityEngine;

public class CameraBorder : AObject
{
	public FlyingArea flyingArea;

	private float widthBorder;

	private float heightBorder;

	private void Start()
	{
	}

	public Vector3 CheckPosition(Vector3 point)
	{
		return point;
	}

	private void OnDrawGizmosSelected()
	{
	}
}
