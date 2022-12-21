using UnityEngine;

public class AirplaneStatusBar : UIContainer
{
	public UIProgressBar armorBar;

	public UIProgressBar fuelBar;

	public UIProgressBar ammoBar;

	public AlertIndicator armorAlert;

	public AlertIndicator fuelAlert;

	public AlertIndicator ammoAlert;

	public UIParamBar bombBar;

	public float blinkDelay = 0.25f;

	public float alertValue = 0.25f;

	public AudioSource audioSource;

	private float blinkTimer;

	private bool blinkOn;

	public float armorRate
	{
		set
		{
			armorBar.progress = value;
			armorAlert.blink = value <= alertValue;
		}
	}

	public float fuelRate
	{
		set
		{
			fuelBar.progress = value;
			fuelAlert.blink = value <= alertValue;
		}
	}

	public float ammoRate
	{
		set
		{
			ammoBar.progress = value;
			ammoAlert.blink = value <= alertValue;
		}
	}

	public int bombCount
	{
		set
		{
			bombBar.SetValue(value);
		}
	}

	private void FixedUpdate()
	{
		if (show && (blinkTimer -= Time.deltaTime) < 0f)
		{
			blinkTimer = blinkDelay;
			blinkOn = !blinkOn;
			bool flag = false;
			flag = armorAlert.UpdateState(blinkOn) || flag;
			flag = fuelAlert.UpdateState(blinkOn) || flag;
			if (ammoAlert.UpdateState(blinkOn) || flag)
			{
				audioSource.Play();
			}
		}
	}
}
