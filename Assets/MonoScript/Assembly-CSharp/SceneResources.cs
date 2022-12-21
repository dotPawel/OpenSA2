using System.Collections.Generic;
using UnityEngine;

public class SceneResources : MonoBehaviour
{
	private Dictionary<string, Queue<GameObject>> resources = new Dictionary<string, Queue<GameObject>>();

	private static SceneResources sceneResourcesReference;

	private static SceneResources current
	{
		get
		{
			if (sceneResourcesReference == null)
			{
				sceneResourcesReference = Object.FindObjectOfType(typeof(SceneResources)) as SceneResources;
			}
			return sceneResourcesReference;
		}
	}

	public static GameObject Pop(GameObject item, Vector3 position, Quaternion rotation)
	{
		string key = item.name;
		Dictionary<string, Queue<GameObject>> dictionary = current.resources;
		if (dictionary.ContainsKey(key))
		{
			Queue<GameObject> queue = dictionary[key];
			if (queue.Count > 0)
			{
				GameObject gameObject = queue.Dequeue();
				Transform transform = gameObject.transform;
				transform.position = position;
				transform.rotation = rotation;
				return gameObject;
			}
		}
		return InstantiateItem(item, position, rotation);
	}

	public static void Push(GameObject item)
	{
		string key = item.name;
		Dictionary<string, Queue<GameObject>> dictionary = current.resources;
		if (dictionary.ContainsKey(key))
		{
			Queue<GameObject> queue = dictionary[key];
			queue.Enqueue(item);
		}
		else
		{
			Queue<GameObject> queue2 = new Queue<GameObject>();
			queue2.Enqueue(item);
			dictionary.Add(key, queue2);
		}
	}

	public static GameObject InstantiateItem(GameObject item, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(item, position, rotation);
		gameObject.name = item.name;
		return gameObject;
	}
}
