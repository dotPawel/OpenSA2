public class BonusPointObjective : Objective
{
	public AirplaneSpawner airplaneSpawner;

	public int points;

	private Player player;

	private void Start()
	{
		airplaneSpawner.onAirplaneSpawned += OnAirplaneSpawned;
		player = Player.Find();
	}

	private void OnAirplaneSpawned(AirplaneSpawner airplaneSpawner, Airplane airplane)
	{
		airplane.onDestroyed += OnAirplaneDestroyed;
	}

	private void OnAirplaneDestroyed(Target target)
	{
		target.onDestroyed -= OnAirplaneDestroyed;
		player.scoreController.AddPoints(ScoreController.FragType.Airplane, points);
	}
}
