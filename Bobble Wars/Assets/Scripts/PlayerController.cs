using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 50.0f;

	// Update is called once per frame
	void Update () {
		
		Vector3 pos = transform.position;
		pos.x += moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		pos.z += moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		transform.position = pos;

	}
}
