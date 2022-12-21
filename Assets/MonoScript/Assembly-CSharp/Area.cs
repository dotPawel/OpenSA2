using UnityEngine;

public class Area : AObject
{
	private const float RADUIS = 0.2f;

	public float radius = 1f;

	public Vector3 point
	{
		get
		{
			Vector2 vector = Random.insideUnitCircle * Random.Range(0f - radius, radius);
			return base.position + new Vector3(vector.x, 0f, vector.y);
		}
	}
}
