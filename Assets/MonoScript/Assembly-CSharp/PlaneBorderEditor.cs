using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlaneBorderEditor : MeshEditor
{
	public Vector2 sizeSource = new Vector2(64f, 64f);

	public Vector2 borderSource = new Vector2(16f, 16f);

	public Rect uvSource = new Rect(0f, 0f, 1f, 1f);

	private int[][] BLOCKS
	{
		get
		{
			return MeshBuilder.BLOCKS;
		}
	}

	public override Rect uv
	{
		get
		{
			return uvSource;
		}
		set
		{
			UpdateUV(value);
		}
	}

	public override Vector2 size
	{
		get
		{
			return sizeSource;
		}
		set
		{
			UpdateSize(value);
		}
	}

	public Vector2 border
	{
		get
		{
			return borderSource;
		}
		set
		{
			UpdateBorder(value);
		}
	}

	private void UpdateUV(Rect arg)
	{
		uvSource = arg;
		if (base.sharedMesh.uv.Length == 16)
		{
			Vector2[] array = base.sharedMesh.uv;
			array[0].x = (array[1].x = (array[8].x = (array[9].x = arg.x)));
			array[2].x = (array[3].x = (array[10].x = (array[11].x = arg.x + arg.width / 3f)));
			array[4].x = (array[5].x = (array[12].x = (array[13].x = arg.x + arg.width * 2f / 3f)));
			array[6].x = (array[7].x = (array[14].x = (array[15].x = arg.x + arg.width)));
			array[8].y = (array[11].y = (array[13].y = (array[15].y = 1f - (arg.y + arg.height))));
			array[9].y = (array[10].y = (array[12].y = (array[14].y = 1f - (arg.y + arg.height * 2f / 3f))));
			array[0].y = (array[3].y = (array[5].y = (array[7].y = 1f - (arg.y + arg.height / 3f))));
			array[1].y = (array[2].y = (array[4].y = (array[6].y = 1f - arg.y)));
			base.sharedMesh.uv = array;
		}
	}

	private void UpdateSize(Vector2 arg)
	{
		sizeSource = arg;
		Vector3[] vertices = base.sharedMesh.vertices;
		if (vertices.Length == 16)
		{
			Vector2 zero = Vector2.zero;
			zero.x = (arg.x - border.x * 2f) / 2f;
			zero.x = Mathf.Clamp(zero.x, 0f, zero.x);
			zero.y = (arg.y - border.y * 2f) / 2f;
			zero.y = Mathf.Clamp(zero.y, 0f, zero.y);
			vertices[BLOCKS[0][0]].y = (vertices[BLOCKS[0][3]].y = (vertices[BLOCKS[2][0]].y = (vertices[BLOCKS[2][3]].y = 0f - zero.y - border.y)));
			vertices[BLOCKS[0][1]].y = (vertices[BLOCKS[0][2]].y = (vertices[BLOCKS[2][1]].y = (vertices[BLOCKS[2][2]].y = 0f - zero.y)));
			vertices[BLOCKS[6][0]].y = (vertices[BLOCKS[6][3]].y = (vertices[BLOCKS[8][0]].y = (vertices[BLOCKS[8][3]].y = zero.y)));
			vertices[BLOCKS[6][1]].y = (vertices[BLOCKS[6][2]].y = (vertices[BLOCKS[8][1]].y = (vertices[BLOCKS[8][2]].y = zero.y + border.y)));
			vertices[BLOCKS[0][0]].x = (vertices[BLOCKS[0][1]].x = (vertices[BLOCKS[6][0]].x = (vertices[BLOCKS[6][1]].x = 0f - zero.x - border.x)));
			vertices[BLOCKS[0][2]].x = (vertices[BLOCKS[0][3]].x = (vertices[BLOCKS[6][2]].x = (vertices[BLOCKS[6][3]].x = 0f - zero.x)));
			vertices[BLOCKS[2][0]].x = (vertices[BLOCKS[2][1]].x = (vertices[BLOCKS[8][0]].x = (vertices[BLOCKS[8][1]].x = zero.x)));
			vertices[BLOCKS[2][2]].x = (vertices[BLOCKS[2][3]].x = (vertices[BLOCKS[8][2]].x = (vertices[BLOCKS[8][3]].x = zero.x + border.x)));
			base.sharedMesh.vertices = vertices;
			base.sharedMesh.RecalculateBounds();
		}
	}

	private void UpdateBorder(Vector2 arg)
	{
		borderSource = arg;
		Vector3[] vertices = base.sharedMesh.vertices;
		if (vertices.Length == 16)
		{
			Vector2 zero = Vector2.zero;
			zero.x = Mathf.Clamp(arg.x, 0f, size.x / 2f);
			zero.y = Mathf.Clamp(arg.y, 0f, size.y / 2f);
			vertices[BLOCKS[0][1]].y = (vertices[BLOCKS[0][2]].y = (vertices[BLOCKS[2][1]].y = (vertices[BLOCKS[2][2]].y = vertices[BLOCKS[0][0]].y + zero.y)));
			vertices[BLOCKS[6][0]].y = (vertices[BLOCKS[6][3]].y = (vertices[BLOCKS[8][0]].y = (vertices[BLOCKS[8][3]].y = vertices[BLOCKS[6][1]].y - zero.y)));
			vertices[BLOCKS[0][3]].x = (vertices[BLOCKS[0][2]].x = (vertices[BLOCKS[6][3]].x = (vertices[BLOCKS[6][2]].x = vertices[BLOCKS[0][0]].x + zero.x)));
			vertices[BLOCKS[2][0]].x = (vertices[BLOCKS[2][1]].x = (vertices[BLOCKS[8][0]].x = (vertices[BLOCKS[8][1]].x = vertices[BLOCKS[2][3]].x - zero.x)));
			base.sharedMesh.vertices = vertices;
			base.sharedMesh.RecalculateBounds();
		}
	}

	protected override void RebuildMesh()
	{
		if (base.meshFilter.sharedMesh == null)
		{
			base.meshFilter.sharedMesh = CreatePlaneBorder();
			UpdateSize(sizeSource);
			UpdateBorder(borderSource);
			UpdateUV(uvSource);
		}
	}

	public static Mesh CreatePlaneBorder()
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
		mesh.triangles = new int[48]
		{
			0, 1, 2, 2, 3, 0, 3, 2, 4, 4,
			5, 3, 5, 4, 6, 6, 7, 5, 9, 0,
			3, 3, 10, 9, 12, 5, 7, 7, 14, 12,
			8, 9, 10, 10, 11, 8, 11, 10, 12, 12,
			13, 11, 13, 12, 14, 14, 15, 13
		};
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}
}
