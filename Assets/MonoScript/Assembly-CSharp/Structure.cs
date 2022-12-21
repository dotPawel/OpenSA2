using System.Collections.Generic;
using UnityEngine;

public class Structure : Target
{
	public struct SpawnLocation
	{
		public Vector3 position;

		public Quaternion rotation;

		public SpawnLocation(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}
	}

	public class PositionComparer : Comparer<Structure>
	{
		public Vector3 direction;

		public bool increase;

		public PositionComparer(Vector3 direction, bool increase)
		{
			this.direction = direction;
			this.increase = increase;
		}

		public override int Compare(Structure x, Structure y)
		{
			float num = ApplyFilter(x.position, direction);
			float num2 = ApplyFilter(y.position, direction);
			return (num != num2) ? ((num < num2) ? ((!increase) ? 1 : (-1)) : (increase ? 1 : (-1))) : 0;
		}

		private float ApplyFilter(Vector3 vector, Vector3 filter)
		{
			for (int i = 0; i < 3; i++)
			{
				if (filter[i] > 0f)
				{
					return vector[i];
				}
			}
			return 0f;
		}
	}

	private const int TICK = 10;

	private const float RANGE = 2f;

	public Flag flag;

	public Vector3 spawnCenter;

	public bool modifySurfaceHeigth;

	private Vector3 spawnPosition;

	private Quaternion spawnRotation;

	public DynamicLinearBar progressBar;

	public Transform explosionLocation;

	public Effect explosionEffect;

	public SpawnLocation spawnLocation
	{
		get
		{
			return new SpawnLocation(base.position + spawnPosition, spawnRotation);
		}
	}

	private void Start()
	{
		progressBar.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
	}

	public void SetFaction(Faction faction)
	{
		base.gameObject.tag = faction.tag;
		base.gameObject.layer = faction.gameObject.layer;
		flag.material = faction.flag;
		progressBar.firstColor = faction.color;
		progressBar.SetProgress(base.healthRate);
		Scenario scenario = Scenario.Find();
		Surface surface = Surface.Find();
		Vector3 targetPoint = scenario.GetTargetPoint(faction);
		Vector3 lhs = targetPoint - base.position;
		spawnRotation = Quaternion.LookRotation((!(Vector3.Dot(lhs, Vector3.right) > 0f)) ? Vector3.left : Vector3.right, Vector3.up);
		if (modifySurfaceHeigth)
		{
			surface.UpdateHeight(base.position, base.position.y);
		}
		spawnPosition = Vector3.up * surface.GetHeight(base.position) + spawnCenter;
		faction.AddUnit(this);
	}

	public override void Hit(float damage, int targetId)
	{
		base.Hit(damage, targetId);
		progressBar.SetProgress(base.healthRate);
	}

	public override void Explode()
	{
		if (explosionEffect != null)
		{
			explosionEffect.Instantiate(explosionLocation.position, explosionLocation.rotation);
		}
		Destroy();
		base.show = false;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(base.position + spawnCenter, 0.25f);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(base.aimPoint, new Vector3(2f, 0f, 2f));
	}
}
