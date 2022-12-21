using System;
using UnityEngine;

public class DynamicCamera : PlayerListener
{
	public Transform topPoint;

	public Transform bottomPoint;

	public Transform cameraTarnsform;

	public Transform audioListener;

	public float depth = 1f;

	public float airplaneBound = 1f;

	private Airplane airplane;

	private FlyingArea flyingArea;

	private float angleRatio;

	private float offset;

	private void Start()
	{
		Camera mainCamera = Camera.main;
		float num = mainCamera.fieldOfView * mainCamera.aspect;
		offset = airplaneBound / 2f * mainCamera.aspect;
		angleRatio = Mathf.Tan(num / 2f * ((float)Math.PI / 180f));
		flyingArea = FlyingArea.Find();
	}

	private void LateUpdate()
	{
		if (airplane != null)
		{
			Vector3 vector = base.position;
			Vector3 point = airplane.position;
			vector.x = point.x;
			base.position = CheckPosition(vector);
			cameraTarnsform.position = Vector3.Slerp(bottomPoint.position, topPoint.position, flyingArea.GetHeightRate(point));
			Vector3 vector2 = vector;
			vector2.y = point.y;
			audioListener.position = vector2;
		}
	}

	public Vector3 CheckPosition(Vector3 point)
	{
		float num = flyingArea.horizon / 2f - (Mathf.Abs(depth - cameraTarnsform.position.z) * angleRatio - offset);
		point.x = Mathf.Clamp(point.x, 0f - num, num);
		return point;
	}

	public void SetPosition(Vector3 point)
	{
		base.position = new Vector3(point.x, base.position.y, base.position.z);
	}

	public override void OnAirplaneSet(Airplane airplane)
	{
		this.airplane = airplane;
	}

	private void OnDrawGizmosSelected()
	{
		if (bottomPoint != null && topPoint != null)
		{
			Gizmos.color = Color.yellow;
			for (int i = 0; i < 10; i++)
			{
				Vector3 center = Vector3.Slerp(bottomPoint.position, topPoint.position, 0.1f * (float)i);
				Gizmos.DrawWireSphere(center, 0.05f);
			}
		}
	}
}
