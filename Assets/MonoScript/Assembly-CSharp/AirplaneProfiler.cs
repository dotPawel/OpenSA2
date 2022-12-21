using System;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneProfiler : MonoBehaviour
{
	[Serializable]
	public class AirplaneInfo
	{
		private const float DAMAGE_UPGRADE_RATE = 0.1f;

		private const float ARMOR_UPGRADE_RATE = 0.1f;

		private const float MOBILITY_UPGRADE_RATE = 0.1f;

		private const float RATE_INDEX_VALUE = 10f;

		private readonly int[] PRICES = new int[11]
		{
			250, 500, 750, 1000, 1250, 1500, 1750, 2000, 2250, 2500,
			2750
		};

		public AirplaneType type;

		public string name;

		public int price;

		public bool locked;

		public int startDamageIndex;

		public int startArmorIndex;

		public int startMobilityIndex;

		public int damageUpgrades;

		public int armorUpgrades;

		public int mobilityUpgrades;

		public bool unlocked
		{
			get
			{
				return !locked || PlayerPrefs.GetInt(string.Format("{0}.airplanes.{1}.unlocked", Campaign.id, type), 0) > 0;
			}
			set
			{
				PlayerPrefs.SetInt(string.Format("{0}.airplanes.{1}.unlocked", Campaign.id, type), value ? 1 : 0);
			}
		}

		public int damageIndex
		{
			get
			{
				return PlayerPrefs.GetInt(string.Format("{0}.airplanes.{1}.damage", Campaign.id, type), startDamageIndex);
			}
			set
			{
				PlayerPrefs.SetInt(string.Format("{0}.airplanes.{1}.damage", Campaign.id, type), value);
			}
		}

		public int armorIndex
		{
			get
			{
				return PlayerPrefs.GetInt(string.Format("{0}.airplanes.{1}.armor", Campaign.id, type), startArmorIndex);
			}
			set
			{
				PlayerPrefs.SetInt(string.Format("{0}.airplanes.{1}.armor", Campaign.id, type), value);
			}
		}

		public int mobilityIndex
		{
			get
			{
				return PlayerPrefs.GetInt(string.Format("{0}.airplanes.{1}.mobility", Campaign.id, type), startMobilityIndex);
			}
			set
			{
				PlayerPrefs.SetInt(string.Format("{0}.airplanes.{1}.mobility", Campaign.id, type), value);
			}
		}

		public int damageUpgradePrice
		{
			get
			{
				return PRICES[damageIndex - 1];
			}
		}

		public int armorUpgradePrice
		{
			get
			{
				return PRICES[armorIndex - 1];
			}
		}

		public int mobilityUpgradePrice
		{
			get
			{
				return PRICES[mobilityIndex - 1];
			}
		}

		public int damage
		{
			get
			{
				return Mathf.Min(damageIndex - startDamageIndex, damageUpgrades);
			}
		}

		public int armor
		{
			get
			{
				return Mathf.Min(armorIndex - startArmorIndex, armorUpgrades);
			}
		}

		public int mobility
		{
			get
			{
				return Mathf.Min(mobilityIndex - startMobilityIndex, mobilityUpgrades);
			}
		}

		public int maxDamageIndex
		{
			get
			{
				return startDamageIndex + damageUpgrades;
			}
		}

		public int maxArmorIndex
		{
			get
			{
				return startArmorIndex + armorUpgrades;
			}
		}

		public int maxMobilityIndex
		{
			get
			{
				return startMobilityIndex + mobilityUpgrades;
			}
		}

		public float damageIndexRate
		{
			get
			{
				return (float)damageIndex / 10f;
			}
		}

		public float armorIndexRate
		{
			get
			{
				return (float)armorIndex / 10f;
			}
		}

		public float mobilityIndexRate
		{
			get
			{
				return (float)mobilityIndex / 10f;
			}
		}

		public bool isMaxDamageUpgrade
		{
			get
			{
				return damageIndex == maxDamageIndex;
			}
		}

		public bool isMaxArmorUpgrade
		{
			get
			{
				return armorIndex == maxArmorIndex;
			}
		}

		public bool isMaxMobilityUpgrade
		{
			get
			{
				return mobilityIndex == maxMobilityIndex;
			}
		}

		public void UpgradeDamage(int value = 1)
		{
			damageIndex += value;
		}

		public void UpgradeArmor(int value = 1)
		{
			armorIndex += value;
		}

		public void UpgradeMobility(int value = 1)
		{
			mobilityIndex += value;
		}

		public void Unlock()
		{
			unlocked = true;
		}

		public void Reset()
		{
			PlayerPrefs.DeleteKey(string.Format("{0}.airplanes.{1}.unlocked", Campaign.id, type));
			PlayerPrefs.DeleteKey(string.Format("{0}.airplanes.{1}.damage", Campaign.id, type));
			PlayerPrefs.DeleteKey(string.Format("{0}.airplanes.{1}.armor", Campaign.id, type));
			PlayerPrefs.DeleteKey(string.Format("{0}.airplanes.{1}.mobility", Campaign.id, type));
		}
	}

	public AirplaneInfo[] factionA;

	public AirplaneInfo[] factionB;

	public static AirplaneProfiler Load()
	{
		GameObject gameObject = (GameObject)Resources.Load("AirplaneProfiler");
		return gameObject.GetComponent<AirplaneProfiler>();
	}

	public AirplaneInfo GetAirplaneInfo(int faction, AirplaneType type)
	{
		if (faction == 0)
		{
			return FindAirplane(factionA, type);
		}
		return FindAirplane(factionB, type);
	}

	public AirplaneInfo[] GetAirplanes(int faction)
	{
		if (faction == 0)
		{
			return factionA;
		}
		return factionB;
	}

	public AirplaneInfo[] GetAvailableAirplanes(int faction)
	{
		if (faction == 0)
		{
			return FindAvailableAirplanes(factionA);
		}
		return FindAvailableAirplanes(factionB);
	}

	private AirplaneInfo[] FindAvailableAirplanes(AirplaneInfo[] array)
	{
		List<AirplaneInfo> list = new List<AirplaneInfo>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].unlocked)
			{
				list.Add(array[i]);
			}
		}
		return list.ToArray();
	}

	private AirplaneInfo FindAirplane(AirplaneInfo[] array, AirplaneType type)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].type == type)
			{
				return array[i];
			}
		}
		return null;
	}

	public void ResetData()
	{
		for (int i = 0; i < factionA.Length; i++)
		{
			factionA[i].Reset();
		}
		for (int j = 0; j < factionB.Length; j++)
		{
			factionB[j].Reset();
		}
	}
}
