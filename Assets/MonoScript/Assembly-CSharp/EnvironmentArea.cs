using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentArea : AObject
{
	[Serializable]
	public class Setting
	{
		public EnvironmentBlock[] blocks;
	}

	public Setting[] settings;

	public int blockCount;

	public float blockSize;

	public float repeatRate;

	public bool generateAtStart;

	private void Start()
	{
		if (generateAtStart)
		{
			Generate();
		}
	}

	[ContextMenu("Generate")]
	public void Generate()
	{
		Clear();
		float num = blockSize * (float)(blockCount - 1) / 2f;
		Dictionary<EnvironmentBlock, float> dictionary = new Dictionary<EnvironmentBlock, float>();
		List<EnvironmentBlock> list = new List<EnvironmentBlock>();
		EnvironmentBlock[] blocks = settings[Scenario.setting].blocks;
		for (int i = 0; i < blockCount; i++)
		{
			Vector3 vector = base.position - Vector3.right * (num - (float)i * blockSize);
			list.Clear();
			foreach (EnvironmentBlock environmentBlock in blocks)
			{
				if (dictionary.ContainsKey(environmentBlock) && dictionary[environmentBlock] > 0f)
				{
					dictionary[environmentBlock] = Mathf.Clamp01(dictionary[environmentBlock] - Mathf.Max(repeatRate, 1f / (float)(blocks.Length - 1)));
				}
				else
				{
					list.Add(environmentBlock);
				}
			}
			EnvironmentBlock environmentBlock2 = list[UnityEngine.Random.Range(0, list.Count)];
			if (dictionary.ContainsKey(environmentBlock2))
			{
				dictionary[environmentBlock2] = 1f;
			}
			else
			{
				dictionary.Add(environmentBlock2, 1f);
			}
			EnvironmentBlock environmentBlock3 = environmentBlock2.Instantiate(vector);
			environmentBlock3.parent = base.goTransform;
		}
	}

	[ContextMenu("Clear")]
	public void Clear()
	{
		while (base.transform.childCount > 0)
		{
			UnityEngine.Object.DestroyImmediate(base.transform.GetChild(0).gameObject);
		}
	}

	private void OnDrawGizmosSelected()
	{
		float num = blockSize * (float)(blockCount - 1) / 2f;
		Gizmos.color = Color.yellow;
		for (int i = 0; i < blockCount; i++)
		{
			Vector3 center = base.position - Vector3.right * (num - (float)i * blockSize);
			Gizmos.DrawWireCube(center, new Vector3(blockSize, 0f, blockSize));
		}
	}
}
