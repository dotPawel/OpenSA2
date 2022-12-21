using UnityEngine;

public class UIBorder : UIElement
{
	public Camera uiCamera;

	public bool autoResize;

	private MeshEditor meshEditorPointer;

	protected MeshEditor meshEditor
	{
		get
		{
			if (meshEditorPointer == null)
			{
				meshEditorPointer = GetComponent<MeshEditor>();
			}
			return meshEditorPointer;
		}
	}

	private void Start()
	{
		if (autoResize && uiCamera != null)
		{
			float num = (float)Screen.width / (float)Screen.height;
			meshEditor.size = new Vector2(uiCamera.orthographicSize * 2f * num, uiCamera.orthographicSize * 2f);
		}
	}
}
