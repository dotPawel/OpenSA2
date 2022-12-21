using System;
using System.Collections.Generic;
using UnityEngine;

public class Surface : AObject
{
	[Serializable]
	public class SurfacePointJoint
	{
		public SurfacePoint pointA;

		public SurfacePoint pointB;

		public void Join()
		{
			pointA.position = pointB.position;
		}
	}

	private const int MODIFY_BONE_COUNT = 3;

	public SurfacePoint[] points;

	public SurfacePointJoint[] joints;

	public float maxHeight = 1f;

	public float minHeight = -0.5f;

	public float randomHeight = 0.25f;

	public float length = 120f;

	public float border = 60f;

	public bool generateHeightAtAwake;

	public Renderer[] surfaceRenderers;

	private Curve3D surfaceCurve;

	private static Surface surfaceReference;

	private readonly string[] settingsPath = new string[3] { "Settings/SurfaceA", "Settings/SurfaceB", "Settings/SurfaceC" };

	public static Surface Find()
	{
		if (surfaceReference == null)
		{
			surfaceReference = UnityEngine.Object.FindObjectOfType(typeof(Surface)) as Surface;
		}
		return surfaceReference;
	}

	private void Awake()
	{
		if (generateHeightAtAwake)
		{
			GenerateHeight();
		}
		surfaceCurve = new Curve3D(GetPointArray());
		Material material = Resources.Load(settingsPath[Scenario.setting], typeof(Material)) as Material;
		if (material != null)
		{
			for (int i = 0; i < surfaceRenderers.Length; i++)
			{
				surfaceRenderers[i].material = material;
			}
		}
	}

	[ContextMenu("Get Surface Points")]
	public void GetSurfacePoints()
	{
		List<SurfacePoint> list = new List<SurfacePoint>(GetComponentsInChildren<SurfacePoint>());
		list.Sort(new SurfacePoint.PositionComparer());
		for (int i = 0; i < joints.Length; i++)
		{
			list.Remove(joints[i].pointB);
		}
		points = list.ToArray();
	}

	public void UpdateHeight(Vector3 point, float height)
	{
		List<SurfacePoint> list = new List<SurfacePoint>(points);
		list.Sort(new SurfacePoint.DistanceComparer(point));
		for (int i = 0; i < 3 && i < list.Count; i++)
		{
			list[i].SetHeight(height);
		}
		surfaceCurve = new Curve3D(GetPointArray());
	}

	[ContextMenu("Generate Height")]
	public void GenerateHeight()
	{
		if (points.Length > 1)
		{
			List<SurfacePoint> list = new List<SurfacePoint>(points);
			list[0].SetHeight(UnityEngine.Random.Range(0f - randomHeight, randomHeight));
			list[points.Length - 1].SetHeight(UnityEngine.Random.Range(0f - randomHeight, randomHeight));
			GenerateHeight(list);
			for (int i = 0; i < joints.Length; i++)
			{
				joints[i].Join();
			}
		}
	}

	[ContextMenu("Reset Height")]
	public void ResetHeight()
	{
		for (int i = 0; i < points.Length; i++)
		{
			points[i].SetHeight(0f);
		}
	}

	private void GenerateHeight(List<SurfacePoint> area)
	{
		int num = area.Count / 2;
		if (num != 0 && num != area.Count - 1)
		{
			float value = (area[0].GetHeight() + area[area.Count - 1].GetHeight()) / 2f + UnityEngine.Random.Range(0f - randomHeight, randomHeight);
			area[num].SetHeight(Mathf.Clamp(value, minHeight, maxHeight));
			GenerateHeight(area.GetRange(0, num));
			GenerateHeight(area.GetRange(num, area.Count - num));
		}
	}

	public float GetHeight(Vector3 point)
	{
		float t = Mathf.Clamp01((border + point.x) / length);
		return surfaceCurve.Interp(t).y;
	}

	private Vector3[] GetPointArray()
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < points.Length; i++)
		{
			list.Add(points[i].position);
		}
		list.Insert(0, list[0] + (list[0] - list[1]));
		list.Add(list[list.Count - 1] + (list[list.Count - 1] - list[list.Count - 2]));
		return list.ToArray();
	}

	private void OnDrawGizmosSelected()
	{
		float num = length / 2f;
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(Vector3.left * num, Vector3.one);
		Gizmos.DrawWireCube(Vector3.right * num, Vector3.one);
		Gizmos.color = Color.cyan;
		Curve3D curve3D = new Curve3D(GetPointArray());
		int num2 = (int)length;
		for (int i = 0; i < num2; i++)
		{
			Vector3 center = Vector3.Lerp(points[0].position, points[points.Length - 1].position, 1f / (float)num2 * (float)i);
			float t = Mathf.Clamp01((border + center.x) / length);
			center.y = curve3D.Interp(t).y;
			Gizmos.DrawWireSphere(center, 0.25f);
		}
	}
}
