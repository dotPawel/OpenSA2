using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DynamicCircleBar : ProgressBar
{
	public Vector3 normalDirection = Vector3.up;

	public float radius = 1f;

	public float height = 0.1f;

	public int segments = 16;

	public Color firstColor = Color.white;

	public Color secondColor = Color.black;

	public float arcAngle = 360f;

	private Color[] colors;

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
		int num = (int)((float)segments * Mathf.Clamp01(1f - progress));
		for (int i = 0; i < segments; i++)
		{
			int num2 = i * 2;
			Color color = ((i >= num) ? firstColor : secondColor);
			colors[num2] = color;
			colors[num2 + 1] = color;
		}
		mesh.colors = colors;
	}

	[ContextMenu("Rebuild Mesh")]
	public void RebuildMesh()
	{
		Mesh mesh = new Mesh();
		int num = segments * 2;
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num];
		colors = new Color[num];
		for (int i = 0; i < segments; i++)
		{
			float num2 = arcAngle / (float)segments * (float)i;
			Quaternion quaternion = Quaternion.Euler(normalDirection * num2);
			int num3 = i * 2;
			array[num3] = quaternion * (Vector3.forward * radius);
			array[num3 + 1] = quaternion * (Vector3.forward * (radius - height));
			colors[num3] = firstColor;
			colors[num3 + 1] = firstColor;
			float x = i % 2;
			array2[num3] = new Vector2(x, 1f);
			array2[num3 + 1] = new Vector2(x, 0f);
		}
		int num4 = segments * 2;
		int[] array3 = new int[num4 * 3];
		for (int j = 0; j < segments; j++)
		{
			int num5 = j * 2;
			int num6 = j * 6;
			array3[num6] = num5 % num;
			array3[num6 + 1] = (num5 + 2) % num;
			array3[num6 + 2] = (num5 + 1) % num;
			array3[num6 + 3] = (num5 + 1) % num;
			array3[num6 + 4] = (num5 + 2) % num;
			array3[num6 + 5] = (num5 + 3) % num;
		}
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.colors = colors;
		mesh.triangles = array3;
		meshFilter.mesh = mesh;
	}
}
