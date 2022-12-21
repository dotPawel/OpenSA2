using UnityEngine;

public class WaitingStage : TutorialStage
{
	public float time = 5f;

	private float timer;

	private bool activated;

	public override void Init(Tutorial tutorial)
	{
		timer = 0f;
		activated = true;
		tutorial.SetTitle(titleIndex);
		tutorial.SetMark(null);
	}

	private void FixedUpdate()
	{
		if (activated && (timer += Time.deltaTime) > time)
		{
			activated = false;
			Complete();
		}
	}
}
