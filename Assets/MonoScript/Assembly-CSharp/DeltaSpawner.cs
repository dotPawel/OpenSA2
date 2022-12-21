using UnityEngine;

public class DeltaSpawner : Spawner
{
	public bool deactive;

	public bool unlimited;

	public int maxLimit;

	public int maxCount;

	public float cooldown;

	public int prespawn;

	public float step;

	protected int limit;

	protected int count;

	protected float timer;

	private void FixedUpdate()
	{
		if (!deactive && (unlimited || count < maxCount) && limit < maxLimit && (timer += Time.deltaTime) >= cooldown)
		{
			timer = 0f;
			SpawnUnit();
		}
	}

	public void SetActive(bool value)
	{
		deactive = !value;
	}

	protected void Prespawn(int value)
	{
		for (int i = 0; i < value; i++)
		{
			if (limit >= maxLimit)
			{
				break;
			}
			SpawnUnit((float)i * step);
		}
	}

	protected virtual void SpawnUnit(float offset = 0f)
	{
		limit++;
		count++;
	}

	protected void ReleaseUnit()
	{
		limit--;
		if (unlimited)
		{
			if (deactive && limit == 0)
			{
				Finish();
			}
		}
		else if ((deactive || count == maxCount) && limit == 0)
		{
			Finish();
		}
	}
}
