using UnityEngine;

public class UIIndicatorAnimation : MonoBehaviour
{
	public UIIndicator indicator;

	public float animationDelay = 0.25f;

	public int stateCount = 4;

	private float timer;

	private int state;

	private float deltaTime = 1f / 30f;

	private void Update()
	{
		if (indicator.show && (timer += deltaTime) > animationDelay)
		{
			timer = 0f;
			indicator.SetState(state++ % stateCount);
		}
	}
}
