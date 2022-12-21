using UnityEngine;

public class Mark : UIElement
{
	private const int TICK = 5;

	private const int VISIBLE_DISTANCE = 7;

	private const int MAX_DISTANCE = 99;

	private const float LABEL_OFFSET = 40f;

	public AObject point;

	public AObject target;

	public Renderer meshRenderer;

	public Renderer labelRenderer;

	public UIText distanceLabel;

	private bool isVisible;

	private int lastDistance;

	private int tick;

	public override bool show
	{
		get
		{
			return base.show;
		}
		set
		{
			distanceLabel.show = value;
			base.show = value;
		}
	}

	public Mark InstantiateFromResource(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = SceneResources.Pop(base.gameObject, position, rotation);
		gameObject.gameObject.SetActiveRecursively(true);
		return gameObject.GetComponent<Mark>();
	}

	public void Remove()
	{
		base.gameObject.SetActiveRecursively(false);
		SceneResources.Push(base.gameObject);
	}

	public virtual void SetVisibility(bool value)
	{
		isVisible = value;
		meshRenderer.enabled = value;
		labelRenderer.enabled = value;
	}

	public virtual bool IsActive()
	{
		return target != null;
	}

	public virtual Vector3 GetPosition()
	{
		return target.position;
	}

	public void SetDistanceTo(AObject point)
	{
		this.point = point;
		if (point != null && target != null)
		{
			SetDistance(Mathf.RoundToInt(Mathf.Abs(point.position.x - target.position.x) * 10f));
		}
	}

	private void FixedUpdate()
	{
		if (target != null && point != null && isVisible && tick++ % 5 == 0)
		{
			SetDistance(Mathf.RoundToInt(Mathf.Abs(point.position.x - target.position.x) * 10f));
		}
	}

	private void LateUpdate()
	{
		if (target != null && isVisible)
		{
			distanceLabel.position = base.position + ((!(base.localPosition.x > 0f)) ? (Vector3.right * 40f) : (Vector3.left * 40f));
		}
	}

	private void SetDistance(int value)
	{
		if (lastDistance != value)
		{
			lastDistance = value;
			int num = value / 10;
			int num2 = Mathf.Max(num - 7, 0);
			if (num2 > 99)
			{
				distanceLabel.text = string.Format("{0}", num2);
			}
			else
			{
				distanceLabel.text = string.Format("{0}.{1}", num2, value - num * 10);
			}
		}
	}
}
