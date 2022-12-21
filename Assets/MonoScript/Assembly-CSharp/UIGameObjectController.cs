using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIGameObjectController : UIElement, UIEventListener
{
	[Serializable]
	public class GOTouch
	{
		public Vector2 screenPoint;

		public GameObject obj;

		public int tapCount;

		public float timer;
	}

	private const float MIN_DELTA = 30f;

	private const float DELAY = 0.5f;

	public Vector2 size;

	public float distance = 50f;

	public LayerMask layerMask;

	public GOTouch[] touches;

	private Camera gameCamera;

	private Rect bound
	{
		get
		{
			Vector3 vector = base.position;
			return new Rect(vector.x - size.x / 2f, vector.y - size.y / 2f, size.x, size.y);
		}
	}

	private int touchCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < touches.Length; i++)
			{
				if (touches[i].screenPoint != Vector2.zero)
				{
					num++;
				}
			}
			return num;
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
	public event Action<GameObject> onTouchBegan;

	[method: MethodImpl(32)]
	public event Action<GameObject, Vector2> onTouchMoved;

	[method: MethodImpl(32)]
	public event Action<GameObject> onTouchEned;

	[method: MethodImpl(32)]
	public event Action<float> onZoom;

	[method: MethodImpl(32)]
	public event Action<GameObject, Vector3> onGameObjectTouch;

	[method: MethodImpl(32)]
	public event Action<GameObject, Vector3> onGameObjectMove;

	[method: MethodImpl(32)]
	public event Action<GameObject, Vector3> onGameObjectTap;

	[method: MethodImpl(32)]
	public event Action<GameObject, Vector3> onGameObjectDoubleTap;

	[method: MethodImpl(32)]
	public event Action<GameObject, GameObject, Vector3> onGameObjectSwipe;

	[method: MethodImpl(32)]
	public event Action<GameObject> onGameObjectCancel;

	private void Start()
	{
		gameCamera = Camera.main;
	}

	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < touches.Length; i++)
		{
			GOTouch gOTouch = touches[i];
			if (gOTouch.timer > 0f)
			{
				gOTouch.timer -= deltaTime;
				if (gOTouch.timer <= 0f)
				{
					gOTouch.tapCount = 0;
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (show)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(base.position, new Vector3(size.x, size.y, 0f));
		}
	}

	public bool Contains(Ray ray)
	{
		return show && bound.Contains(ray.origin);
	}

	public bool SetEvent(UITouch touch)
	{
		if (touch.id > touches.Length - 1)
		{
			return true;
		}
		GOTouch gOTouch = touches[touch.id];
		RaycastHit hitInfo;
		switch (touch.phase)
		{
		case TouchPhase.Began:
		{
			gOTouch.screenPoint = touch.screenPosition;
			if (touchCount > 1)
			{
				CancelGameObjects();
				break;
			}
			Ray ray = gameCamera.ScreenPointToRay(touch.screenPosition);
			if ((gOTouch.obj != null && gOTouch.tapCount > 0 && gOTouch.obj.GetComponent<Collider>().Raycast(ray, out hitInfo, distance)) || Physics.Raycast(ray, out hitInfo, distance, layerMask.value))
			{
				GameObject gameObject = hitInfo.collider.gameObject;
				if (gameObject == gOTouch.obj)
				{
					gOTouch.tapCount++;
				}
				else
				{
					gOTouch.obj = gameObject;
					gOTouch.tapCount = 1;
				}
				gOTouch.timer = 0.5f;
				OnGameObjectTouch(gameObject, hitInfo.point);
				OnTouchBegan(gOTouch.obj);
			}
			else
			{
				gOTouch.obj = null;
			}
			break;
		}
		case TouchPhase.Moved:
			if (touchCount > 1)
			{
				Vector2 zero = Vector2.zero;
				for (int i = 0; i < touches.Length; i++)
				{
					zero += touches[i].screenPoint;
				}
				zero /= (float)touches.Length;
				float num = Mathf.Sign(Vector2.Dot(touch.screenPosition - zero, touch.deltaPosition));
				float magnitude = touch.deltaPosition.magnitude;
				OnZoom(magnitude * num);
			}
			else if (gOTouch.obj != null)
			{
				Ray ray = gameCamera.ScreenPointToRay(touch.screenPosition);
				Vector3 pointOnRay = Utils.GetPointOnRay(ray, gOTouch.obj.transform.position.y);
				OnGameObjectMove(gOTouch.obj, pointOnRay);
				OnTouchMoved(gOTouch.obj, touch.deltaPosition);
			}
			break;
		case TouchPhase.Ended:
			if (gOTouch.obj != null)
			{
				Ray ray = gameCamera.ScreenPointToRay(touch.screenPosition);
				Vector3 pointOnRay2 = Utils.GetPointOnRay(ray, gOTouch.obj.transform.position.y);
				if ((touch.screenPosition - gOTouch.screenPoint).magnitude < 30f)
				{
					if (gOTouch.tapCount > 1)
					{
						OnGameObjectDoubleTap(gOTouch.obj, pointOnRay2);
					}
					else
					{
						OnGameObjectTap(gOTouch.obj, pointOnRay2);
					}
				}
				else if (Physics.Raycast(ray, out hitInfo, distance, layerMask.value))
				{
					OnGameObjectSwipe(gOTouch.obj, hitInfo.collider.gameObject, hitInfo.point);
				}
				else
				{
					OnGameObjectSwipe(gOTouch.obj, null, pointOnRay2);
				}
				OnTouchEnded(gOTouch.obj);
			}
			gOTouch.screenPoint = Vector2.zero;
			break;
		case TouchPhase.Canceled:
			if (gOTouch.obj != null)
			{
				OnGameObjectCancel(gOTouch.obj);
				gOTouch.obj = null;
			}
			gOTouch.screenPoint = Vector2.zero;
			break;
		}
		return true;
	}

	private void OnGameObjectTouch(GameObject obj, Vector3 point)
	{
		if (this.onGameObjectTouch != null)
		{
			this.onGameObjectTouch(obj, point);
		}
	}

	private void OnGameObjectTap(GameObject obj, Vector3 point)
	{
		if (this.onGameObjectTap != null)
		{
			this.onGameObjectTap(obj, point);
		}
	}

	private void OnGameObjectMove(GameObject obj, Vector3 point)
	{
		if (this.onGameObjectMove != null)
		{
			this.onGameObjectMove(obj, point);
		}
	}

	private void OnGameObjectSwipe(GameObject obj, GameObject target, Vector3 point)
	{
		if (this.onGameObjectSwipe != null)
		{
			this.onGameObjectSwipe(obj, target, point);
		}
	}

	private void OnTouchBegan(GameObject obj)
	{
		if (this.onTouchBegan != null)
		{
			this.onTouchBegan(obj);
		}
	}

	private void OnTouchMoved(GameObject obj, Vector2 delta)
	{
		if (this.onTouchMoved != null)
		{
			this.onTouchMoved(obj, delta);
		}
	}

	private void OnTouchEnded(GameObject obj)
	{
		if (this.onTouchEned != null)
		{
			this.onTouchEned(obj);
		}
	}

	private void OnGameObjectDoubleTap(GameObject obj, Vector3 point)
	{
		if (this.onGameObjectDoubleTap != null)
		{
			this.onGameObjectDoubleTap(obj, point);
		}
	}

	private void OnGameObjectCancel(GameObject obj)
	{
		if (this.onGameObjectCancel != null)
		{
			this.onGameObjectCancel(obj);
		}
	}

	private void OnZoom(float delta)
	{
		if (this.onZoom != null)
		{
			this.onZoom(delta);
		}
	}

	private void CancelGameObjects()
	{
		for (int i = 0; i < touches.Length; i++)
		{
			if (touches[i].obj != null)
			{
				OnGameObjectCancel(touches[i].obj);
				touches[i].obj = null;
			}
		}
	}
}
