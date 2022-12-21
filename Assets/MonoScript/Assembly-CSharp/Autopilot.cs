using UnityEngine;

public class Autopilot : MonoBehaviour
{
	private const float DIRECTION_RATIO = 0.95f;

	private const float MIN_HEIGTH_RATE = 2f;

	public float minHeight = 0.5f;

	public float maxHeight = 7f;

	private Airplane airplane;

	public void SetAirplane(Airplane value)
	{
		airplane = value;
		base.enabled = airplane != null;
	}

	private void Start()
	{
		SetAirplane(airplane);
	}

	private void Update()
	{
		if (airplane != null)
		{
			airplane.Move(Vector3.up * Vector3.Dot(airplane.normal, (!(airplane.forward.x > 0f)) ? Vector3.left : Vector3.right));
		}
	}
}
