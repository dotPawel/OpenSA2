using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Scenario : AObject
{
	public PlayerSpawner playerSpawner;

	public bool supplyAvailable = true;

	public bool airplaneSelectionAvailable = true;

	private static Scenario scenarioReference;

	public bool completed { get; protected set; }

	public bool finished { get; protected set; }

	public static int difficulty
	{
		get
		{
			return PlayerPrefs.GetInt("scenraio.difficulty", 0);
		}
		set
		{
			PlayerPrefs.SetInt("scenraio.difficulty", value);
		}
	}

	public static int setting
	{
		get
		{
			return PlayerPrefs.GetInt("scenraio.setting", 0);
		}
		set
		{
			PlayerPrefs.SetInt("scenraio.setting", value);
		}
	}

	[method: MethodImpl(32)]
	public event Action<Scenario> onScenarioStarted;

	[method: MethodImpl(32)]
	public event Action<Scenario> onScenarioFinished;

	public static Scenario Find()
	{
		if (scenarioReference == null)
		{
			scenarioReference = UnityEngine.Object.FindObjectOfType(typeof(Scenario)) as Scenario;
		}
		return scenarioReference;
	}

	public virtual void StartScenario()
	{
		MonoBehaviour[] componentsInChildren = GetComponentsInChildren<MonoBehaviour>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ScenarioListener scenarioListener = componentsInChildren[i] as ScenarioListener;
			if (scenarioListener != null)
			{
				scenarioListener.OnScenarioStart(this);
			}
		}
		if (this.onScenarioStarted != null)
		{
			this.onScenarioStarted(this);
		}
	}

	public virtual void FinishScenario(bool completed = true)
	{
		if (finished)
		{
			return;
		}
		finished = true;
		this.completed = completed;
		MonoBehaviour[] componentsInChildren = GetComponentsInChildren<MonoBehaviour>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ScenarioListener scenarioListener = componentsInChildren[i] as ScenarioListener;
			if (scenarioListener != null)
			{
				scenarioListener.OnScenarioFinish(this);
			}
		}
		if (this.onScenarioFinished != null)
		{
			this.onScenarioFinished(this);
		}
	}

	public virtual bool SessionComplete()
	{
		return finished;
	}

	public virtual Vector3 GetTargetPoint(Faction faction)
	{
		return Vector3.zero;
	}

	[ContextMenu("Set difficult")]
	protected void SetDifficult()
	{
		difficulty = 3;
	}
}
