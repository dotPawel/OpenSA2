using UnityEngine;

public class ObjectLabel : AObject
{
	public Transform labelTransform;

	public Transform elementTransform;

	public Vector3 lookVector = Vector3.back;

	public Vector3 offset = new Vector3(0f, 3f, 0f);

	private void Update()
	{
		labelTransform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
		labelTransform.position = base.position + offset;
	}

	[ContextMenu("Random Engine Rotate")]
	private void RotateElement()
	{
		if (elementTransform != null)
		{
			elementTransform.localRotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
		}
	}
}
