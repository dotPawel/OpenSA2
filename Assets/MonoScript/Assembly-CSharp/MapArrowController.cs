using UnityEngine;

public class MapArrowController : AObject
{
	public MapArrow mapArrowRrefab;

	public float startRange = 0.1f;

	public float endRange = 0.5f;

	public void AddArrow(Vector3 start, Vector3 end)
	{
		start.y = (end.y = base.position.y);
		MapArrow mapArrow = mapArrowRrefab.Instantiate(base.position, base.rotation);
		mapArrow.parent = base.goTransform;
		Vector3 normalized = (end - start).normalized;
		mapArrow.SetArrow(start + normalized * startRange, end - normalized * endRange);
		mapArrow.SetColor(Player.factionIndex);
	}
}
