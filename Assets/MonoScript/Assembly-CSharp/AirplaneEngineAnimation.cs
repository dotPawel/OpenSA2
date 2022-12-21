using UnityEngine;

public class AirplaneEngineAnimation : UnitAnimation
{
	public Vector3 engineRotateAngle;

	public Transform engineTransform;

	public override void SetRate(float value)
	{
		engineTransform.Rotate(engineRotateAngle * value, Space.Self);
	}
}
