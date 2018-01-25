using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 50.0f;
	public Rigidbody head;
	private CharacterController characterController;

	void Start () {
		characterController = GetComponent<CharacterController>();
	}
	// Update is called once per frame
	void Update () {
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		characterController.SimpleMove(moveDirection * moveSpeed);
	}

	// Gaurenteed to be called at consistent intervals regardless of framerate
	// Used for physics
	void FixedUpdate() {
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		if (moveDirection == Vector3.zero) {
			// TODO
		} else {
			head.AddForce(transform.right * 150, ForceMode.Acceleration);
		}
	}
}
