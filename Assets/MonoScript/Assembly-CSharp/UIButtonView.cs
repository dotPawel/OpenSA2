using UnityEngine;

public abstract class UIButtonView : MonoBehaviour
{
	public virtual void Press(bool value)
	{
	}

	public virtual void Disable(bool value)
	{
	}

	public virtual void Show(bool value)
	{
	}

	public virtual void Select(bool value)
	{
	}

	public virtual void Blink(bool value)
	{
	}
}
