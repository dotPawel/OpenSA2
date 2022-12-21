using UnityEngine;

public class MapBorder : AObject
{
	public Vector2 size;

	private float widthBorder;

	private float heightBorder;

	private void Awake()
	{
		Camera mainCamera = Camera.main;
		float orthographicSize = mainCamera.orthographicSize;
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = orthographicSize * num;
		heightBorder = Mathf.Clamp(size.y / 2f - orthographicSize, 0f, size.y / 2f);
		widthBorder = Mathf.Clamp(size.x / 2f - num2, 0f, size.x / 2f);
	}

	public Vector3 CheckPosition(Vector3 point)
	{
		Vector3 vector = base.position;
		Vector3 result = point;
		result.x = Mathf.Clamp(point.x, vector.x - widthBorder, vector.x + widthBorder);
		result.z = Mathf.Clamp(point.z, vector.z - heightBorder, vector.z + heightBorder);
		return result;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(base.position, new Vector3(size.x, 0f, size.y));
	}
}
