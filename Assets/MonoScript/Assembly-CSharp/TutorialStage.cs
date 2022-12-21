using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialStage : MonoBehaviour
{
	public int titleIndex;

	[method: MethodImpl(32)]
	public event Action<TutorialStage> onCompleted;

	public virtual void Init(Tutorial tutorial)
	{
	}

	public virtual void Complete()
	{
		if (this.onCompleted != null)
		{
			this.onCompleted(this);
		}
	}
}
