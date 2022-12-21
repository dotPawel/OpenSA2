using UnityEngine;

public class RandomMission : Mission
{
	public Type[] types;

	public Setting[] settings;

	public void Randomize()
	{
		type = types[Random.Range(0, types.Length)];
		setting = settings[Random.Range(0, settings.Length)];
	}

	public override void Load()
	{
		Mission.missionArea = string.Empty;
		Mission.missionType = type;
		Scenario.difficulty = base.level;
		Scenario.setting = (int)setting;
		Game.LoadLevel(type.ToString());
	}
}
