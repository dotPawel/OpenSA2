using System.Collections.Generic;
using UnityEngine;

public class ObjectivesMarks : AObject
{
	private const int TICK = 10;

	public Mark markPrefab;

	public List<Objective> objectives;

	public PlayerSpawner playerSpawner;

	private Mark mark;

	private MarkController markController;

	private Player player;

	private void Start()
	{
		if (objectives.Count > 0)
		{
			player = Player.Find();
			markController = MarkController.Find();
			mark = markPrefab.InstantiateFromResource(Vector3.zero, Quaternion.identity);
			mark.SetDistanceTo(player);
			markController.AddMark(mark);
			mark.target = objectives[0];
			for (int i = 0; i < objectives.Count; i++)
			{
				objectives[i].onCompleted += OnObjectiveCompleted;
			}
		}
	}

	private void OnObjectiveCompleted(Objective objective)
	{
		objective.onCompleted -= OnObjectiveCompleted;
		objectives.Remove(objective);
		if (objectives.Count > 0)
		{
			mark.target = objectives[0];
			return;
		}
		playerSpawner.RemoveAirportMark();
		mark.target = playerSpawner.airport;
	}
}
