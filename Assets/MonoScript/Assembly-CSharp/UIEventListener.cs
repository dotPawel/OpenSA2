using UnityEngine;

public interface UIEventListener
{
	int order { get; }

	bool Contains(Ray ray);

	bool SetEvent(UITouch touch);
}
