using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BombLauncher : Weapon
{
	public Transform lauchPoint;

	public Mount mount;

	public int bombCount;

	public override bool ready
	{
		get
		{
			return base.ready && bombCount > 0;
		}
	}

	[method: MethodImpl(32)]
	public event Action<BombLauncher, int> onBombCountUpdated;

	public override void Fire()
	{
		if (ready)
		{
			mount.Instantiate(lauchPoint.position, lauchPoint.rotation, damage, id);
			SetBombCount(bombCount - 1);
			base.Fire();
		}
	}

	public void SetBombCount(int value)
	{
		bombCount = Mathf.Clamp(value, 0, int.MaxValue);
		if (this.onBombCountUpdated != null)
		{
			this.onBombCountUpdated(this, bombCount);
		}
	}
}
