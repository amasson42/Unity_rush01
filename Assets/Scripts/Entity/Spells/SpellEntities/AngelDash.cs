using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelDash : SpellEntity {

    public GameObject sight;
    float timer;
    bool dashing;
    float totalDistance;
    float savedVerticalAngle;
    float savedHorizontalAngle;
    Collider casterCollider;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        savedVerticalAngle = PlayerEntityController.instance.followCamera.verticalAngle;
        savedHorizontalAngle = PlayerEntityController.instance.followCamera.horizontalAngle;
        PlayerEntityController.instance.followCamera.offset = new Vector3(0, 3, 0);
        timer = 0;
        dashing = false;
        caster.OrderStopMove();
        caster.RetainMove();
        caster.agent.enabled = false;
        casterCollider = caster.GetComponent<Collider>();
        if (casterCollider)
            casterCollider.enabled = false;
        sight.transform.position = Camera.main.transform.position;
        sight.transform.rotation = Camera.main.transform.rotation;
        sight.transform.SetParent(Camera.main.transform);
	}

	// Update is called once per frame
	void Update () {
        if (timer < 1.0f && !dashing) {
            // jumping in the air
            caster.transform.position += new Vector3(0, 100.0f * Time.deltaTime, 0);
            timer += Time.deltaTime;
        } else if (!dashing) {
            // looking for a ground target
            sight.SetActive(true);
            PlayerEntityController.instance.followCamera.verticalAngle += Input.GetAxis("Mouse X");
            PlayerEntityController.instance.followCamera.horizontalAngle -= Input.GetAxis("Mouse Y");
            var hit = PlayerEntityController.GetClickedRaycast();
            caster.transform.LookAt(hit.point);
            if (hit.collider) {
                if (Input.GetMouseButtonDown(0)) {
                    timer = 0;
                    sight.SetActive(false);
                    dashing = true;
                    pointTarget = hit.point;
                    transform.position = caster.transform.position;
                    totalDistance = Vector3.Distance(transform.position, pointTarget);
                    PlayerEntityController.instance.followCamera.offset = Vector3.zero;
                }
            }
        } else {
            timer += Time.deltaTime;
            caster.transform.position = Vector3.MoveTowards(transform.position, pointTarget, timer * totalDistance);
            // dashing to the ground
            if (timer >= 1.0f) {
                caster.ReleaseMove();
                Cursor.lockState = CursorLockMode.None;
                PlayerEntityController.instance.followCamera.verticalAngle = savedVerticalAngle;
                PlayerEntityController.instance.followCamera.horizontalAngle = savedHorizontalAngle;
                caster.agent.enabled = true;
                casterCollider.enabled = true;
                Destroy(gameObject);
            }
        }
	}

	public override bool CanBeCastedBy(SpellCaster caster, out string error) {
		if (caster.caster.unit && caster.caster.unit.currentHealth < caster.caster.unit.maxHealth) {
			error = "life is not full";
			return false;
		}
		error = "";
		return true;
	}
}
