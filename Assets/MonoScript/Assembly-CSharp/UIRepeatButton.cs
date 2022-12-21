using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIRepeatButton : UIElement, UIEventListener
{
	public Vector2 size;

	public UIButtonView view;

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
			if (isDisabled != value)
			{
				if (view != null)
				{
					view.Disable(value);
				}
				isDisabled = value;
			}
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
	public event Action<UIRepeatButton> onPressed;

	[method: MethodImpl(32)]
	public event Action<UIRepeatButton> onReleased;

	private void OnDrawGizmos()
	{
		if (show)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(base.position, new Vector3(size.x, size.y, 0f));
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
			if (view != null)
			{
				view.Press(true);
			}
			break;
		case TouchPhase.Moved:
		case TouchPhase.Stationary:
			if (bound.Contains(touch.ray.origin))
			{
				if (view != null)
				{
					view.Press(true);
				}
				OnPressed();
			}
			else
			{
				if (view != null)
				{
					view.Press(false);
				}
				OnReleased();
			}
			break;
		case TouchPhase.Ended:
		case TouchPhase.Canceled:
			OnReleased();
			if (view != null)
			{
				view.Press(false);
			}
			break;
		}
		return true;
	}

	private void OnPressed()
	{
		if (this.onPressed != null)
		{
			this.onPressed(this);
		}
	}

	private void OnReleased()
	{
		if (this.onReleased != null)
		{
			this.onReleased(this);
		}
	}
}
