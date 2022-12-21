using UnityEngine;

public class Curve3D
{
	public Vector3[] points { get; private set; }

	public float length
	{
		get
		{
			float num = 0f;
			if (points != null)
			{
				for (int i = 0; i < points.Length - 1; i++)
				{
					num += Vector3.Distance(points[i], points[i + 1]);
				}
			}
			return num;
		}
	}

	public Curve3D()
		: this(new Vector3[0])
	{
	}

	public Curve3D(Vector3[] points)
	{
		this.points = points;
	}

	public void Update(Vector3[] points)
	{
		this.points = points;
	}

	public Vector3 Interp(float t)
	{
		int num = ((points != null) ? points.Length : 0);
		if (num == 1)
		{
			return points[0];
		}
		if (num == 2)
		{
			return Utils.Vector3Lerp(points[0], points[1], t);
		}
		if (num == 3)
		{
			float num2 = 1f - t;
			return num2 * num2 * points[0] + 2f * num2 * t * points[1] + t * t * points[2];
		}
		if (num > 3)
		{
			int num3 = num - 3;
			int num4 = Mathf.Min(Mathf.FloorToInt(t * (float)num3), num3 - 1);
			float num5 = t * (float)num3 - (float)num4;
			Vector3 vector = points[num4];
			Vector3 vector2 = points[num4 + 1];
			Vector3 vector3 = points[num4 + 2];
			Vector3 vector4 = points[num4 + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num5 * num5 * num5) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num5 * num5) + (-vector + vector3) * num5 + 2f * vector2);
		}
		return Vector3.zero;
	}

	public Vector3 Velocity(float t)
	{
		int num = ((points != null) ? points.Length : 0);
		if (num == 1)
		{
			return points[0];
		}
		if (num == 2)
		{
			return Utils.Vector3Lerp(points[0], points[1], t);
		}
		if (num == 3)
		{
			return (2f * points[0] - 4f * points[1] + 2f * points[2]) * t + 2f * points[1] - 2f * points[0];
		}
		if (num > 3)
		{
			int num2 = num - 3;
			int num3 = Mathf.Min(Mathf.FloorToInt(t * (float)num2), num2 - 1);
			float num4 = t * (float)num2 - (float)num3;
			Vector3 vector = points[num3];
			Vector3 vector2 = points[num3 + 1];
			Vector3 vector3 = points[num3 + 2];
			Vector3 vector4 = points[num3 + 3];
			return 1.5f * (-vector + 3f * vector2 - 3f * vector3 + vector4) * (num4 * num4) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * num4 + 0.5f * vector3 - 0.5f * vector;
		}
		return Vector3.zero;
	}

	public float GetLengthWithAccuracy(float accuracy)
	{
		float num = 0f;
		float num2 = 1f / accuracy;
		Vector3 b = Interp(0f);
		for (int i = 1; (float)i < num2; i++)
		{
			Vector3 vector = Interp((float)i * accuracy);
			num += Vector3.Distance(vector, b);
			b = vector;
		}
		return num;
	}
}
