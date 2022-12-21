using UnityEngine;

public class AutoRemoveEffect : Effect
{
	public float delay;

	protected float timer;

	private void OnEnable()
	{
		timer = 0f;
	}

	private void FixedUpdate()
	{
		if ((timer += Time.deltaTime) > delay)
		{
			Remove();
		}
	}
}
