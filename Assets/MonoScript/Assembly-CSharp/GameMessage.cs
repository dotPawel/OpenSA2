using UnityEngine;

public class GameMessage : AObject
{
	public UIText textView;

	public UIView rectView;

	public float time;

	public float fadeTime;

	private float timer;

	public string text
	{
		set
		{
			textView.text = value;
		}
	}

	public int layer
	{
		set
		{
			textView.gameObject.layer = value;
		}
	}

	private void FixedUpdate()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			if (timer < fadeTime)
			{
				float alpha = timer / fadeTime;
				textView.alpha = alpha;
				rectView.alpha = alpha;
			}
			if (timer <= 0f)
			{
				textView.show = false;
				rectView.show = false;
			}
		}
	}

	public void Show(string text, Vector3 point)
	{
		base.position = point;
		textView.text = text;
		timer = time;
		textView.alpha = 1f;
		rectView.alpha = 1f;
		textView.show = true;
		rectView.show = true;
	}

	public GameMessage Instantiate(Vector3 position)
	{
		GameObject gameObject = SceneResources.Pop(base.gameObject, position, Quaternion.identity);
		gameObject.SetActiveRecursively(true);
		return gameObject.GetComponent<GameMessage>();
	}

	public void Remove()
	{
		base.gameObject.SetActiveRecursively(false);
		SceneResources.Push(base.gameObject);
	}
}
