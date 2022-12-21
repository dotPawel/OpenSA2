using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, UIEventSource
{
	public const int TOUCHES = 5;

	public const float SIZE = 320f;

	public Camera uiCamera;

	public bool multitouch;

	private List<UIEventListener> listeners = new List<UIEventListener>();

	private UIEventListener[] touches = new UIEventListener[5];

	public static UIManager current
	{
		get
		{
			return Object.FindObjectOfType(typeof(UIManager)) as UIManager;
		}
	}

	private int maxTouchs
	{
		get
		{
			return (!multitouch) ? 1 : 5;
		}
	}

	public static UIManager Create()
	{
		GameObject gameObject = new GameObject("UIManager");
		Camera camera = gameObject.AddComponent<Camera>();
		camera.orthographic = true;
		camera.orthographicSize = 320f;
		camera.nearClipPlane = 0f;
		camera.farClipPlane = 10f;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.cullingMask = 1073741824;
		UIManager uIManager = gameObject.AddComponent<UIManager>();
		uIManager.uiCamera = camera;
		Camera main = Camera.main;
		uIManager.transform.position = main.transform.position;
		uIManager.transform.parent = main.transform;
		int cullingMask = main.cullingMask;
		cullingMask ^= 0x40000000;
		cullingMask ^= int.MinValue;
		main.cullingMask = cullingMask;
		return uIManager;
	}

	private void Update()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			CheckTouches(Input.touches);
		}
		else
		{
			CheckMouse();
		}
	}

	private void CheckMouse()
	{
		int num = 0;
		if (Input.GetMouseButtonDown(num))
		{
			Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
			CheckTouch(new UITouch(deltaPosition: new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), id: num, ray: ray, phase: TouchPhase.Began, screenPosition: Input.mousePosition));
		}
		if (Input.GetMouseButton(num))
		{
			Ray ray2 = uiCamera.ScreenPointToRay(Input.mousePosition);
			Vector2 deltaPosition2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			CheckTouch(new UITouch(num, ray2, (deltaPosition2.magnitude > 0f) ? TouchPhase.Moved : TouchPhase.Stationary, Input.mousePosition, deltaPosition2));
		}
		if (Input.GetMouseButtonUp(num))
		{
			Ray ray3 = uiCamera.ScreenPointToRay(Input.mousePosition);
			CheckTouch(new UITouch(deltaPosition: new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), id: num, ray: ray3, phase: TouchPhase.Ended, screenPosition: Input.mousePosition));
		}
	}

	private void CheckTouches(Touch[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			Touch touch = array[i];
			Ray ray = uiCamera.ScreenPointToRay(touch.position);
			CheckTouch(new UITouch(ray, touch));
		}
	}

	private void CheckTouch(UITouch touch)
	{
		int id = touch.id;
		if (id >= maxTouchs)
		{
			return;
		}
		if (touches[id] != null)
		{
			touches[id].SetEvent(touch);
			if (touch.phase == TouchPhase.Ended)
			{
				touches[id] = null;
			}
			return;
		}
		for (int i = 0; i < listeners.Count; i++)
		{
			UIEventListener uIEventListener = listeners[i];
			if (uIEventListener != null && uIEventListener.Contains(touch.ray) && uIEventListener.SetEvent(touch))
			{
				touches[id] = uIEventListener;
				break;
			}
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
}
