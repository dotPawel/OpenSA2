using UnityEngine;

public class FlyingArea : AObject
{
	public float vertical;

	public float horizon;

	private static FlyingArea flyingAreaReference;

	public static FlyingArea Find()
	{
		if (flyingAreaReference == null)
		{
			flyingAreaReference = Object.FindObjectOfType(typeof(FlyingArea)) as FlyingArea;
		}
		return flyingAreaReference;
	}

	public Vector3 CheckPosition(Vector3 point)
	{
		Vector3 result = point;
		float num = horizon / 2f;
		result.x = Mathf.Clamp(point.x, 0f - num, num);
		result.y = Mathf.Clamp(point.y, 0f - vertical, vertical);
		return result;
	}

	public bool CheckBorderCollide(Vector3 point)
	{
		return point.y == vertical;
	}

	public float GetHeightRate(Vector3 point)
	{
		return Mathf.Clamp01(point.y / vertical);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(base.position + Vector3.up * vertical / 2f, new Vector3(horizon, vertical));
	}
}
