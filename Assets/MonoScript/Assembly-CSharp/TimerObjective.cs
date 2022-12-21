public class TimerObjective : Objective
{
	public float time;

	public override void Init(Scenario scenario)
	{
		HUD hUD = UIController.Find<HUD>();
		hUD.timerBar.onTimerFinished += OnTimerFinished;
		hUD.SetTimer(time);
	}

	private void OnTimerFinished(TimerBar timerBar)
	{
		Complete();
	}
}
