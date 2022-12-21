using System.Collections.Generic;
using UnityEngine;

public class FlagMarks : MonoBehaviour
{
	public Mark markPrefab;

	public List<FlagPlatform> flags;

	private MarkController markController;

	private Mark mark;

	private void Start()
	{
		if (flags.Count > 0)
		{
			markController = MarkController.Find();
			mark = markPrefab.InstantiateFromResource(Vector3.zero, Quaternion.identity);
			mark.target = flags[0];
			Player player = Player.Find();
			if (player != null)
			{
				mark.SetDistanceTo(player);
			}
			markController.AddMark(mark);
			for (int i = 0; i < flags.Count; i++)
			{
				flags[i].onCaptured += OnFlagCaptured;
			}
		}
	}

	private void OnFlagCaptured(FlagPlatform flag)
	{
		flag.onCaptured -= OnFlagCaptured;
		if (flags.Count > 0)
		{
			mark.target = flags[0];
			return;
		}
		markController.RemoveMark(mark);
		mark.Remove();
	}
}
