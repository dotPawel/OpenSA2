using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIButton : UIElement, UIEventListener
{
	public UIButtonView view;

	public Vector2 size;

	private bool isFocused;

	private bool isDisabled;

	public override bool show
	{
		get
		{
			return base.show;
		}
		set
		{
			base.show = value;
			if (view != null)
			{
				view.Show(value);
			}
		}
	}

	public bool disable
	{
		get
		{
			return isDisabled;
		}
		set
		{
			if (view != null)
			{
				view.Disable(value);
			}
			isDisabled = value;
		}
	}

	private Rect bound
	{
		get
		{
			Vector3 vector = base.position;
			return new Rect(vector.x - size.x / 2f, vector.y - size.y / 2f, size.x, size.y);
		}
	}

	public int order
	{
		get
		{
			return depth;
		}
	}

	[method: MethodImpl(32)]
	public event Action<UIButton> onFocused;

	[method: MethodImpl(32)]
	public event Action<UIButton> onClicked;

	private void OnDrawGizmos()
	{
		if (show)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(base.position, new Vector3(size.x, size.y, 0f));
		}
	}

	private void OnClicked()
	{
		if (this.onClicked != null)
		{
			this.onClicked(this);
		}
	}

	private void OnFocused()
	{
		if (this.onFocused != null)
		{
			this.onFocused(this);
		}
	}

	public bool Contains(Ray ray)
	{
		return !isDisabled && show && bound.Contains(ray.origin);
	}

	public bool SetEvent(UITouch touch)
	{
		switch (touch.phase)
		{
		case TouchPhase.Began:
			isFocused = true;
			if (view != null)
			{
				view.Press(true);
			}
			OnFocused();
			break;
		case TouchPhase.Moved:
			if (isFocused && view != null)
			{
				view.Press(bound.Contains(touch.ray.origin));
			}
			break;
		case TouchPhase.Ended:
			if (isFocused)
			{
				if (view != null)
				{
					view.Press(false);
				}
				if (bound.Contains(touch.ray.origin))
				{
					OnClicked();
				}
				isFocused = false;
			}
			break;
		case TouchPhase.Canceled:
			if (isFocused)
			{
				if (view != null)
				{
					view.Press(false);
				}
				isFocused = false;
			}
			break;
		}
		return true;
	}
}
