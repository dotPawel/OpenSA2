using UnityEngine;

public class EnvironmentBlock : AObject
{
	public bool randomRotate = true;

	public float size = 5f;

	public EnvironmentBlock Instantiate(Vector3 position)
	{
		return (EnvironmentBlock)Object.Instantiate(this, position, (!randomRotate) ? Quaternion.identity : Quaternion.Euler(Vector3.up * Random.Range(0, 4) * 90f));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(base.position, new Vector3(size, 0f, size));
	}
}
