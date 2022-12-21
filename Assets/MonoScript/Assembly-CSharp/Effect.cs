using UnityEngine;

public class Effect : AObject
{
	public Effect Instantiate(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = SceneResources.Pop(base.gameObject, position, rotation);
		gameObject.gameObject.SetActiveRecursively(true);
		return gameObject.GetComponent<Effect>();
	}

	public void Remove()
	{
		base.gameObject.SetActiveRecursively(false);
		SceneResources.Push(base.gameObject);
	}
}
