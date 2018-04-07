using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Follow camera is the behavior of making the camera following a player target.
	It will automaticaly place the camera at the position of the player with
		a specific distance and angular offset.
	TODO: create rayCastWatcher wich avoid letting obstacles between the camera and the target
 */

public class FollowCamera : MonoBehaviour {

	public Camera cameraNode;

	public GameObject visualTarget;

	public float cameraHeightDistance = 10.0f;
	public float horizontalAngle = 30.0f;
	public float verticalAngle = 0.0f;
	public float rollAngle = 0.0f;
	public float moveSpeed = 10.0f;
	public float angularSpeed = 180.0f;
	public bool rayCastWatcher = false;

	void Start () {
		if (cameraNode == null)
			cameraNode = GetComponentInChildren<Camera>();
		if (visualTarget)
			transform.position = visualTarget.transform.position;
		if (cameraNode)
			cameraNode.transform.localPosition = new Vector3(0, 0, -cameraHeightDistance);
		transform.localEulerAngles = new Vector3(horizontalAngle, verticalAngle, rollAngle);
	}
	
	void Update () {
		if (horizontalAngle > 90.0f)
			horizontalAngle = 90.0f;
		else if (horizontalAngle < 0.0f)
			horizontalAngle = 0.0f;
		if (verticalAngle > 360.0f)
			verticalAngle = 360.0f;
		else if (verticalAngle < -360.0f)
			verticalAngle = -360.0f;
		if (visualTarget)
			transform.position = Vector3.MoveTowards(transform.position, visualTarget.transform.position, moveSpeed * Time.deltaTime);
		if (cameraNode)
			cameraNode.transform.localPosition = Vector3.MoveTowards(cameraNode.transform.localPosition, new Vector3(0, 0, -cameraHeightDistance), moveSpeed * Time.deltaTime);
		transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, new Vector3(horizontalAngle, verticalAngle, rollAngle), angularSpeed * Time.deltaTime);
	}
}
