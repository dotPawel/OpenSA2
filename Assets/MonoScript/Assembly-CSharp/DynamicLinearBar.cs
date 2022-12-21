using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class DynamicLinearBar : ProgressBar
{
	public Vector2 borderSize = new Vector2(1.1f, 0.6f);

	public Vector2 barSize = new Vector2(1f, 0.5f);

	public int segments = 8;

	public Color borderColor = Color.black;

	public Color firstColor = Color.green;

	public Color secondColor = Color.red;

	private Color[] colorsCache;

	private MeshFilter meshFilterPointer;

	private MeshFilter meshFilter
	{
		get
		{
			if (meshFilterPointer == null)
			{
				meshFilterPointer = GetComponent<MeshFilter>();
			}
			return meshFilterPointer;
		}
	}

	private void Awake()
	{
		RebuildMesh();
	}

	public override void SetProgress(float progress)
	{
		Mesh mesh = meshFilter.mesh;
		int num = (int)((float)(segments + 1) * Mathf.Clamp01(1f - progress));
		if (colorsCache == null)
		{
			colorsCache = mesh.colors;
		}
		for (int i = 0; i < segments + 1; i++)
		{
			int num2 = i * 2;
			Color color = ((i >= num) ? firstColor : secondColor);
			colorsCache[num2] = color;
			colorsCache[num2 + 1] = color;
		}
		mesh.colors = colorsCache;
	}

	public void RebuildMesh()
	{
		CombineInstance[] array = new CombineInstance[2];
		array[0].mesh = CreateBarMesh(barSize, Vector3.zero, firstColor);
		array[0].transform = Matrix4x4.identity;
		array[1].mesh = CreateBorderMesh(borderSize, Vector3.back * 0.01f, borderColor);
		array[1].transform = Matrix4x4.identity;
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(array);
		meshFilter.mesh = mesh;
	}

	private Mesh CreateBarMesh(Vector2 size, Vector3 center, Color color)
	{
		Mesh mesh = new Mesh();
		int num = 2 + segments * 2;
		Vector3[] array = new Vector3[num];
		Color[] array2 = new Color[num];
		Vector2[] array3 = new Vector2[num];
		float num2 = size.x / (float)segments;
		float num3 = (0f - size.x) / 2f;
		float y = size.y / 2f;
		float y2 = (0f - size.y) / 2f;
		for (int i = 0; i < segments + 1; i++)
		{
			int num4 = i * 2;
			array[num4] = center + new Vector3(num3 + num2 * (float)i, y, 0f);
			array[num4 + 1] = center + new Vector3(num3 + num2 * (float)i, y2, 0f);
			array2[num4] = color;
			array2[num4 + 1] = color;
			float x = i % 2;
			array3[num4] = new Vector2(x, 1f);
			array3[num4 + 1] = new Vector2(x, 0f);
		}
		int num5 = segments * 2;
		int[] array4 = new int[num5 * 3];
		for (int j = 0; j < segments; j++)
		{
			int num6 = j * 2;
			int num7 = j * 6;
			array4[num7] = num6 % num;
			array4[num7 + 1] = (num6 + 1) % num;
			array4[num7 + 2] = (num6 + 2) % num;
			array4[num7 + 3] = (num6 + 1) % num;
			array4[num7 + 4] = (num6 + 3) % num;
			array4[num7 + 5] = (num6 + 2) % num;
		}
		mesh.vertices = array;
		mesh.uv = array3;
		mesh.colors = array2;
		mesh.triangles = array4;
		return mesh;
	}

	private Mesh CreateBorderMesh(Vector2 size, Vector3 center, Color color)
	{
		Mesh mesh = new Mesh();
		int num = 4;
		Vector3[] array = new Vector3[num];
		Color[] array2 = new Color[num];
		Vector2[] array3 = new Vector2[num];
		array[0] = center + new Vector3((0f - size.x) / 2f, size.y / 2f, 0f);
		array[1] = center + new Vector3((0f - size.x) / 2f, (0f - size.y) / 2f, 0f);
		array[2] = center + new Vector3(size.x / 2f, size.y / 2f, 0f);
		array[3] = center + new Vector3(size.x / 2f, (0f - size.y) / 2f, 0f);
		for (int i = 0; i < num; i++)
		{
			array3[i] = Vector2.zero;
			array2[i] = color;
		}
		int[] triangles = new int[6] { 0, 1, 2, 1, 3, 2 };
		mesh.vertices = array;
		mesh.uv = array3;
		mesh.colors = array2;
		mesh.triangles = triangles;
		return mesh;
	}
}
