using UnityEngine;

public class CloudParticle : AObject
{
	public Renderer meshRenderer;

	public MeshFilter meshFilter;

	public float speed;

	public Mesh mesh
	{
		set
		{
			meshFilter.mesh = value;
		}
	}

	public Material material
	{
		set
		{
			meshRenderer.material = value;
		}
	}

	public static CloudParticle Create(Vector3 position, Quaternion rotation, Transform parent)
	{
		GameObject gameObject = new GameObject("Cloud");
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		CloudParticle cloudParticle = gameObject.AddComponent<CloudParticle>();
		cloudParticle.meshRenderer = meshRenderer;
		cloudParticle.meshFilter = meshFilter;
		cloudParticle.parent = parent;
		cloudParticle.position = position;
		cloudParticle.rotation = rotation;
		return cloudParticle;
	}

	public void Show(bool value)
	{
		base.gameObject.active = value;
	}
}
