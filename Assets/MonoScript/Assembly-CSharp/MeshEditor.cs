using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshEditor : MonoBehaviour
{
	public float alphaSource = 1f;

	private MeshFilter meshFilterPointer;

	protected MeshFilter meshFilter
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

	protected Mesh sharedMesh
	{
		get
		{
			if (meshFilter.sharedMesh == null)
			{
				RebuildMesh();
			}
			return meshFilter.sharedMesh;
		}
		set
		{
			meshFilter.sharedMesh = value;
		}
	}

	public virtual Rect uv { get; set; }

	public virtual Vector2 size { get; set; }

	public virtual float alpha
	{
		get
		{
			return alphaSource;
		}
		set
		{
			if (alphaSource != value)
			{
				UpdateAlpha(value);
			}
		}
	}

	private void OnEnable()
	{
		sharedMesh = null;
		RebuildMesh();
	}

	private void OnDrawGizmos()
	{
		RebuildMesh();
	}

	protected virtual void UpdateAlpha(float arg)
	{
		alphaSource = arg;
		if (meshFilter.sharedMesh != null)
		{
			Color[] colors = meshFilter.sharedMesh.colors;
			for (int i = 0; i < colors.Length; i++)
			{
				colors[i].a = arg;
			}
			meshFilter.sharedMesh.colors = colors;
		}
	}

	protected virtual void RebuildMesh()
	{
	}
}
