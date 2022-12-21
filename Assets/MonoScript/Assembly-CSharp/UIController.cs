using UnityEngine;

public class UIController : MonoBehaviour
{
	public UIElement view;

	public UIController rootControler;

	public bool show
	{
		get
		{
			return view != null && view.show;
		}
		set
		{
			if (view != null)
			{
				view.show = value;
			}
		}
	}

	public static T Find<T>() where T : UIController
	{
		return Object.FindObjectOfType(typeof(T)) as T;
	}

	public virtual void ViewWillAppear()
	{
	}

	public virtual void ViewDidAppear()
	{
	}

	public virtual void ViewWillDisappear()
	{
	}

	public virtual void ViewDidDisappear()
	{
	}
}
