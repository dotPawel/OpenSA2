using UnityEngine;

public class UICampaignButtonView : UIButtonView
{
	public UIView normal;

	public UIView pressed;

	public UIIndicator ranks;

	public UIText label;

	public UIElement[] elements;

	private bool showProgress;

	private bool isVisible;

	public int rank
	{
		set
		{
			ranks.SetState(value);
		}
	}

	public float progress
	{
		set
		{
			showProgress = value > 0f;
			label.text = string.Format("{0}%", StatisticsScreen.GetIntField(Mathf.RoundToInt(value * 100f), 2));
			ranks.show = isVisible && showProgress;
			label.show = isVisible && showProgress;
		}
	}

	public override void Show(bool value)
	{
		isVisible = value;
		normal.show = isVisible;
		pressed.show = false;
		ranks.show = isVisible && showProgress;
		label.show = isVisible && showProgress;
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i].show = isVisible;
		}
	}

	public override void Press(bool value)
	{
		normal.show = !value;
		pressed.show = value;
	}
}
