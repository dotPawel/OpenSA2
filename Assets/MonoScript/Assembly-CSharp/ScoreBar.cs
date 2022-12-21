public class ScoreBar : UIContainer
{
	private const int NUMBER_COUNT = 5;

	public UIText textLabel;

	public UIIndicator iconIndicator;

	public int icon
	{
		set
		{
			iconIndicator.SetState(value);
		}
	}

	public int score
	{
		set
		{
			textLabel.text = StatisticsScreen.GetIntField(value, 5);
		}
	}
}
