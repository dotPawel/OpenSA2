using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
	public MeshRenderer meshRenderer;

	public float animationSpeed = 1f;

	public float waveRatio = 0.5f;

	private Material material;

	private void Start()
	{
		material = meshRenderer.material;
	}

	private void FixedUpdate()
	{
		float time = Time.time;
		material.SetFloat("_Blend", Mathf.PingPong(time * animationSpeed, 1f));
		Vector2 offset = Vector2.one * waveRatio * Mathf.Sin(time * animationSpeed);
		material.SetTextureOffset("_MainTex1", offset);
		material.SetTextureOffset("_MainTex2", offset);
	}
}
