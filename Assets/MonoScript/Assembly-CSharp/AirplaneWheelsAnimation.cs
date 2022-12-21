using UnityEngine;

public class AirplaneWheelsAnimation : UnitAnimation
{
	public Renderer[] wheelsRenderers;

	public Transform[] wheelsTransforms;

	public Vector3[] rotateAngles;

	private Quaternion[] wheelsRotations;

	public bool visibility { get; private set; }

	public override void SetRate(float value)
	{
		if (wheelsRotations == null)
		{
			wheelsRotations = new Quaternion[rotateAngles.Length];
			for (int i = 0; i < wheelsRotations.Length; i++)
			{
				wheelsRotations[i] = Quaternion.Euler(rotateAngles[i]);
			}
		}
		bool flag = value < 1f;
		if (flag != visibility)
		{
			visibility = flag;
			for (int j = 0; j < wheelsRenderers.Length; j++)
			{
				wheelsRenderers[j].enabled = flag;
			}
		}
		if (flag && base.rate != value)
		{
			base.rate = value;
			for (int k = 0; k < wheelsTransforms.Length; k++)
			{
				wheelsTransforms[k].localRotation = Quaternion.Slerp(Quaternion.identity, wheelsRotations[k], base.rate);
			}
		}
	}
}
