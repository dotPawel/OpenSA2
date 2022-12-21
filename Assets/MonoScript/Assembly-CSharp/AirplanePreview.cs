using UnityEngine;

public class AirplanePreview : MonoBehaviour
{
	public Camera previewCamera;

	public Transform platform;

	public Vector3 rotateAngle;

	public GameObject[] airplanesA;

	public GameObject[] airplanesB;

	public bool visible { get; private set; }

	public bool show
	{
		set
		{
			visible = value;
			previewCamera.enabled = visible;
		}
	}

	private void FixedUpdate()
	{
		if (visible)
		{
			platform.Rotate(rotateAngle * Time.deltaTime, Space.Self);
		}
	}

	public void SetModel(int factionIndex, int airplaneIndex)
	{
		switch (factionIndex)
		{
		case 0:
		{
			for (int k = 0; k < airplanesA.Length; k++)
			{
				airplanesA[k].gameObject.SetActiveRecursively(airplaneIndex == k);
			}
			for (int l = 0; l < airplanesB.Length; l++)
			{
				airplanesB[l].gameObject.SetActiveRecursively(false);
			}
			break;
		}
		case 1:
		{
			for (int m = 0; m < airplanesA.Length; m++)
			{
				airplanesA[m].gameObject.SetActiveRecursively(false);
			}
			for (int n = 0; n < airplanesB.Length; n++)
			{
				airplanesB[n].gameObject.SetActiveRecursively(airplaneIndex == n);
			}
			break;
		}
		default:
		{
			for (int i = 0; i < airplanesA.Length; i++)
			{
				airplanesA[i].gameObject.SetActiveRecursively(false);
			}
			for (int j = 0; j < airplanesB.Length; j++)
			{
				airplanesB[j].gameObject.SetActiveRecursively(false);
			}
			break;
		}
		}
	}
}
