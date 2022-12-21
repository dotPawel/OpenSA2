using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class SkillScreen : UIController
{
	public List<UITabButton> tabButtons;

	public UIButton doneButton;

	public int selectedTab { get; private set; }

	[method: MethodImpl(32)]
	public event Action<SkillScreen> onDoneClicked;

	private void Start()
	{
		for (int i = 0; i < tabButtons.Count; i++)
		{
			tabButtons[i].onClicked += OnTabClicked;
		}
		doneButton.onClicked += _003CStart_003Em__1C;
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("SkillsScreen");
	}

	private void OnTabClicked(UITabButton tabButton)
	{
		SetSelected(tabButtons.IndexOf(tabButton));
	}

	public void SetSkillData(int index, string title, int icon, string text, string description)
	{
		if (index < tabButtons.Count)
		{
			UISkillButtonView uISkillButtonView = (UISkillButtonView)tabButtons[index].view;
			uISkillButtonView.title = title;
			uISkillButtonView.SelectIcon(icon);
			uISkillButtonView.text = text;
			uISkillButtonView.description = description;
		}
	}

	public void SetSelected(int value)
	{
		selectedTab = value;
		for (int i = 0; i < tabButtons.Count; i++)
		{
			tabButtons[i].selected = value == i;
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1C(UIButton P_0)
	{
		if (this.onDoneClicked != null)
		{
			this.onDoneClicked(this);
		}
	}
}
