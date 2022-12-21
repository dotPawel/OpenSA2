using System.Collections.Generic;
using UnityEngine;

public class UILayer : UIContainer, UIEventListener, UIEventSource
{
	public Vector2 size;

	public bool multitouch;

	public bool clear;

	private List<UIEventListener> listeners = new List<UIEventListener>();

	private UIEventListener[] touches = new UIEventListener[5];

	private Rect bound
	{
		get
		{
			Vector3 vector = base.position;
			return new Rect(vector.x - size.x / 2f, vector.y - size.y / 2f, size.x, size.y);
		}
	}

	private int maxTouchs
	{
		get
		{
			return (!multitouch) ? 1 : 5;
		}
	}

	public int order
	{
		get
		{
			return depth;
		}
	}

	private void OnDrawGizmos()
	{
		if (show)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(base.position, new Vector3(size.x, size.y, 0f));
		}
	}

	public void AddListener(UIEventListener listener)
	{
		if (!listeners.Contains(listener))
		{
			listeners.Add(listener);
			listeners.Sort(new UIOrderComparer());
		}
	}

	public void RemoveListener(UIEventListener listener)
	{
		listeners.Remove(listener);
	}

	public bool Contains(Ray ray)
	{
		return show && bound.Contains(ray.origin);
	}

	public bool SetEvent(UITouch touch)
	{
		int id = touch.id;
		if (id < maxTouchs)
		{
			if (touches[id] != null)
			{
				touches[id].SetEvent(touch);
				if (touch.phase == TouchPhase.Ended)
				{
					touches[id] = null;
				}
				return true;
			}
			foreach (UIEventListener listener in listeners)
			{
				if (listener != null && listener.Contains(touch.ray) && listener.SetEvent(touch))
				{
					touches[id] = listener;
					return true;
				}
			}
		}
		return !clear;
	}
}
