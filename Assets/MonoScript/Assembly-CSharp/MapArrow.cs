using UnityEngine;

public class MapArrow : AObject
{
	private const int COLOR_MATERIAL_INDEX = 1;

	public Transform trailPoint;

	public Renderer meshRenderer;

	public Color[] colors;

	public void SetColor(int index)
	{
		meshRenderer.materials[1].color = colors[index];
	}

	public void SetArrow(Vector3 start, Vector3 end)
	{
		base.position = end;
		base.rotation = Quaternion.LookRotation(end - start, Vector3.up);
		trailPoint.position = start;
	}

	public MapArrow Instantiate(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(base.gameObject, position, rotation);
		return gameObject.GetComponent<MapArrow>();
	}
}
