using UnityEngine;

public class StructureExplosion : Effect
{
	public GameObject[] particles;

	public float[] times;

	public Interval delay;

	public float removeDelay = 3f;

	protected float timer;

	private void OnEnable()
	{
		timer = 0f;
	}

	private void FixedUpdate()
	{
		if (!(timer < removeDelay))
		{
			return;
		}
		timer += Time.deltaTime;
		for (int i = 0; i < particles.Length; i++)
		{
			GameObject gameObject = particles[i];
			if (gameObject.active && times[i] < timer)
			{
				gameObject.active = false;
			}
		}
	}

	[ContextMenu("Generate remove times")]
	public void GenerateRemoveTimes()
	{
		times = new float[particles.Length];
		for (int i = 0; i < times.Length; i++)
		{
			times[i] = delay.random;
		}
	}
}
