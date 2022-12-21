using UnityEngine;

public class AirplaneExplosion : AutoRemoveEffect
{
	public Transform scorchTransform;

	private Surface surfaceCache;

	private Surface surface
	{
		get
		{
			if (surfaceCache == null)
			{
				surfaceCache = Surface.Find();
			}
			return surfaceCache;
		}
	}

	private void OnEnable()
	{
		timer = 0f;
		Vector3 point = base.position;
		point.y = surface.GetHeight(point);
		scorchTransform.position = point;
		scorchTransform.rotation = Quaternion.identity;
	}
}
