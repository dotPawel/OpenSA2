using UnityEngine;

public class UIElementEditor : MonoBehaviour
{
	public bool saveMesh = true;

	public UIElementEditor Dublecate()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(base.gameObject);
		gameObject.transform.position = base.transform.position;
		gameObject.transform.parent = base.transform.parent;
		MeshFilter[] componentsInChildren = gameObject.GetComponentsInChildren<MeshFilter>();
		foreach (MeshFilter meshFilter in componentsInChildren)
		{
			meshFilter.mesh = (Mesh)Object.Instantiate(meshFilter.sharedMesh);
		}
		return gameObject.GetComponent<UIElementEditor>();
	}
}
