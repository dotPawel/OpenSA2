public class AirplaneParamsBar : UIContainer
{
	public UIParamBar damageBar;

	public UIParamBar armorBar;

	public UIParamBar mobilityBar;

	public int damage
	{
		set
		{
			damageBar.SetValue(value);
		}
	}

	public int armor
	{
		set
		{
			armorBar.SetValue(value);
		}
	}

	public int mobility
	{
		set
		{
			mobilityBar.SetValue(value);
		}
	}

	public void SetArmorAvaible(int value)
	{
		armorBar.SetAvaible(value);
	}

	public void SetDamageAvaible(int value)
	{
		damageBar.SetAvaible(value);
	}

	public void SetMobilityAvaible(int value)
	{
		mobilityBar.SetAvaible(value);
	}
}
