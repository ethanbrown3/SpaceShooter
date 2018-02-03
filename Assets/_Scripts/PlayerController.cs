using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 50.0f;
	public Rigidbody head;
	public LayerMask layerMask;
    public Animator bodyAnimator;
    public float[] hitForce;
    public float timeBetweenHits = 2.5f;
    public Rigidbody marineBody;

    private bool isDead = false;
    private bool isHit = false;
    private float timeSinceHit = 0;
    private int hitNumber = -1;
	private Vector3 currentLookTarget;
	private CharacterController characterController;
    private DeathParticles deathParticles;

	void Start () {
		characterController = GetComponent<CharacterController>();
        deathParticles = gameObject.GetComponentInChildren<DeathParticles>();
	}
	// Update is called once per frame
	void Update () {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);

        if (isHit) {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > timeBetweenHits) {
                isHit = false;
                timeSinceHit = 0;
            }
        }
    }

	// Gaurenteed to be called at consistent intervals regardless of framerate
	// Used for physics
	void FixedUpdate() {
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		if (moveDirection == Vector3.zero) {
            bodyAnimator.SetBool("isPlayerMoving", false);
		} else {
			head.AddForce(transform.right * 150, ForceMode.Acceleration);
            bodyAnimator.SetBool("isPlayerMoving", true);
        }

        RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin, ray.direction*1000, Color.green);

		if (Physics.Raycast(ray, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore)) {
			// 1
			Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
			// 2
			Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
			// 3
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
		}
	}

    void OnTriggerEnter(Collider other) {
        Debug.Log("Entered Player Collider");
        Alien alien = other.gameObject.GetComponent<Alien>(); 
        if (alien != null) { // Did an alien collide with the player
            if (!isHit) {
                hitNumber += 1; // add a hit to the player
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                if (hitNumber < hitForce.Length) { // hero is still alive so shake the camera
                    cameraShake.intensity = hitForce[hitNumber];
                    cameraShake.Shake();
                } else {
                    Die();
                }
                isHit = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt);
            }
            alien.Die();
        }
    }

    public void Die() {
        bodyAnimator.SetBool("isPlayerMoving", false);
        marineBody.transform.parent = null;
        marineBody.isKinematic = false;
        marineBody.useGravity = true;
        marineBody.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        marineBody.gameObject.GetComponent<Gun>().enabled = false;
        Destroy(head.gameObject.GetComponent<HingeJoint>());
        head.transform.parent = null;
        head.useGravity = true;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.marineDeath);
        deathParticles.Activate();
        Destroy(gameObject);

    }
}
