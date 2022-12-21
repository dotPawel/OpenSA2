using UnityEngine;

public class UIEquipmentButtonView : UIButtonView
{
	public UIView selectedView;

	public AObject model;

	public UIElement[] elements;

	public Vector3 rotateAngle;

	private bool isSelected;

	private bool isVisible;

	public override void Show(bool value)
	{
		isVisible = value;
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i].show = value;
		}
		selectedView.show = isVisible && isSelected;
		model.show = isVisible;
	}

	public override void Select(bool value)
	{
		isSelected = value;
		selectedView.show = isVisible && isSelected;
		model.localRotation = Quaternion.identity;
	}

	private void FixedUpdate()
	{
		if (isSelected)
		{
			model.Rotate(rotateAngle * Time.deltaTime, Space.Self);
		}
	}
}
