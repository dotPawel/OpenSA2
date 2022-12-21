using UnityEngine;

public class Utils
{
	public static bool IsDistanceLow(Vector3 point0, Vector3 point1, float distance)
	{
		return (point0 - point1).sqrMagnitude < distance * distance;
	}

	public static bool IsDistanceHigh(Vector3 point0, Vector3 point1, float distance)
	{
		return (point0 - point1).sqrMagnitude > distance * distance;
	}

	public static bool IsXRangeLow(Vector3 point0, Vector3 point1, float range)
	{
		return Mathf.Abs(point0.x - point1.x) < range;
	}

	public static bool IsRangeLow(Vector3 point0, Vector3 point1, float range)
	{
		point0.z = 0f;
		point1.z = 0f;
		return (point0 - point1).sqrMagnitude < range * range;
	}

	public static bool IsRangeHigh(Vector3 point0, Vector3 point1, float range)
	{
		point0.z = 0f;
		point1.z = 0f;
		return (point0 - point1).sqrMagnitude > range * range;
	}

	public static int RoundToInt(float arg)
	{
		return (int)(arg + 0.5f);
	}

	public static Vector3 Vector3Lerp(Vector3 vector1, Vector3 vector2, float rate)
	{
		if (rate > 1f)
		{
			return vector2;
		}
		if (rate < 0f)
		{
			return vector1;
		}
		return new Vector3(vector1.x + (vector2.x - vector1.x) * rate, vector1.y + (vector2.y - vector1.y) * rate, vector1.z + (vector2.z - vector1.z) * rate);
	}

	public static Vector2 Vector2Lerp(Vector2 vector1, Vector2 vector2, float rate)
	{
		if (rate > 1f)
		{
			return vector2;
		}
		if (rate < 0f)
		{
			return vector1;
		}
		return new Vector2(vector1.x + (vector2.x - vector1.x) * rate, vector1.y + (vector2.y - vector1.y) * rate);
	}

	public static float Lerp(float from, float to, float rate)
	{
		if (rate < 0f)
		{
			return from;
		}
		if (rate > 1f)
		{
			return to;
		}
		return (to - from) * rate + from;
	}

	public static float NoiseFilter(float value, float ratio)
	{
		return (float)(int)(value * ratio) / ratio;
	}

	public static Vector3 NoiseFilter(Vector3 vector, float ratio)
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < 3; i++)
		{
			zero[i] = (float)(int)(vector[i] * ratio) / ratio;
		}
		return zero;
	}

	public static Vector3 CutWithMask(Vector3 vector, Vector3 mask)
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < 3; i++)
		{
			zero[i] = (((int)mask[i] != 0) ? 0f : vector[i]);
		}
		return zero;
	}

	public static Vector3 GetPointOnRay(Ray ray, float height)
	{
		Vector3 origin = ray.origin;
		Vector3 direction = ray.direction;
		float x = (height - origin.y) * direction.x / direction.y + origin.x;
		float z = (height - origin.y) * direction.z / direction.y + origin.z;
		return new Vector3(x, height, z);
	}
}
