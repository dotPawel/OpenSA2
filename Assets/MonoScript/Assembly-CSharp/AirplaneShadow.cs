using UnityEngine;

public class AirplaneShadow : AObject
{
	public AirplaneShadow Instantiate(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = SceneResources.Pop(base.gameObject, position, rotation);
		gameObject.SetActiveRecursively(true);
		return gameObject.GetComponent<AirplaneShadow>();
	}

	public void Remove()
	{
		base.gameObject.SetActiveRecursively(false);
		SceneResources.Push(base.gameObject);
	}
}
