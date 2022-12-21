using UnityEngine;

public class Obstacle : Target
{
	public DynamicLinearBar progressBar;

	public Transform explosionLocation;

	public Effect explosionEffect;

	private void Start()
	{
		progressBar.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
	}

	public void SetFaction(Faction faction)
	{
		base.gameObject.tag = faction.tag;
		base.gameObject.layer = faction.gameObject.layer;
		progressBar.firstColor = faction.color;
		progressBar.SetProgress(base.healthRate);
		Surface surface = Surface.Find();
		Vector3 point = base.position;
		point.y = surface.GetHeight(point);
		base.position = point;
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
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(base.aimPoint, new Vector3(2f, 0f, 2f));
	}
}
