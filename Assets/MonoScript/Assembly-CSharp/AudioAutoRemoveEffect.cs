using UnityEngine;

public class AudioAutoRemoveEffect : Effect
{
	public AudioSource audioSource;

	public bool randomClip = true;

	public AudioClip[] clips;

	public bool randomPitch = true;

	public Interval pitch = new Interval(1f, 1.2f);

	public float delay;

	private int clipIndex;

	protected float timer;

	private void OnEnable()
	{
		timer = 0f;
		if (randomClip)
		{
			audioSource.clip = clips[clipIndex++ % clips.Length];
		}
		if (randomPitch)
		{
			audioSource.pitch = pitch.random;
		}
		audioSource.Play();
	}

	private void FixedUpdate()
	{
		if ((timer += Time.deltaTime) > delay)
		{
			Remove();
		}
	}
}
