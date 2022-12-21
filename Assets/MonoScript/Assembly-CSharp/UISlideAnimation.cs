using UnityEngine;

public class UISlideAnimation : UIAnimationClip
{
	public UIIndicator indicator;

	private int stateIndex;

	public static UISlideAnimation Init(GameObject obj, UIIndicator indicator)
	{
		UISlideAnimation uISlideAnimation = obj.AddComponent<UISlideAnimation>();
		uISlideAnimation.indicator = indicator;
		return uISlideAnimation;
	}

	public void Play(int state)
	{
		stateIndex = state;
	}

	public override bool UpdateAnimation(float deltaTime)
	{
		if (indicator.state != stateIndex)
		{
			indicator.SetState(stateIndex);
		}
		return false;
	}
}
