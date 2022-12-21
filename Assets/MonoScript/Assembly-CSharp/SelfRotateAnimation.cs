using UnityEngine;

public class SelfRotateAnimation : AObject
{
	public Vector3 rotate;

	public bool resetLocalRotation = true;

	private void OnEnable()
	{
		if (resetLocalRotation)
		{
			base.localRotation = Quaternion.identity;
		}
	}

	private void FixedUpdate()
	{
		Rotate(rotate * Time.deltaTime, Space.Self);
	}
}
