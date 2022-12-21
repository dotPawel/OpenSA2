using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSkillSystem : MonoBehaviour
{
	public List<PlayerSkill> skills;

	public int skillsPerRank = 3;

	public int skillPoinsPerRank = 1;

	[method: MethodImpl(32)]
	public event Action<PlayerSkillSystem> onInitCompleted;

	public void Init()
	{
		Player.rank = PlayerRankSystem.GetPlayerRank(Campaign.score);
		UpdatePlayerProfile();
		if (GetPlayerSkillPoints() > GetUsedSkillPoints())
		{
			if (GetUsedSkillPoints() > 0)
			{
				SortSkills();
			}
			SkillScreen skillScreen = UIController.Find<SkillScreen>();
			for (int i = 0; i < skillsPerRank; i++)
			{
				PlayerSkill playerSkill = skills[i];
				skillScreen.SetSkillData(i, playerSkill.title, (int)playerSkill.type, playerSkill.text, playerSkill.description);
			}
			skillScreen.SetSelected(0);
			skillScreen.Show();
			skillScreen.onDoneClicked += OnSkillScreenDoneClicked;
		}
		else
		{
			Complete();
		}
	}

	[ContextMenu("Reset Skills")]
	public void Reset()
	{
		for (int i = 0; i < skills.Count; i++)
		{
			skills[i].Reset();
		}
	}

	[ContextMenu("Print Skills")]
	public void PrintSkills()
	{
		for (int i = 0; i < skills.Count; i++)
		{
			Debug.Log(string.Format("{0}:{1}", skills[i].title, skills[i].level));
		}
	}

	private void OnSkillScreenDoneClicked(SkillScreen skillScreen)
	{
		skills[skillScreen.selectedTab].LevelUp();
		UpdatePlayerProfile();
		if (GetPlayerSkillPoints() > GetUsedSkillPoints())
		{
			SortSkills();
			for (int i = 0; i < skillsPerRank; i++)
			{
				PlayerSkill playerSkill = skills[i];
				skillScreen.SetSkillData(i, playerSkill.title, (int)playerSkill.type, playerSkill.text, playerSkill.description);
			}
			skillScreen.SetSelected(0);
		}
		else
		{
			Complete();
		}
	}

	private void Complete()
	{
		if (this.onInitCompleted != null)
		{
			this.onInitCompleted(this);
		}
	}

	public int GetPlayerSkillPoints()
	{
		return (Player.rank + 1) * skillPoinsPerRank;
	}

	public int GetUsedSkillPoints()
	{
		int num = 0;
		for (int i = 0; i < skills.Count; i++)
		{
			num += skills[i].level;
		}
		return num;
	}

	public void SortSkills()
	{
		skills.Sort(new PlayerSkill.LevelComparer());
	}

	public void UpdatePlayerProfile()
	{
		PlayerProfile playerProfile = UIController.Find<PlayerProfile>();
		playerProfile.rank = Player.rank;
		playerProfile.score = Campaign.score;
	}
}
