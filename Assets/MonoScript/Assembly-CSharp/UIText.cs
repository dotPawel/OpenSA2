using UnityEngine;

[RequireComponent(typeof(UITextRenderer))]
public class UIText : UIElement
{
	private UITextRenderer textRendererSource;

	private UITextRenderer textRenderer
	{
		get
		{
			if (textRendererSource == null)
			{
				textRendererSource = GetComponent<UITextRenderer>();
			}
			return textRendererSource;
		}
	}

	public string text
	{
		get
		{
			return textRenderer.text;
		}
		set
		{
			textRenderer.text = value;
		}
	}

	public float alpha
	{
		get
		{
			return textRenderer.alpha;
		}
		set
		{
			textRenderer.alpha = value;
		}
	}

	public Color color
	{
		get
		{
			return textRenderer.color;
		}
		set
		{
			textRenderer.color = value;
		}
	}
}
