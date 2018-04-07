using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSpawner : MonoBehaviour {

	public GameObject[] instances;
	public float spawnRadius = 5.0f;
	public float spawnDelay = 8.0f;
	public int maxUnits = 4;
	public Unit levelTarget;

	private List<GameObject> spawned;
	private float spawnCooldown;

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), spawnRadius);
		Gizmos.DrawIcon(transform.position, "enemy");
	}

	// Use this for initialization
	void Start () {
		spawned = new List<GameObject>();
	}

	// Update is called once per frame
	void Update () {
		if (spawnCooldown > 0.0f)
			spawnCooldown -= Time.deltaTime;
		if (instances.Length > 0 && spawnCooldown <= 0) {
			if (SpawnCount() < maxUnits) {
				Spawn();
			} else {
				spawnCooldown = 0.5f;
			}
		}
	}

	public void Spawn() {
		Vector3 position = transform.position;
		float randomAlpha = Random.Range(0, Mathf.PI * 2);
		float randomRange = Random.Range(0, spawnRadius);
		position.x += randomRange * Mathf.Cos(randomAlpha);
		position.z += randomRange * Mathf.Sin(randomAlpha);
		GameObject newInstance = GameObject.Instantiate(instances[Random.Range(0, 2)], position, Quaternion.identity);
		Unit unit = newInstance.GetComponent<Unit>();
		if (unit && levelTarget) {
			while (unit.level < levelTarget.level)
				unit.LevelUp();
		}
		spawned.Add(newInstance);
		spawnCooldown = spawnDelay;
	}

	int SpawnCount() {
		int total = 0;
		for (int i = spawned.Count - 1; i >= 0; i--) {
			if (spawned[i])
				total++;
			else
				spawned.RemoveAt(i);
		}
		return (total);
	}
}
