using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : AObject
{
	public Vector2 size = new Vector2(2f, 1f);

	public int rows = 3;

	public int cols = 3;

	public Interval speed;

	public Interval height;

	public float width;

	public float[] spawnDelays;

	public Material material;

	public float[] depths;

	public bool prespawn = true;

	private Mesh[] meshes;

	private Stack<CloudParticle> pool = new Stack<CloudParticle>();

	private List<CloudParticle> clouds = new List<CloudParticle>();

	private float timer;

	private float spawnDelay;

	private int depthIndex;

	private float releaseWidth;

	private Vector3 spawnPoint
	{
		get
		{
			Vector3 result = base.position + Vector3.left * width / 2f;
			result.y = height.random;
			result.z = depths[depthIndex++ % depths.Length];
			return result;
		}
	}

	private void Start()
	{
		spawnDelay = spawnDelays[Scenario.setting];
		meshes = new Mesh[rows * cols];
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Vector4 uv = new Vector4((float)i * 1f / (float)rows, (float)j * 1f / (float)cols, (float)(i + 1) * 1f / (float)rows, (float)(j + 1) * 1f / (float)cols);
				meshes[i * cols + j] = MeshBuilder.CreatePlane(size, uv);
			}
		}
		releaseWidth = base.position.x + width / 2f;
		if (prespawn)
		{
			for (float num = 0f; num < width; num += speed.random * spawnDelay)
			{
				SpawnCloud(num);
			}
		}
	}

	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		if ((timer -= deltaTime) <= 0f)
		{
			timer = spawnDelay;
			SpawnCloud();
		}
		int num = 0;
		while (num < clouds.Count)
		{
			CloudParticle cloudParticle = clouds[num];
			cloudParticle.position += Vector3.right * cloudParticle.speed * deltaTime;
			if (cloudParticle.position.x > releaseWidth)
			{
				clouds.RemoveAt(num);
				pool.Push(cloudParticle);
				cloudParticle.Show(false);
			}
			else
			{
				num++;
			}
		}
	}

	private void SpawnCloud(float offset = 0f)
	{
		if (pool.Count > 0)
		{
			CloudParticle cloudParticle = pool.Pop();
			cloudParticle.Show(true);
			cloudParticle.position = spawnPoint + Vector3.right * offset;
			cloudParticle.speed = speed.random;
			clouds.Add(cloudParticle);
		}
		else
		{
			Vector3 vector = spawnPoint + Vector3.right * offset;
			CloudParticle cloudParticle2 = CloudParticle.Create(vector, base.rotation, base.goTransform);
			cloudParticle2.material = material;
			cloudParticle2.mesh = meshes[Random.Range(0, meshes.Length)];
			cloudParticle2.speed = speed.random;
			clouds.Add(cloudParticle2);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(Vector3.up * height.Lerp(0.5f), new Vector3(width, height.max - height.min, 0f));
	}
}
