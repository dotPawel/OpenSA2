using UnityEngine;

public class TutorialTarget : Unit
{
	public Effect explosionEffect;

	public Transform model;

	public bool drift;

	public float driftSpeed = 1f;

	public float driftScale = 1f;

	private float timer;

	private float defaulHeight;

	private void OnEnable()
	{
		defaulHeight = base.position.y;
		RestoreHealth();
	}

	private void FixedUpdate()
	{
		if (drift)
		{
			Vector3 vector = base.position;
			timer += Time.deltaTime * driftSpeed;
			vector.y = defaulHeight + Mathf.Sin(timer) * driftScale;
			base.position = vector;
		}
	}

	public void SetFaction(Faction faction)
	{
		base.gameObject.tag = faction.tag;
		base.gameObject.layer = faction.gameObject.layer;
		faction.AddUnit(this);
	}

	public override void Explode()
	{
		if (explosionEffect != null)
		{
			explosionEffect.Instantiate(model.position, model.rotation);
		}
		Destroy();
	}

	public override void Destroy()
	{
		base.Destroy();
		Remove();
	}
}
