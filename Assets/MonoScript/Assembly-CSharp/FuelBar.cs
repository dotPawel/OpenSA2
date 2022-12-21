using UnityEngine;

public class FuelBar : UIContainer
{
	public ProgressBar progressBar;

	private float timer;

	private float time;

	private void Update()
	{
		if (show && timer > 0f)
		{
			timer -= Time.deltaTime;
			progressBar.SetProgress(1f - timer / time);
		}
	}

	public void SetFillAnimation(float time)
	{
		this.time = (timer = time);
		progressBar.SetProgress(0f);
	}
}
