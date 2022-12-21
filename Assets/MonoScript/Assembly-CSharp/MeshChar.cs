using UnityEngine;

public class MeshChar
{
	public Vector3[] vertices { get; private set; }

	public Vector2[] uv { get; private set; }

	public int[] triangles { get; private set; }

	public float width { get; private set; }

	public float height { get; private set; }

	public Vector3 offset
	{
		set
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] += value;
			}
		}
	}

	public MeshChar(float size, float kerning, Rect uv)
	{
		width = size * kerning;
		height = size;
		float num = width / 2f;
		float num2 = height / 2f;
		vertices = new Vector3[4]
		{
			new Vector3(0f - num, 0f - num2, 0f),
			new Vector3(0f - num, num2, 0f),
			new Vector3(num, num2, 0f),
			new Vector3(num, 0f - num2, 0f)
		};
		float num3 = uv.width * kerning;
		float num4 = uv.height;
		float num5 = uv.x + (uv.width - num3) / 2f;
		this.uv = new Vector2[4]
		{
			new Vector2(num5, uv.y - num4),
			new Vector2(num5, uv.y),
			new Vector2(num5 + num3, uv.y),
			new Vector2(num5 + num3, uv.y - num4)
		};
		triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
	}
}
