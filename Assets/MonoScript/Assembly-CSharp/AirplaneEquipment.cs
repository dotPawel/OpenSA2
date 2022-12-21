using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class AirplaneEquipment : UILayer
{
	public List<UITabButton> tabButtons;

	private int equipmentIndex;

	[method: MethodImpl(32)]
	public event Action<AirplaneEquipment, int> onEquipmentChanged;

	private void Start()
	{
		for (int i = 0; i < tabButtons.Count; i++)
		{
			tabButtons[i].onClicked += OnTabClicked;
		}
	}

	private void OnTabClicked(UITabButton tabButton)
	{
		SetEquipment(tabButtons.IndexOf(tabButton));
	}

	public void SetEquipment(int value)
	{
		equipmentIndex = value;
		for (int i = 0; i < tabButtons.Count; i++)
		{
			tabButtons[i].selected = i == equipmentIndex;
		}
		if (this.onEquipmentChanged != null)
		{
			this.onEquipmentChanged(this, value);
		}
	}
}
