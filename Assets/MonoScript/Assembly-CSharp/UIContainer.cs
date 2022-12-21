using UnityEngine;

public class UIContainer : UIElement
{
	public override bool show
	{
		get
		{
			return base.show;
		}
		set
		{
			base.show = value;
			foreach (Transform item in base.uiTransform)
			{
				UIElement component = item.GetComponent<UIElement>();
				if (component != null)
				{
					component.show = value;
				}
			}
		}
	}
}
