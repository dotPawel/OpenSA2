using System.Collections.Generic;
using UnityEngine;

public class BufferingElements : AObject
{
	public GameObject[] elements;

	public int count = 1;

	private void Start()
	{
		GameObject[] array = elements;
		foreach (GameObject item in array)
		{
			List<GameObject> list = new List<GameObject>();
			for (int j = 0; j < count; j++)
			{
				list.Add(SceneResources.Pop(item, base.position, base.rotation));
			}
			foreach (GameObject item2 in list)
			{
				item2.SetActiveRecursively(false);
				SceneResources.Push(item2);
			}
		}
	}
}
