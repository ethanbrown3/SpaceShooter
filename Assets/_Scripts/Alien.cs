using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour {

    public Transform target;
    public float navigationUpdate;
    public UnityEvent OnDestroy;
    public Rigidbody head;
    public bool isAlive = true;

    private DeathParticles deathParticles;
    private float navigationTime = 0;
    private NavMeshAgent agent;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
        navigationTime += Time.deltaTime;
        if (navigationTime > navigationUpdate) {
            if (target != null)
                agent.destination = target.position;
            navigationTime = 0;
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Bullet>()) {
            if (isAlive)
                Die();
        }
    }

    public DeathParticles GetDeathParticles() {
        if (deathParticles == null) {
            deathParticles = GetComponentInChildren<DeathParticles>();
        }
        return deathParticles;
    }

    public void Die() {
        isAlive = false;
        head.GetComponent<Animator>().enabled = false;
        head.isKinematic = false;
        head.useGravity = true;
        head.GetComponent<SphereCollider>().enabled = true;
        head.gameObject.transform.parent = null;
        head.velocity = new Vector3(0, 26.0f, 3.0f);
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);

        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
        head.GetComponent<SelfDestruct>().Initiate();
        if (deathParticles) {
            deathParticles.transform.parent = null;
            deathParticles.Activate();
        }
        Destroy(gameObject);

    }
}
