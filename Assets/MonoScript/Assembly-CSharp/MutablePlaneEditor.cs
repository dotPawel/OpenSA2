using UnityEngine;

public class MutablePlaneEditor : MeshEditor
{
	public enum Direction
	{
		Rigth = 0,
		Down = 1,
		Left = 2,
		Up = 3
	}

	public enum UVAnchor
	{
		Left = 0,
		Right = 1
	}

	public Rect uvSource = new Rect(0f, 0f, 1f, 1f);

	public Vector2 sizeSource = new Vector2(64f, 64f);

	public float ratio;

	public UVAnchor uvAnchor;

	public Direction direction;

	private Vector2[] uvs;

	private Vector3[] vertices;

	public override Rect uv
	{
		get
		{
			return uvSource;
		}
		set
		{
			uvSource = value;
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
			sizeSource = value;
			UpdateSize(value);
		}
	}

	public void Cut(float ratio)
	{
		this.ratio = ratio;
		Rect arg = uvSource;
		Vector2 arg2 = sizeSource;
		if (uvAnchor == UVAnchor.Right)
		{
			arg.x = arg.x + arg.width - arg.width * ratio;
		}
		arg.width *= ratio;
		arg2.x *= ratio;
		UpdateUV(arg);
		UpdateSize(arg2);
	}

	private void UpdateUV(Rect arg)
	{
		if (uvs == null || uvs.Length != 4)
		{
			uvs = base.sharedMesh.uv;
		}
		if (uvs.Length == 4)
		{
			uvs[0].x = (uvs[1].x = arg.x);
			uvs[0].y = (uvs[3].y = 1f - (arg.y + arg.height));
			uvs[1].y = (uvs[2].y = 1f - arg.y);
			uvs[2].x = (uvs[3].x = arg.x + arg.width);
			base.sharedMesh.uv = uvs;
		}
	}

	private void UpdateSize(Vector2 arg)
	{
		if (vertices == null || vertices.Length != 4)
		{
			vertices = base.sharedMesh.vertices;
		}
		Vector2 vector = arg / 2f;
		if (vertices.Length == 4)
		{
			vertices[0].x = (vertices[1].x = 0f);
			vertices[2].x = (vertices[3].x = arg.x);
			vertices[0].y = (vertices[3].y = 0f - vector.y);
			vertices[1].y = (vertices[2].y = vector.y);
			base.sharedMesh.vertices = vertices;
		}
	}

	protected override void RebuildMesh()
	{
		if (base.meshFilter.sharedMesh == null)
		{
			base.meshFilter.sharedMesh = MeshBuilder.CreatePlane();
			uv = uvSource;
			size = sizeSource;
		}
	}
}
