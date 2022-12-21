using UnityEngine;

public class GameMessenger : AObject
{
	private static GameMessenger gameMessengerSource;

	public GameMessage gameMessage;

	private GameMessage gameMessageReference;

	private static GameMessenger gameMessenger
	{
		get
		{
			if (gameMessengerSource == null)
			{
				gameMessengerSource = (GameMessenger)Object.FindObjectOfType(typeof(GameMessenger));
			}
			return gameMessengerSource;
		}
	}

	public static void Message(string text, Vector2 position)
	{
		if (gameMessenger != null)
		{
			gameMessenger.PushMessage(text, position);
		}
	}

	public void PushMessage(string text, Vector2 position)
	{
		Vector3 point = base.position + new Vector3(position.x, position.y, 0f);
		if (gameMessageReference == null)
		{
			gameMessageReference = gameMessage.Instantiate(point);
		}
		gameMessageReference.Show(text, point);
	}
}
