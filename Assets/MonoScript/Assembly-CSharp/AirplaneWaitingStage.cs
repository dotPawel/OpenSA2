using UnityEngine;

public class AirplaneWaitingStage : TutorialStage
{
	private const float DELAY = 0.1f;

	public float time = 5f;

	private float timer;

	private bool activated;

	public override void Init(Tutorial tutorial)
	{
		Player player = Player.Find();
		activated = true;
		timer = 0f;
		player.Takeoff();
		HUD hUD = UIController.Find<HUD>();
		hUD.bombButton.gameObject.SetActiveRecursively(false);
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
