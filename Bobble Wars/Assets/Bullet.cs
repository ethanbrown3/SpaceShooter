using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		Destroy(gameObject);
	}
}
