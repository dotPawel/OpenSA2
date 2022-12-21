using UnityEngine;

public class QualityConfigure : MonoBehaviour
{
	public int iOSLowQualityLevel = 6;

	public int iOSHighQualityLevel = 7;

	public int androidQualityLevel = 8;

	private void Awake()
	{
		QualitySettings.SetQualityLevel(androidQualityLevel, true);
	}
}
