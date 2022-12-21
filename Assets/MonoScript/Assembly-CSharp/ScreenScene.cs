using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScene : MonoBehaviour
{
	[Serializable]
	public class EffectClip
	{
		public float time;

		public GameObject gameObject;
	}

	public Faction factionA;

	public Faction factionB;

	public Structure[] factioAStructures;

	public Turret[] factionATurrets;

	public Obstacle[] fractionAObstacles;

	public Structure[] factioBStructures;

	public Turret[] factionBTurrets;

	public Obstacle[] fractionBObstacles;

	public bool slowMotion;

	public int factionIndex;

	public Interval structureDamage = new Interval(100f, 200f);

	public Interval turretDamage = new Interval(50f, 100f);

	public HUD.Type hudType;

	public List<EffectClip> effects;

	private float timer;

	private void Start()
	{
		Structure[] array = factioAStructures;
		foreach (Structure structure in array)
		{
			structure.SetFaction(factionA);
			structure.Hit(structureDamage.random, 0);
		}
		Turret[] array2 = factionATurrets;
		foreach (Turret turret in array2)
		{
			turret.SetFaction(factionA);
			turret.Hit(turretDamage.random, 0);
		}
		Obstacle[] array3 = fractionAObstacles;
		foreach (Obstacle obstacle in array3)
		{
			obstacle.SetFaction(factionA);
		}
		Structure[] array4 = factioBStructures;
		foreach (Structure structure2 in array4)
		{
			structure2.SetFaction(factionB);
			structure2.Hit(structureDamage.random, 0);
		}
		Turret[] array5 = factionBTurrets;
		foreach (Turret turret2 in array5)
		{
			turret2.SetFaction(factionB);
			turret2.Hit(turretDamage.random, 0);
		}
		Obstacle[] array6 = fractionBObstacles;
		foreach (Obstacle obstacle2 in array6)
		{
			obstacle2.SetFaction(factionB);
		}
		HUD hUD = UIController.Find<HUD>();
		if (hUD != null)
		{
			hUD.airplaneStatusBar.armorRate = UnityEngine.Random.Range(0.4f, 0.9f);
			hUD.airplaneStatusBar.fuelRate = UnityEngine.Random.Range(0.4f, 0.9f);
			hUD.airplaneStatusBar.ammoRate = UnityEngine.Random.Range(0.4f, 0.9f);
			hUD.airplaneStatusBar.bombCount = UnityEngine.Random.Range(1, 4);
			hUD.scoreBar.score = UnityEngine.Random.Range(100, 900);
			hUD.scoreBar.icon = factionIndex;
			hUD.SetType(hudType);
			hUD.Show();
		}
		if (slowMotion)
		{
			Time.timeScale = 0.1f;
		}
	}

	private void Update()
	{
		timer += Time.deltaTime;
		int num = 0;
		while (num < effects.Count)
		{
			if (effects[num].time < timer)
			{
				effects[num].gameObject.SetActiveRecursively(true);
				effects.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}
}
