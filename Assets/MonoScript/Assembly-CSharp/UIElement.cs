using UnityEngine;

public class UIElement : MonoBehaviour
{
	private const float RATIO = 100f;

	public const int UI_LAYER = 30;

	public const int INVISIBLE_LAYER = 31;

	private Transform goTransform;

	protected Transform uiTransform
	{
		get
		{
			return (!(goTransform != null)) ? (goTransform = base.transform) : goTransform;
		}
	}

	public Transform parent
	{
		get
		{
			return uiTransform.parent;
		}
		set
		{
			Unregister();
			uiTransform.parent = value;
			Register();
		}
	}

	public Vector3 localPosition
	{
		get
		{
			return uiTransform.localPosition;
		}
		set
		{
			uiTransform.localPosition = value;
		}
	}

	public Quaternion localRotation
	{
		get
		{
			return uiTransform.localRotation;
		}
		set
		{
			uiTransform.localRotation = value;
		}
	}

	public Vector3 position
	{
		get
		{
			return uiTransform.position;
		}
		set
		{
			uiTransform.position = value;
		}
	}

	public Quaternion rotation
	{
		get
		{
			return uiTransform.rotation;
		}
		set
		{
			uiTransform.rotation = value;
		}
	}

	public Vector3 scale
	{
		get
		{
			return uiTransform.localScale;
		}
		set
		{
			uiTransform.localScale = value;
		}
	}

	public virtual bool show
	{
		get
		{
			return base.gameObject.layer != 31;
		}
		set
		{
			base.gameObject.layer = ((!value) ? 31 : 30);
		}
	}

	public virtual int depth
	{
		get
		{
			return Mathf.RoundToInt(localPosition.z * 100f);
		}
		set
		{
			localPosition = new Vector3(localPosition.x, localPosition.y, (float)value / 100f);
		}
	}

	private void OnEnable()
	{
		Register();
	}

	private void OnDisable()
	{
		Unregister();
	}

	public void Register()
	{
		UIEventListener uIEventListener = this as UIEventListener;
		if (uIEventListener != null && parent != null)
		{
			UIEventSource uIEventSource = (UIEventSource)parent.gameObject.GetComponent(typeof(UIEventSource));
			if (uIEventSource != null)
			{
				uIEventSource.AddListener(uIEventListener);
			}
		}
	}

	public void Unregister()
	{
		UIEventListener uIEventListener = this as UIEventListener;
		if (uIEventListener != null && parent != null)
		{
			UIEventSource uIEventSource = (UIEventSource)parent.gameObject.GetComponent(typeof(UIEventSource));
			if (uIEventSource != null)
			{
				uIEventSource.RemoveListener(uIEventListener);
			}
		}
	}

	public void Destroy()
	{
		if (base.gameObject != null)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public UIElement Instantiate(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(base.gameObject, position, rotation);
		return gameObject.GetComponent<UIElement>();
	}
}
