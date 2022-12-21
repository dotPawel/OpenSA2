using UnityEngine;

public class ScenarioScheme : MonoBehaviour
{
	public Structure playerAirport;

	public ScenarioSide sideA;

	public ScenarioSide sideB;

	public void SetEnable(bool value)
	{
		base.gameObject.SetActiveRecursively(value);
	}
}
