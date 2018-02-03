using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform launchPosition;
	public float fireRate = 0.1f;
    public bool isUpgraded;
    public float upgradeDuration = 10.0f;

    private float timeUpgraded;
    private float nextFire = 0.0f; // make sure you can fire again until time has passed this value
    private AudioSource audioSource;

    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && Time.time > nextFire) {
            fireBullet();
        }

        timeUpgraded += Time.deltaTime;
        if (timeUpgraded > upgradeDuration && isUpgraded == true) {
            isUpgraded = false;
        }
	}

    public void UpgradeGun() {
        isUpgraded = true;
        timeUpgraded = 0;
    }

    void fireBullet() {
		nextFire = Time.time + fireRate; // update the time we can fire again
        Rigidbody bullet = createBullet();
		bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * 100;
        if (isUpgraded) {
            Rigidbody bullet2 = createBullet();
            bullet2.velocity = (transform.right + transform.forward / 0.5f) * 100;
            Rigidbody bullet3 = createBullet();
            bullet3.velocity = ((transform.right * -1) + transform.forward / 0.5f) * 100;
        }

        if (isUpgraded) {
            audioSource.PlayOneShot(SoundManager.Instance.upgradedGunFire);
        } else {
            audioSource.PlayOneShot(SoundManager.Instance.gunFire);
        }
    }

    private Rigidbody createBullet() {
        GameObject bullet = Instantiate(bulletPrefab, launchPosition.position, launchPosition.rotation);
        bullet.transform.position = launchPosition.position;
        return bullet.GetComponent<Rigidbody>();
    }
}

