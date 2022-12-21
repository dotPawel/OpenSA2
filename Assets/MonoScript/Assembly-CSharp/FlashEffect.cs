using UnityEngine;

public class FlashEffect : Effect
{
	public Renderer meshRenderer;

	public float fadeTime;

	public Interval scale;

	private Material material;

	private float timer;

	private void Start()
	{
		material = meshRenderer.material;
	}

	private void OnEnable()
	{
		SetAlpha(1f);
		SetScale(0f);
		timer = fadeTime;
	}

	private void Update()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			float num = Mathf.Clamp01(1f - timer / fadeTime);
			SetAlpha(1f - num);
			SetScale((!(num < 0.5f)) ? (1f - (num - 0.5f) / 0.5f) : (num / 0.5f));
			if (timer <= 0f)
			{
				Remove();
			}
		}
	}

	private void SetAlpha(float value)
	{
		if (material == null)
		{
			material = meshRenderer.material;
		}
		Color color = material.color;
		color.a = value;
		material.color = color;
	}

	private void SetScale(float value)
	{
		base.localScale = Vector3.one * scale.Lerp(value);
	}
}
