using UnityEngine;

public class MeshBuilder
{
	public static int[][] BLOCKS = new int[9][]
	{
		new int[4] { 8, 9, 10, 11 },
		new int[4] { 11, 10, 12, 13 },
		new int[4] { 13, 12, 14, 15 },
		new int[4] { 9, 0, 3, 10 },
		new int[4] { 10, 3, 5, 12 },
		new int[4] { 12, 5, 7, 14 },
		new int[4] { 0, 1, 2, 3 },
		new int[4] { 3, 2, 4, 5 },
		new int[4] { 5, 4, 6, 7 }
	};

	public static Mesh CreatePlane(Vector2 size, Vector4 uv)
	{
		Mesh mesh = new Mesh();
		Vector2 vector = size / 2f;
		mesh.vertices = new Vector3[4]
		{
			new Vector3(0f - vector.x, 0f - vector.y, 0f),
			new Vector3(0f - vector.x, vector.y, 0f),
			new Vector3(vector.x, vector.y, 0f),
			new Vector3(vector.x, 0f - vector.y, 0f)
		};
		mesh.colors = new Color[4]
		{
			Color.white,
			Color.white,
			Color.white,
			Color.white
		};
		mesh.uv = new Vector2[4]
		{
			new Vector2(uv.x, uv.y),
			new Vector2(uv.x, uv.w),
			new Vector2(uv.z, uv.w),
			new Vector2(uv.z, uv.y)
		};
		mesh.triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public static Mesh CreatePlane()
	{
		return CreatePlane(new Vector2(1f, 1f), new Vector4(0f, 0f, 1f, 1f));
	}

	public static Mesh CreatePlaneNine()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[16]
		{
			new Vector3(-0.3f, 0.1f, 0f),
			new Vector3(-0.3f, 0.3f, 0f),
			new Vector3(-0.1f, 0.3f, 0f),
			new Vector3(-0.1f, 0.1f, 0f),
			new Vector3(0.1f, 0.3f, 0f),
			new Vector3(0.1f, 0.1f, 0f),
			new Vector3(0.3f, 0.3f, 0f),
			new Vector3(0.3f, 0.1f, 0f),
			new Vector3(-0.3f, -0.3f, 0f),
			new Vector3(-0.3f, -0.1f, 0f),
			new Vector3(-0.1f, -0.1f, 0f),
			new Vector3(-0.1f, -0.3f, 0f),
			new Vector3(0.1f, -0.1f, 0f),
			new Vector3(0.1f, -0.3f, 0f),
			new Vector3(0.3f, -0.1f, 0f),
			new Vector3(0.3f, -0.3f, 0f)
		};
		mesh.uv = new Vector2[16]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(0f, 1f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(0f, 0f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 0f)
		};
		Color[] array = new Color[mesh.vertices.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Color.white;
		}
		mesh.colors = array;
		mesh.triangles = new int[54]
		{
			0, 1, 2, 2, 3, 0, 3, 2, 4, 4,
			5, 3, 5, 4, 6, 6, 7, 5, 9, 0,
			3, 3, 10, 9, 10, 3, 5, 5, 12, 10,
			12, 5, 7, 7, 14, 12, 8, 9, 10, 10,
			11, 8, 11, 10, 12, 12, 13, 11, 13, 12,
			14, 14, 15, 13
		};
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}
}
