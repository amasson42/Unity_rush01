using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenStrike : SpellEntity {

	public MoltenStrike_meteor meteorPrefab;
	float duration = 2.0f;

	// Use this for initialization
	void Start () {
		transform.position = pointTarget;
		SpawnMeteor();
		SpawnMeteor();
		SpawnMeteor();
	}
	
	// Update is called once per frame
	void Update () {
		duration -= Time.deltaTime;
		if (duration <= 0)
			Destroy(gameObject);
	}

	void SpawnMeteor() {
		GameObject meteor = GameObject.Instantiate(meteorPrefab.gameObject, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
		MoltenStrike_meteor meteorScript = meteor.GetComponent<MoltenStrike_meteor>();
		meteorScript.caster = caster;
		Rigidbody meteorBody = meteor.GetComponent<Rigidbody>();

		Vector3 randomPointTarget = pointTarget;
		float randomAlpha = Random.Range(0, Mathf.PI * 2);
		float randomRange = Random.Range(2.0f, 3.0f);
		randomPointTarget.x += randomRange * Mathf.Cos(randomAlpha);
		randomPointTarget.z += randomRange * Mathf.Sin(randomAlpha);
		randomPointTarget.y += 3.0f;

		meteorBody.AddForce((randomPointTarget - transform.position) * 2.0f, ForceMode.Impulse);
	}

	public override bool CanBeCastedBy(SpellCaster caster, out string error) {
		error = "";
		return true;
	}

}
