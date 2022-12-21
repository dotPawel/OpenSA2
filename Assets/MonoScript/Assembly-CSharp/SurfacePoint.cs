using System.Collections.Generic;
using UnityEngine;

public class SurfacePoint : AObject
{
	public class PositionComparer : Comparer<SurfacePoint>
	{
		public override int Compare(SurfacePoint x, SurfacePoint y)
		{
			float num = ApplyFilter(x.position, Vector3.right);
			float num2 = ApplyFilter(y.position, Vector3.right);
			return (num != num2) ? ((!(num < num2)) ? 1 : (-1)) : 0;
		}

		private float ApplyFilter(Vector3 vector, Vector3 filter)
		{
			for (int i = 0; i < 3; i++)
			{
				if (filter[i] > 0f)
				{
					return vector[i];
				}
			}
			return 0f;
		}
	}

	public class DistanceComparer : Comparer<SurfacePoint>
	{
		public Vector3 point;

		public DistanceComparer(Vector3 point)
		{
			this.point = point;
		}

		public override int Compare(SurfacePoint x, SurfacePoint y)
		{
			float sqrMagnitude = (x.position - point).sqrMagnitude;
			float sqrMagnitude2 = (y.position - point).sqrMagnitude;
			return (sqrMagnitude != sqrMagnitude2) ? ((!(sqrMagnitude < sqrMagnitude2)) ? 1 : (-1)) : 0;
		}
	}

	public void SetHeight(float value)
	{
		Vector3 vector = base.position;
		vector.y = value;
		base.position = vector;
	}

	public float GetHeight()
	{
		return base.position.y;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.position, 0.25f);
	}
}
