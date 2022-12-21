using UnityEngine;

public class AObject : MonoBehaviour
{
	private Transform aObjectTransform;

	private int tick;

	protected Transform goTransform
	{
		get
		{
			return (!(aObjectTransform != null)) ? (aObjectTransform = base.gameObject.transform) : aObjectTransform;
		}
	}

	public Vector3 position
	{
		get
		{
			return goTransform.position;
		}
		set
		{
			goTransform.position = value;
		}
	}

	public Vector3 localPosition
	{
		get
		{
			return goTransform.localPosition;
		}
		set
		{
			goTransform.localPosition = value;
		}
	}

	public Quaternion rotation
	{
		get
		{
			return goTransform.rotation;
		}
		set
		{
			goTransform.rotation = value;
		}
	}

	public Quaternion localRotation
	{
		get
		{
			return goTransform.localRotation;
		}
		set
		{
			goTransform.localRotation = value;
		}
	}

	public Vector3 localScale
	{
		get
		{
			return goTransform.localScale;
		}
		set
		{
			goTransform.localScale = value;
		}
	}

	public Transform parent
	{
		get
		{
			return goTransform.parent;
		}
		set
		{
			goTransform.parent = value;
		}
	}

	public Vector3 forward
	{
		get
		{
			return goTransform.forward;
		}
		set
		{
			goTransform.forward = value;
		}
	}

	public Vector3 normal
	{
		get
		{
			return goTransform.up;
		}
		set
		{
			goTransform.up = value;
		}
	}

	public bool show
	{
		get
		{
			return base.gameObject.active;
		}
		set
		{
			if (show != value)
			{
				base.gameObject.SetActiveRecursively(value);
			}
		}
	}

	public void LookAt(Vector3 point)
	{
		goTransform.LookAt(point);
	}

	public void Rotate(Vector3 angles, Space space)
	{
		goTransform.Rotate(angles, space);
	}

	protected bool GetTick(int tick)
	{
		return this.tick++ % tick == 0;
	}
}
