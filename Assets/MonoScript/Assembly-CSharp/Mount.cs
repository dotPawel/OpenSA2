using UnityEngine;

public class Mount : AObject
{
	public float damage;

	public int targetId;

	public float criticalStrike;

	public Mount Instantiate(Vector3 position, Quaternion rotation, float damage, int targetId)
	{
		GameObject gameObject = SceneResources.Pop(base.gameObject, position, rotation);
		Mount component = gameObject.GetComponent<Mount>();
		component.damage = damage;
		component.targetId = targetId;
		gameObject.SetActiveRecursively(true);
		return component;
	}

	public void Remove()
	{
		base.gameObject.SetActiveRecursively(false);
		SceneResources.Push(base.gameObject);
	}
}
