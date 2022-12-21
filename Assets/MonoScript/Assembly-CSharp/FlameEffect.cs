using UnityEngine;

public class FlameEffect : AObject
{
	private const float VOLUME_FADE_TIME = 0.1f;

	public MeshFilter meshFilter;

	public MeshRenderer meshRenderer;

	public float fadeTime = 0.14f;

	public float audioFadeTime = 0.5f;

	public AudioClip[] clips;

	public AudioSource audioSource;

	private Mesh mesh;

	private Color[] colors;

	private float timer;

	private float audioTimer;

	private float audioLastTime;

	private void Start()
	{
		mesh = meshFilter.mesh;
		colors = mesh.colors;
	}

	private void OnEnable()
	{
		meshRenderer.enabled = false;
	}

	private void Update()
	{
		float num = Time.deltaTime;
		if (num == 0f)
		{
			num = Time.realtimeSinceStartup - audioLastTime;
		}
		if (timer > 0f)
		{
			timer -= num;
			float num2 = Mathf.Clamp01(timer / fadeTime);
			SetAlpha(num2);
			SetScale(num2);
			if (num2 == 0f)
			{
				meshRenderer.enabled = false;
			}
		}
		if (audioTimer > 0f)
		{
			audioTimer -= num;
			audioSource.volume = Mathf.Clamp01(audioTimer / 0.1f);
			if (audioTimer <= 0f)
			{
				audioSource.Stop();
			}
		}
		audioLastTime = Time.realtimeSinceStartup;
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			audioLastTime = Time.realtimeSinceStartup;
		}
	}

	public void Play()
	{
		if (audioSource != null)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
			audioTimer = audioFadeTime;
		}
		SetAlpha(1f);
		SetScale(1f);
		meshRenderer.enabled = true;
		timer = fadeTime;
	}

	public void SetSoundClip(int index)
	{
		if (audioSource != null)
		{
			audioSource.clip = clips[index];
		}
	}

	private void SetAlpha(float value)
	{
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i].a = value;
		}
		mesh.colors = colors;
	}

	private void SetScale(float value)
	{
		base.localScale = Utils.Vector3Lerp(Vector3.zero, Vector3.one, value);
	}
}
