public class UITabButtonView : UIButtonView
{
	public UIView selected;

	public UIElement[] elements;

	private bool isSelected;

	private bool isVisible;

	public override void Show(bool value)
	{
		isVisible = value;
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i].show = value;
		}
		selected.show = isVisible && isSelected;
	}

	public override void Select(bool value)
	{
		isSelected = value;
		selected.show = isVisible && isSelected;
	}
}
