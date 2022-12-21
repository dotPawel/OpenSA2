using UnityEngine;

public class Flag : AObject
{
	public Transform[] points;

	public Vector3 vector = Vector3.up;

	public float speed = 1f;

	public float offset = 0.5f;

	public Vector3 rotationAngle;

	public Renderer meshRenderer;

	public Material material
	{
		set
		{
			meshRenderer.material = value;
		}
	}

	private void Start()
	{
		base.rotation = Quaternion.Euler(rotationAngle);
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < points.Length; i++)
		{
			points[i].position += vector * Mathf.Sin(Time.time * speed + (float)i * offset);
		}
	}
}
