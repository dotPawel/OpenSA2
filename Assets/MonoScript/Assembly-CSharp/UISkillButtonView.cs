public class UISkillButtonView : UIButtonView
{
	public UIText titleLabel;

	public UIText textLabel;

	public UIText descriptionLabel;

	public UIIndicator icons;

	public UIView selectedView;

	public UIElement[] elements;

	public bool isSelected;

	public bool isVisible;

	public string title
	{
		set
		{
			titleLabel.text = value;
		}
	}

	public string text
	{
		set
		{
			textLabel.text = value;
		}
	}

	public string description
	{
		set
		{
			descriptionLabel.text = value;
		}
	}

	public override void Show(bool value)
	{
		isVisible = value;
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i].show = value;
		}
		selectedView.show = isVisible && isSelected;
	}

	public override void Select(bool value)
	{
		isSelected = value;
		selectedView.show = isVisible && isSelected;
	}

	public void SelectIcon(int value)
	{
		icons.SetState(value);
	}
}
