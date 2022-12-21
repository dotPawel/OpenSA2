using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Scenario
{
	public List<TutorialStage> stages;

	public Structure airport;

	public Vector3 targetPoint;

	public UIIndicator tutorialTitles;

	public TutorialMark tutorialMark;

	public ControlSelector controlSelector;

	public bool sessionComplete;

	private void Start()
	{
		controlSelector.onControlSelected += OnControlSelected;
		controlSelector.SetVisible(true);
	}

	private void OnControlSelected(ControlSelector controlSelector, int controlIndex)
	{
		controlSelector.SetVisible(false);
		Player player = Player.Find();
		if (player != null)
		{
			player.SetControlType((ControlType)controlIndex);
		}
		if (stages.Count > 0)
		{
			Faction faction = Player.faction;
			playerSpawner.SetAirport(airport);
			airport.SetFaction(faction);
			stages[0].onCompleted += OnStageCompleted;
			stages[0].Init(this);
			StartScenario();
		}
	}

	private void OnStageCompleted(TutorialStage stage)
	{
		if (!base.finished)
		{
			stage.onCompleted -= OnStageCompleted;
			stages.Remove(stage);
			if (stages.Count > 0)
			{
				stages[0].onCompleted += OnStageCompleted;
				stages[0].Init(this);
			}
			else
			{
				FinishScenario(true);
			}
		}
	}

	public override bool SessionComplete()
	{
		return sessionComplete;
	}

	public override void FinishScenario(bool completed)
	{
		tutorialTitles.SetState(0);
		base.FinishScenario(completed);
	}

	public override Vector3 GetTargetPoint(Faction faction)
	{
		return targetPoint;
	}

	public void SetTitle(int index)
	{
		tutorialTitles.SetState(index);
	}

	public void SetMark(Target target)
	{
		tutorialMark.SetTarget(target);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(targetPoint, 0.25f);
	}
}
