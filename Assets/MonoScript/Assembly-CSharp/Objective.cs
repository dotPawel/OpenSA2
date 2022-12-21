using System;
using System.Runtime.CompilerServices;

public class Objective : AObject
{
	public bool completed { get; private set; }

	[method: MethodImpl(32)]
	public event Action<Objective> onCompleted;

	public virtual void Init(Scenario scenario)
	{
	}

	public virtual void Complete()
	{
		completed = true;
		if (this.onCompleted != null)
		{
			this.onCompleted(this);
		}
	}
}
