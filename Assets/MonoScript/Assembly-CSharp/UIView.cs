using UnityEngine;

[RequireComponent(typeof(MeshEditor))]
public class UIView : UIElement
{
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

	public virtual float alpha
	{
		get
		{
			return meshEditor.alpha;
		}
		set
		{
			meshEditor.alpha = value;
		}
	}

	public Texture texture
	{
		get
		{
			return (!(this != null) || !(base.GetComponent<Renderer>() != null) || !(base.GetComponent<Renderer>().material != null)) ? null : base.GetComponent<Renderer>().material.mainTexture;
		}
		set
		{
			if (this != null && base.GetComponent<Renderer>() != null && base.GetComponent<Renderer>().material != null)
			{
				base.GetComponent<Renderer>().material.mainTexture = value;
			}
		}
	}
}
