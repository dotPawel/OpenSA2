using UnityEngine;

public class QuadBez
{
	public Vector3 point0;

	public Vector3 point1;

	public Vector3 point2;

	public QuadBez()
		: this(Vector3.zero, Vector3.zero, Vector3.zero)
	{
	}

	public QuadBez(Vector3 point0, Vector3 point1, Vector3 point2)
	{
		Init(point0, point1, point2);
	}

	public void Init(Vector3 point0, Vector3 point1, Vector3 point2)
	{
		this.point0 = point0;
		this.point1 = point1;
		this.point2 = point2;
	}

	public Vector3 Interp(float t)
	{
		float num = 1f - t;
		return num * num * point0 + 2f * num * t * point1 + t * t * point2;
	}
}
