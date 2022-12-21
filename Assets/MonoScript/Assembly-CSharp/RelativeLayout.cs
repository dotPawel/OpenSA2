using System;
using System.Collections.Generic;
using UnityEngine;

public class RelativeLayout : MonoBehaviour
{
	[Serializable]
	public class Rule
	{
		public enum Alignment
		{
			Left = 0,
			Top = 1,
			Right = 2,
			Bottom = 3
		}

		public enum OffsetType
		{
			Units = 0,
			Rate = 1
		}

		public Alignment alignment;

		public Transform target;

		public OffsetType offsetType;

		public float offset;

		public void Apply(Transform location)
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector3 position = location.position;
			Camera uiCamera = UIManager.current.uiCamera;
			switch (alignment)
			{
			case Alignment.Left:
			case Alignment.Right:
				zero2 = ((alignment != 0) ? Vector3.right : Vector3.left);
				zero = ((!(target != null)) ? (uiCamera.transform.position + zero2 * uiCamera.aspect * uiCamera.orthographicSize) : target.position);
				position.x = (zero - zero2 * ((offsetType != OffsetType.Rate) ? offset : (offset * uiCamera.aspect * uiCamera.orthographicSize * 2f))).x;
				break;
			case Alignment.Top:
			case Alignment.Bottom:
				zero2 = ((alignment != Alignment.Top) ? Vector3.down : Vector3.up);
				zero = ((!(target != null)) ? (uiCamera.transform.position + zero2 * uiCamera.orthographicSize) : target.position);
				position.y = (zero - zero2 * ((offsetType != OffsetType.Rate) ? offset : (offset * uiCamera.orthographicSize * 2f))).y;
				break;
			}
			location.position = position;
		}
	}

	private Transform transformPointer;

	public List<Rule> rules = new List<Rule>();

	private Transform goTransform
	{
		get
		{
			if (transformPointer == null)
			{
				transformPointer = base.gameObject.GetComponent<Transform>();
			}
			return transformPointer;
		}
	}

	private void Start()
	{
		ApplyRules();
	}

	public void ApplyRules()
	{
		foreach (Rule rule in rules)
		{
			rule.Apply(goTransform);
		}
	}

	public void AddRule(Rule rule)
	{
		rules.Add(rule);
	}

	public void RemoveRule(Rule rule)
	{
		rules.Remove(rule);
	}

	public void RemoveRule()
	{
		int count = rules.Count;
		if (count > 0)
		{
			rules.RemoveAt(count - 1);
		}
	}
}
