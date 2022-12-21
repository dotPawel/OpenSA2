public class LandStage : TutorialStage
{
	private const float AIRPORT_RANGE = 3f;

	public Structure airport;

	public override void Init(Tutorial tutorial)
	{
		Player player = Player.Find();
		player.airplane.movement.onStateChanged += OnAirplaneMovementStateChanged;
		tutorial.SetMark(airport);
		tutorial.SetTitle(titleIndex);
		tutorial.sessionComplete = true;
	}

	private void OnAirplaneMovementStateChanged(AirMovement movement, AirMovement.State state)
	{
		if (state == AirMovement.State.Landed && Utils.IsXRangeLow(movement.position, airport.position, 3f))
		{
			Complete();
		}
	}
}
