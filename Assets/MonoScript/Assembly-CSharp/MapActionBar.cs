using UnityEngine;

public class MapActionBar : UILayer
{
	private const int SCORE_NUMBER_COUNT = 6;

	public UIButton closeButton;

	public PlaneNineEditor background;

	public Camera uiCamera;

	public UIText scoreLabel;

	public int score
	{
		set
		{
			scoreLabel.text = StatisticsScreen.GetIntField(value, 6);
		}
	}

	private void Start()
	{
		float num = (float)Screen.width / (float)Screen.height;
		background.size = new Vector2(uiCamera.orthographicSize * 2f * num, background.size.y);
	}
}
