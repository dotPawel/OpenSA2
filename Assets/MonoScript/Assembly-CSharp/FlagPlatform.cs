using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlagPlatform : AObject
{
	private const int TICK = 10;

	public Faction faction;

	public Flag flag;

	public float captureRange = 0.5f;

	private bool captured;

	[method: MethodImpl(32)]
	public event Action<FlagPlatform> onCaptured;

	public void SetFaction(Faction value)
	{
		faction = value;
		captured = false;
		flag.material = faction.flag;
	}

	private void FixedUpdate()
	{
		if (captured || !(faction != null) || !GetTick(10))
		{
			return;
		}
		for (int i = 0; i < faction.enemies.Count; i++)
		{
			Target target = faction.enemies[i];
			if (target.captureArea && Utils.IsXRangeLow(base.position, target.position, captureRange))
			{
				Captrure(faction.enemy);
				break;
			}
		}
	}

	private void Captrure(Faction faction)
	{
		captured = true;
		flag.material = faction.flag;
		if (this.onCaptured != null)
		{
			this.onCaptured(this);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.position, captureRange);
	}
}
