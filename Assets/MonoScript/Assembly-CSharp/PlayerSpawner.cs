using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	public Structure airport;

	public bool supportSupply;

	public float maxSupplyRange = 10f;

	public Mark airportMark;

	private Player playerReference;

	private MarkController markController;

	private Mark mark;

	private Player player
	{
		get
		{
			if (playerReference == null)
			{
				playerReference = Player.Find();
			}
			return playerReference;
		}
	}

	[method: MethodImpl(32)]
	public event Action<PlayerSpawner, Vector3> onSupplyDelivered;

	public void SetAirport(Structure airport)
	{
		this.airport = airport;
		if (airportMark != null)
		{
			markController = MarkController.Find();
			if (markController != null)
			{
				mark = airportMark.InstantiateFromResource(Vector3.zero, Quaternion.identity);
				mark.target = airport;
				mark.SetDistanceTo(player);
				markController.AddMark(mark);
			}
		}
	}

	public void RemoveAirportMark()
	{
		if (mark != null)
		{
			markController.RemoveMark(mark);
			mark.Remove();
		}
	}

	public Airplane SpawnAirplane(AirplaneType airplaneType)
	{
		return SpawnAirplane((int)airplaneType);
	}

	public Airplane SpawnAirplane(int airplaneIndex)
	{
		Structure.SpawnLocation spawnLocation = airport.spawnLocation;
		Faction faction = Player.faction;
		return faction.SpawnAirplane(airplaneIndex, spawnLocation.position, spawnLocation.rotation);
	}

	public float AskSupply(Vector3 point)
	{
		Structure.SpawnLocation spawnLocation = airport.spawnLocation;
		if (!Utils.IsXRangeLow(point, spawnLocation.position, maxSupplyRange))
		{
			spawnLocation.position = point + (spawnLocation.position - point).normalized * maxSupplyRange;
		}
		Faction faction = Player.faction;
		Truck truck = faction.SpawnUnit(UnitType.OilTruck, spawnLocation.position, spawnLocation.rotation) as Truck;
		truck.onPathComplete += OnTruckPathComplete;
		truck.onDestinationPointUpdated += OnDestinationPointUpdated;
		truck.SetPath(new Vector3[2]
		{
			point,
			airport.spawnLocation.position
		});
		return Mathf.Abs(point.x - spawnLocation.position.x) / truck.movement.maxSpeed;
	}

	private void OnTruckPathComplete(Truck truck)
	{
		truck.onPathComplete -= OnTruckPathComplete;
		truck.onDestinationPointUpdated -= OnDestinationPointUpdated;
		truck.Destroy();
	}

	private void OnDestinationPointUpdated(Truck truck)
	{
		if (this.onSupplyDelivered != null)
		{
			this.onSupplyDelivered(this, truck.position);
		}
	}
}
