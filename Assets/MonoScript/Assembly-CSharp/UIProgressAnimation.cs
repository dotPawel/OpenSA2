using UnityEngine;

public class UIProgressAnimation : UIAnimationClip
{
	public ProgressBar progressBar;

	private float time = 1f;

	private float startProgress;

	private float endProgress;

	private float timer;

	public static UIProgressAnimation Init(GameObject obj, ProgressBar progressBar, float time)
	{
		UIProgressAnimation uIProgressAnimation = obj.AddComponent<UIProgressAnimation>();
		uIProgressAnimation.progressBar = progressBar;
		uIProgressAnimation.time = time;
		return uIProgressAnimation;
	}

	public void Play(float start, float end)
	{
		startProgress = start;
		endProgress = end;
		progressBar.SetProgress(startProgress);
		timer = time;
	}

	public override bool UpdateAnimation(float deltaTime)
	{
		if (timer > 0f)
		{
			timer -= deltaTime;
			progressBar.SetProgress(Mathf.Lerp(startProgress, endProgress, 1f - timer / time));
			return true;
		}
		return false;
	}
}
