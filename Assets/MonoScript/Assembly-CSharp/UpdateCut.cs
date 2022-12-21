using UnityEngine;

public class UpdateCut
{
	private const int DELAY = 5;

	public int delay;

	private Renderer renderer;

	private int tick;

	public UpdateCut(Renderer renderer)
	{
		this.renderer = renderer;
		delay = 5;
		tick = 0;
	}

	public float Update(float deltaTime)
	{
		if (!renderer.isVisible)
		{
			if (tick++ % delay == 0)
			{
				return deltaTime * (float)delay;
			}
			return 0f;
		}
		return deltaTime;
	}
}
