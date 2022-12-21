using UnityEngine;

public class Boot : MonoBehaviour
{
	public Patch[] patches;

	public string sceneName;

	public bool debugMode;

	private void Start()
	{
		Game.debugMode = debugMode;
		for (int i = 0; i < patches.Length; i++)
		{
			patches[i].Apply();
		}
		LoadLevel(sceneName);
	}

	public void LoadLevel(string levelName)
	{
		Application.LoadLevel(levelName);
	}
}
