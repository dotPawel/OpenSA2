using UnityEngine;

public class Unit : Target
{
	public Unit Instantiate(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = SceneResources.Pop(base.gameObject, position, rotation);
		gameObject.SetActiveRecursively(true);
		return gameObject.GetComponent<Unit>();
	}

	public void Remove()
	{
		base.gameObject.SetActiveRecursively(false);
		SceneResources.Push(base.gameObject);
	}

	public override void Destroy()
	{
		base.Destroy();
		Remove();
	}
}
