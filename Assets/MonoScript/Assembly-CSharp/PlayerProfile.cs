public class PlayerProfile : UIController
{
	private const int SCORE_NUMBER_COUNT = 6;

	public UIIndicator ranks;

	public UIText scoreLabel;

	public UIIndicator theme;

	public int rank
	{
		set
		{
			ranks.SetState(value);
		}
	}

	public int score
	{
		set
		{
			scoreLabel.text = StatisticsScreen.GetIntField(value, 6);
		}
	}

	public void SetTheme(int index)
	{
		theme.SetState(index);
	}
}
