public class MainMenuScenario : Scenario
{
	public ScenarioSide sideA;

	public ScenarioSide sideB;

	public Faction factionA;

	public Faction factionB;

	private void Start()
	{
		sideA.SetFaction(factionA);
		sideB.SetFaction(factionB);
		StartScenario();
		SoundManager soundManager = SoundManager.Find();
		if (soundManager != null && !soundManager.mainTheme)
		{
			soundManager.FadeIn();
		}
	}
}
