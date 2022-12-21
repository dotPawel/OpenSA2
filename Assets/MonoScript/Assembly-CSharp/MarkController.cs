using System.Collections.Generic;
using UnityEngine;

public class MarkController : UIContainer
{
	public Camera uiCamera;

	public float offset;

	public List<Mark> marks;

	private Rect screenRect;

	private Camera gameCamera;

	private Vector3 centerPoint;

	private Vector2 border;

	private static MarkController markControllerReference;

	public static MarkController Find()
	{
		if (markControllerReference == null)
		{
			markControllerReference = (MarkController)Object.FindObjectOfType(typeof(MarkController));
		}
		return markControllerReference;
	}

	private void Start()
	{
		gameCamera = Camera.main;
		screenRect = new Rect(0f - offset, 0f - offset, (float)Screen.width + offset, (float)Screen.height + offset);
		centerPoint = new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f);
		border = new Vector2(uiCamera.aspect * uiCamera.orthographicSize - offset, uiCamera.orthographicSize - offset);
	}

	private void LateUpdate()
	{
		if (!show)
		{
			return;
		}
		for (int i = 0; i < marks.Count; i++)
		{
			Mark mark = marks[i];
			if (mark.IsActive())
			{
				Vector3 vector = gameCamera.WorldToScreenPoint(mark.GetPosition());
				if (screenRect.Contains(vector))
				{
					mark.SetVisibility(false);
					continue;
				}
				mark.SetVisibility(true);
				Vector3 normalized = (vector - centerPoint).normalized;
				mark.localPosition = GetIntersectPoint(new Ray(Vector3.zero, normalized));
			}
			else
			{
				mark.SetVisibility(false);
			}
		}
	}

	public void AddMark(Mark mark)
	{
		marks.Add(mark);
		mark.parent = base.uiTransform;
		mark.localRotation = Quaternion.identity;
		mark.show = show;
	}

	public void RemoveMark(Mark mark)
	{
		marks.Remove(mark);
		mark.parent = null;
	}

	private Vector3 GetIntersectPoint(Ray ray, float depth = 0f)
	{
		Vector3 direction = ray.direction;
		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
		{
			float x = Mathf.Sign(direction.x) * border.x;
			float y = GetY(ray, x);
			return new Vector3(x, y, depth);
		}
		float y2 = Mathf.Sign(direction.y) * border.y;
		float x2 = GetX(ray, y2);
		return new Vector3(x2, y2, depth);
	}

	private float GetY(Ray ray, float x)
	{
		Vector3 origin = ray.origin;
		Vector3 direction = ray.direction;
		return (x - origin.x) * direction.y / direction.x + origin.y;
	}

	private float GetX(Ray ray, float y)
	{
		Vector3 origin = ray.origin;
		Vector3 direction = ray.direction;
		return (y - origin.y) * direction.x / direction.y + origin.x;
	}

	private void OnDrawGizmosSelected()
	{
		if (uiCamera != null)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(size: new Vector3(uiCamera.aspect * uiCamera.orthographicSize - offset, uiCamera.orthographicSize - offset, 0f) * 2f, center: base.position);
		}
	}
}
