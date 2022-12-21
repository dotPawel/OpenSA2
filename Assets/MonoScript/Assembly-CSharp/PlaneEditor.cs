using UnityEngine;

public class PlaneEditor : MeshEditor
{
	public Vector2 sizeSource = new Vector2(64f, 64f);

	public Rect uvSource = new Rect(0f, 0f, 1f, 1f);

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

	public static PlaneEditor Init(GameObject gameObject, Vector2 size, Rect uv)
	{
		PlaneEditor planeEditor = gameObject.AddComponent<PlaneEditor>();
		planeEditor.sizeSource = size;
		planeEditor.uvSource = uv;
		return planeEditor;
	}

	private void UpdateUV(Rect arg)
	{
		uvSource = arg;
		if (base.sharedMesh.uv.Length == 4)
		{
			Vector2[] array = base.sharedMesh.uv;
			array[0].x = (array[1].x = arg.x);
			array[0].y = (array[3].y = 1f - (arg.y + arg.height));
			array[1].y = (array[2].y = 1f - arg.y);
			array[2].x = (array[3].x = arg.x + arg.width);
			base.sharedMesh.uv = array;
		}
	}

	private void UpdateSize(Vector2 arg)
	{
		sizeSource = arg;
		Vector3[] vertices = base.sharedMesh.vertices;
		if (vertices.Length == 4)
		{
			vertices[0].x = (vertices[1].x = (0f - arg.x) / 2f);
			vertices[2].x = (vertices[3].x = arg.x / 2f);
			vertices[0].y = (vertices[3].y = (0f - arg.y) / 2f);
			vertices[1].y = (vertices[2].y = arg.y / 2f);
			base.sharedMesh.vertices = vertices;
			base.sharedMesh.RecalculateBounds();
		}
	}

	protected override void RebuildMesh()
	{
		if (base.meshFilter.sharedMesh == null)
		{
			base.meshFilter.sharedMesh = MeshBuilder.CreatePlane();
			UpdateUV(uvSource);
			UpdateSize(sizeSource);
			UpdateAlpha(alphaSource);
		}
	}
}
