using UnityEngine;

public class PlatformUIConfigure : MonoBehaviour
{
	public Camera[] cameras;

	public int minHeight = 320;

	public int maxHeight = 384;

	public int maxWidth = 1136;

	private void Awake()
	{
		float num = Screen.height;
		if (Screen.width > maxWidth)
		{
			float num2 = (float)Screen.width / num;
			num = (float)maxWidth / num2;
		}
		float orthographicSize = Mathf.Clamp(num / 2f, minHeight, maxHeight);
		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].orthographicSize = orthographicSize;
		}
	}
}
