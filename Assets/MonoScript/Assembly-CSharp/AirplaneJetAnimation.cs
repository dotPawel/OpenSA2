using UnityEngine;

public class AirplaneJetAnimation : UnitAnimation
{
	public Interval flameScale;

	public Vector3 flameRotation;

	public Transform[] flames;

	public float scaleSpeed = 1f;

	private float timer;

	public override void SetRate(float value)
	{
		float num = 1f / scaleSpeed;
		float t = Mathf.PingPong(timer += value, num) / num;
		for (int i = 0; i < flames.Length; i++)
		{
			flames[i].localScale = Vector3.one * flameScale.Lerp(t);
			flames[i].Rotate(flameRotation * value, Space.Self);
		}
	}
}
