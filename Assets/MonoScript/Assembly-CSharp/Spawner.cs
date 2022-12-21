using System;
using System.Runtime.CompilerServices;

public class Spawner : AObject
{
	protected Faction faction;

	[method: MethodImpl(32)]
	public event Action<Spawner> onFinished;

	public virtual void SetFaction(Faction value)
	{
		faction = value;
	}

	protected void Finish()
	{
		if (this.onFinished != null)
		{
			this.onFinished(this);
		}
	}
}
