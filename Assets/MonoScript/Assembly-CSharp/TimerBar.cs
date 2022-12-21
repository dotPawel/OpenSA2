using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TimerBar : UIContainer
{
	private const string FORMAT = "{0}:{1}";

	public UIText timeLabel;

	private float timer;

	private int previousTime;

	[method: MethodImpl(32)]
	public event Action<TimerBar> onTimerFinished;

	private void FixedUpdate()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			int num = (int)timer;
			if (num != previousTime)
			{
				previousTime = num;
				int num2 = num / 60;
				int value = num - num2 * 60;
				timeLabel.text = string.Format("{0}:{1}", StatisticsScreen.GetIntField(num2, 2), StatisticsScreen.GetIntField(value, 2));
			}
			if (timer <= 0f && this.onTimerFinished != null)
			{
				this.onTimerFinished(this);
			}
		}
	}

	public void SetTime(float value)
	{
		timer = value;
	}
}
