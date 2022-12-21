using UnityEngine;

public class CampaignFaction : MonoBehaviour
{
	private const float RADIUS = 0.5f;

	public CampaignFaction enemy;

	public CampaignArea[] controlAreas;

	public CampaignArea[] centerAreas;

	public Material flag;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		for (int i = 0; i < controlAreas.Length; i++)
		{
			if (controlAreas[i] != null)
			{
				Gizmos.DrawWireSphere(controlAreas[i].position, 0.5f);
			}
		}
	}
}
