using UnityEngine;

public class TutorialMark : AObject
{
	private const int TICK = 5;

	public Mark markPrefab;

	private Mark mark;

	private MarkController markController;

	private Player player;

	private void Start()
	{
		player = Player.Find();
		markController = MarkController.Find();
		mark = markPrefab.InstantiateFromResource(Vector3.zero, Quaternion.identity);
		mark.SetDistanceTo(player);
		markController.AddMark(mark);
	}

	public void SetTarget(Target target)
	{
		mark.target = target;
	}
}
