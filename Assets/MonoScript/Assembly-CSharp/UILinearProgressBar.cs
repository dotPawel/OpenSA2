using UnityEngine;

[RequireComponent(typeof(MutablePlaneEditor))]
public class UILinearProgressBar : UIProgressBar
{
	public float progressValue = 1f;

	private MutablePlaneEditor meshEditorSource;

	private MutablePlaneEditor meshEditor
	{
		get
		{
			if (meshEditorSource == null)
			{
				meshEditorSource = GetComponent<MutablePlaneEditor>();
			}
			return meshEditorSource;
		}
	}

	public override float progress
	{
		get
		{
			return progressValue;
		}
		set
		{
			progressValue = value;
			meshEditor.Cut(Mathf.Clamp01(value));
		}
	}

	public virtual float alpha
	{
		get
		{
			return meshEditor.alpha;
		}
		set
		{
			meshEditor.alpha = value;
		}
	}
}
