using System;
using System.Collections.Generic;
using UnityEngine;

public class UIStatesController : UIController
{
	[Serializable]
	public class State
	{
		public string name = "Main";

		public List<int> indexes;

		public State()
		{
			indexes = new List<int>();
		}
	}

	public List<UIController> controllers;

	public List<State> states;

	public bool showAtStart;

	public int defaultStateIndex;

	private Stack<State> stackStates = new Stack<State>();

	public static UIStatesController Find(string name)
	{
		GameObject gameObject = GameObject.Find(name);
		return (!(gameObject != null)) ? null : gameObject.GetComponent<UIStatesController>();
	}

	private void Start()
	{
		if (showAtStart)
		{
			ShowDefault();
		}
	}

	public void ShowDefault()
	{
		stackStates.Clear();
		Show(defaultStateIndex);
	}

	public void Show(string name)
	{
		for (int i = 0; i < states.Count; i++)
		{
			if (states[i].name.Equals(name))
			{
				Show(states[i]);
				break;
			}
		}
	}

	public void Show(int stateIndex)
	{
		if (stateIndex < states.Count)
		{
			Show(states[stateIndex]);
		}
	}

	public void Show(State state)
	{
		if (state == null)
		{
			return;
		}
		stackStates.Push(state);
		for (int i = 0; i < controllers.Count; i++)
		{
			if (!state.indexes.Contains(i))
			{
				UIController uIController = controllers[i];
				if (uIController.show)
				{
					Hide(uIController);
				}
			}
		}
		for (int j = 0; j < controllers.Count; j++)
		{
			if (state.indexes.Contains(j))
			{
				UIController uIController2 = controllers[j];
				if (!uIController2.show)
				{
					Show(uIController2);
				}
			}
		}
	}

	public void Back()
	{
		stackStates.Pop();
		if (stackStates.Count > 0)
		{
			Show(stackStates.Pop());
		}
		else
		{
			Show(defaultStateIndex);
		}
	}

	private void Show(UIController controller)
	{
		controller.rootControler = this;
		controller.ViewWillAppear();
		controller.show = true;
		controller.ViewDidAppear();
	}

	private void Hide(UIController controller)
	{
		controller.ViewWillDisappear();
		controller.show = false;
		controller.ViewDidDisappear();
	}
}
