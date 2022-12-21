public class TargetsStatistics : UIContainer
{
	private const int NUMBER_COUNT = 4;

	public UIText infantryLabel;

	public UIText tankLabel;

	public UIText airplaneLabel;

	public UIText structureLabel;

	public int infantryCount
	{
		set
		{
			infantryLabel.text = StatisticsScreen.GetIntField(value, 4);
		}
	}

	public int tankCount
	{
		set
		{
			tankLabel.text = StatisticsScreen.GetIntField(value, 4);
		}
	}

	public int airplaneCount
	{
		set
		{
			airplaneLabel.text = StatisticsScreen.GetIntField(value, 4);
		}
	}

	public int structureCount
	{
		set
		{
			structureLabel.text = StatisticsScreen.GetIntField(value, 4);
		}
	}
}
