using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform launchPosition;
	public float fireRate = 0.1f;

	private float nextFire = 0.0f; // make sure you can fire again until time has passed this value
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && Time.time > nextFire) {
            fireBullet();
        }
	}
	
	void fireBullet() {
		nextFire = Time.time + fireRate; // update the time we can fire again
		GameObject bullet = Instantiate(bulletPrefab, launchPosition.position, launchPosition.rotation);
		bullet.transform.position = launchPosition.position;
		bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * 100;
	}
}
