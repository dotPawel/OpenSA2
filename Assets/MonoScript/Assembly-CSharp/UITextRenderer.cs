using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class UITextRenderer : MonoBehaviour
{
	public float fontSize = 10f;

	public float lineSpace = 10f;

	public float spaceWidth = 1f;

	public Vector2 rect;

	public bool wrapWords;

	public TextParser.Anchor anchor;

	public TextParser.Alignment alignment;

	public UIFont font;

	public string textSource = string.Empty;

	public Color colorSource = Color.white;

	private MeshFilter meshFilterPointer;

	private Color[] colors;

	public string text
	{
		get
		{
			return textSource;
		}
		set
		{
			RenderText(value);
		}
	}

	public float alpha
	{
		get
		{
			return colorSource.a;
		}
		set
		{
			if (colorSource.a != value)
			{
				colorSource.a = value;
				UpdateColor();
			}
		}
	}

	public Color color
	{
		get
		{
			return colorSource;
		}
		set
		{
			if (colorSource != value)
			{
				colorSource = value;
				UpdateColor();
			}
		}
	}

	private MeshFilter meshFilter
	{
		get
		{
			if (meshFilterPointer == null)
			{
				meshFilterPointer = GetComponent<MeshFilter>();
			}
			return meshFilterPointer;
		}
	}

	private void OnDrawGizmos()
	{
		if (wrapWords)
		{
			Gizmos.color = Color.green;
			Vector3 vector = Vector3.zero;
			switch (alignment)
			{
			case TextParser.Alignment.Left:
				vector = Vector3.right * rect.x / 2f;
				break;
			case TextParser.Alignment.Right:
				vector = Vector3.left * rect.x / 2f;
				break;
			}
			Gizmos.DrawWireCube(base.transform.position + vector, new Vector3(rect.x, rect.y, 0f));
		}
		if (meshFilter.sharedMesh == null)
		{
			Render();
		}
	}

	private void OnEnable()
	{
		Render();
	}

	public void Render()
	{
		if (font == null)
		{
			return;
		}
		if (meshFilter.sharedMesh != null)
		{
			meshFilter.sharedMesh.Clear();
		}
		TextParser textParser = new TextParser(font, wrapWords, rect, lineSpace, spaceWidth, fontSize);
		TextParser.Page page = textParser.ParseText(textSource);
		page.SetAnchor(anchor);
		page.SetAlignment(alignment);
		List<MeshChar> characters = page.characters;
		if (characters.Count <= 0)
		{
			return;
		}
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<Vector2> list2 = new List<Vector2>();
		List<int> list3 = new List<int>();
		foreach (MeshChar item in characters)
		{
			int[] array = new int[item.triangles.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = list.Count + item.triangles[i];
			}
			list3.AddRange(array);
			list.AddRange(item.vertices);
			list2.AddRange(item.uv);
		}
		colors = GenerateColor(list.Count);
		mesh.vertices = list.ToArray();
		mesh.uv = list2.ToArray();
		mesh.triangles = list3.ToArray();
		mesh.colors = colors;
		meshFilter.mesh = mesh;
		UpdateColor();
	}

	private void UpdateColor()
	{
		if (colors == null)
		{
			colors = meshFilter.sharedMesh.colors;
		}
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = colorSource;
		}
		meshFilter.sharedMesh.colors = colors;
	}

	private Color[] GenerateColor(int count)
	{
		Color[] array = new Color[count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Color(1f, 1f, 1f, 1f);
		}
		return array;
	}

	private void RenderText(string arg)
	{
		textSource = arg;
		Render();
	}
}
