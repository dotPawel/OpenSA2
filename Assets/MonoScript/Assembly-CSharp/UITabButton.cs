using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UITabButton : UIElement, UIEventListener
{
	public UIButtonView view;

	public Vector2 size;

	private bool isSelected;

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

	public bool selected
	{
		get
		{
			return isSelected;
		}
		set
		{
			if (view != null)
			{
				view.Select(value);
			}
			isSelected = value;
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
	public event Action<UITabButton> onClicked;

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

	public bool Contains(Ray ray)
	{
		return !isDisabled && !isSelected && show && bound.Contains(ray.origin);
	}

	public bool SetEvent(UITouch touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (view != null)
			{
				view.Press(true);
			}
			OnClicked();
		}
		return true;
	}
}
