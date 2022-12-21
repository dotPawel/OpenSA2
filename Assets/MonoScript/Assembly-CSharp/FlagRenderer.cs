using UnityEngine;

public class FlagRenderer : MonoBehaviour
{
	public float scale = 0.05f;

	public float speed = 4f;

	public float step = 0.4f;

	private MeshRenderer meshRenderer;

	private Vector3[] baseHeight;

	private Mesh mesh;

	private float time;

	private void Awake()
	{
		meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		mesh = component.mesh;
		baseHeight = mesh.vertices;
	}

	private void FixedUpdate()
	{
		if (meshRenderer.isVisible)
		{
			Vector3[] array = new Vector3[baseHeight.Length];
			time += Time.deltaTime;
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector = baseHeight[i];
				vector.z += Mathf.Sin(time * speed + (float)i * step) * scale;
				array[i] = vector;
			}
			mesh.vertices = array;
		}
	}
}
