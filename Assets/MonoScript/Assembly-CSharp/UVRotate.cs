using System;
using UnityEngine;

public class UVRotate
{
	public static Vector2[] RotateUVArowndQuat(Vector2 point, Vector2[] uv, float angle, int startIndex, int endIndex)
	{
		Quaternion quaternion = Quaternion.Euler(Vector3.forward * angle);
		for (int i = 0; i < uv.Length; i++)
		{
			if (i >= startIndex && i <= endIndex)
			{
				uv[i] -= point;
				uv[i] = quaternion * new Vector3(uv[i].x, uv[i].y, 0f);
				uv[i] += point;
			}
		}
		return uv;
	}

	public static Vector2[] RotateUVArownd(Vector2 point, Vector2[] uv, float angle, int startIndex, int endIndex)
	{
		float num = Mathf.Sin((float)Math.PI / 180f * angle);
		float num2 = Mathf.Cos((float)Math.PI / 180f * angle);
		for (int i = 0; i < uv.Length; i++)
		{
			if (i >= startIndex && i <= endIndex)
			{
				uv[i] -= point;
				uv[i].x = uv[i].x * num2 - uv[i].y * num;
				uv[i].y = uv[i].x * num + uv[i].y * num2;
				uv[i] += point;
			}
		}
		return uv;
	}
}
