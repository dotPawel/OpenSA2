using UnityEngine;

public class CaravanObjective : Objective
{
	public int truckCount;

	private int destroyedCount;

	public ForcesSpawner forcesSpawner;

	public Vector3 destinationPoint;

	private Vector2 gameMessageLocation = new Vector2(0f, 220f);

	private void Awake()
	{
		forcesSpawner.onUnitSpawned += OnUnitSpawned;
	}

	private void OnUnitSpawned(Unit unit)
	{
		Truck truck = unit as Truck;
		if (truck != null)
		{
			truck.onPathComplete += OnTruckPathComplete;
			truck.onDestroyed += OnTruckDestroyed;
			truck.SetPath(new Vector3[1] { destinationPoint });
		}
	}

	private void OnTruckDestroyed(Target target)
	{
		Truck truck = target as Truck;
		truck.onDestinationPointUpdated -= OnTruckPathComplete;
		truck.onDestroyed -= OnTruckDestroyed;
		destroyedCount++;
		if (destroyedCount == truckCount)
		{
			Complete();
		}
		else if (!base.completed)
		{
			GameMessenger.Message(string.Format("{0}/{1} TRUCKS DESTROYED", destroyedCount, truckCount), gameMessageLocation);
		}
	}

	private void OnTruckPathComplete(Truck truck)
	{
		truck.onDestinationPointUpdated -= OnTruckPathComplete;
		truck.onDestroyed -= OnTruckDestroyed;
		truck.Destroy();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(destinationPoint, 1f);
	}
}
