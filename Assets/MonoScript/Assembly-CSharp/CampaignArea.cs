using System.Collections.Generic;
using UnityEngine;

public class CampaignArea : AObject
{
	public class ConnectionComparer : Comparer<CampaignArea>
	{
		public CampaignFaction faction;

		public ConnectionComparer(CampaignFaction faction)
		{
			this.faction = faction;
		}

		public override int Compare(CampaignArea areaA, CampaignArea areaB)
		{
			int num = 0;
			int num2 = 0;
			foreach (CampaignArea connection in areaA.connections)
			{
				if (connection.faction == faction)
				{
					num++;
				}
			}
			foreach (CampaignArea connection2 in areaB.connections)
			{
				if (connection2.faction == faction)
				{
					num2++;
				}
			}
			return (num != num2) ? ((num >= num2) ? 1 : (-1)) : 0;
		}
	}

	private const float RADIUS = 0.2f;

	public List<CampaignArea> connections;

	public CampaignFaction faction;

	private Mission missionCache;

	public Mission mission
	{
		get
		{
			if (missionCache == null)
			{
				missionCache = GetComponent<Mission>();
			}
			return missionCache;
		}
	}

	public void SetFaction(CampaignFaction value)
	{
		faction = value;
		base.GetComponent<Renderer>().sharedMaterial = faction.flag;
	}

	public void Connect(CampaignArea[] array)
	{
		foreach (CampaignArea campaignArea in array)
		{
			if (campaignArea != this)
			{
				Connect(campaignArea);
			}
		}
	}

	public void Disconnect(CampaignArea[] array)
	{
		foreach (CampaignArea campaignArea in array)
		{
			if (campaignArea != this)
			{
				Disconnect(campaignArea);
			}
		}
	}

	public void Connect(CampaignArea area)
	{
		if (!connections.Contains(area))
		{
			connections.Add(area);
		}
		if (!area.connections.Contains(this))
		{
			area.connections.Add(this);
		}
	}

	public void Disconnect(CampaignArea area)
	{
		if (connections.Contains(area))
		{
			connections.Remove(area);
		}
		if (area.connections.Contains(this))
		{
			area.connections.Remove(this);
		}
	}

	public CampaignArea GetConnectedAreaInRange(CampaignFaction faction, float minRange = 0f)
	{
		CampaignArea result = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < connections.Count; i++)
		{
			CampaignArea campaignArea = connections[i];
			if (campaignArea.faction == faction)
			{
				float sqrMagnitude = (campaignArea.position - base.position).sqrMagnitude;
				if (sqrMagnitude > minRange * minRange && sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = campaignArea;
				}
			}
		}
		return result;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = ((connections.Count <= 0) ? Color.white : Color.yellow);
		Gizmos.DrawSphere(base.position, 0.2f);
		Gizmos.color = Color.cyan;
		for (int i = 0; i < connections.Count; i++)
		{
			Gizmos.DrawLine(base.position, connections[i].position);
		}
	}
}
