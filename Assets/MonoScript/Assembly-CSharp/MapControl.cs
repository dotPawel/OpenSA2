using UnityEngine;

public class MapControl : AObject
{
	public MapBorder mapBorder;

	public float editorSensitivity = 1f;

	public float iOSSensitivity = 0.1f;

	public float androidSensitivity = 0.05f;

	public float damping = 10f;

	public float minAcceleration = 1f;

	public float arrowVisibilityRange = 1f;

	private Vector3 impulse;

	private float timer;

	private bool kineticMotion;

	private Vector3 acceleration;

	private Vector3 velocity;

	private MapScreen mapScreen;

	private Vector3[] vectors = new Vector3[4]
	{
		Vector3.forward,
		Vector3.right,
		Vector3.back,
		Vector3.left
	};

	private bool[] arrowFlags;

	public float sensitivity
	{
		get
		{
			return androidSensitivity;
		}
	}

	private void Awake()
	{
		mapScreen = UIController.Find<MapScreen>();
		mapScreen.gameObjectController.onTouchBegan += OnGameObjectControllerTouchBegan;
		mapScreen.gameObjectController.onTouchMoved += OnGameObjectControllerTouchMoved;
		mapScreen.gameObjectController.onTouchEned += OnGameObjectControllerTouchEned;
		arrowFlags = new bool[vectors.Length];
	}

	private void OnGameObjectControllerTouchBegan(GameObject obj)
	{
		kineticMotion = false;
		acceleration = Vector3.zero;
	}

	private void OnGameObjectControllerTouchEned(GameObject obj)
	{
		if (acceleration.magnitude > minAcceleration)
		{
			kineticMotion = true;
		}
	}

	public void SetPosition(Vector3 point)
	{
		base.position = mapBorder.CheckPosition(point);
		UpdateScreenArrows();
	}

	private void OnGameObjectControllerTouchMoved(GameObject obj, Vector2 vector)
	{
		Vector3 vector2 = base.goTransform.TransformDirection(new Vector3(vector.x, 0f, vector.y)) * sensitivity;
		Vector3 point = base.position - vector2;
		acceleration = -vector2;
		base.position = mapBorder.CheckPosition(point);
		UpdateScreenArrows();
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (deltaTime > 0f)
		{
			acceleration = Vector3.SmoothDamp(acceleration, Vector3.zero, ref velocity, damping * deltaTime);
			if (kineticMotion)
			{
				Vector3 point = base.position + acceleration;
				base.position = mapBorder.CheckPosition(point);
				UpdateScreenArrows();
			}
		}
	}

	private void UpdateScreenArrows()
	{
		for (int i = 0; i < vectors.Length; i++)
		{
			Vector3 vector = base.position + vectors[i] * arrowVisibilityRange;
			arrowFlags[i] = mapBorder.CheckPosition(vector) == vector;
		}
		mapScreen.SetArrows(arrowFlags);
	}
}
