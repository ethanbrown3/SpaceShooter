using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour {

    public Transform target;
    public float navigationUpdate;

    private float navigationTime = 0;
    private NavMeshAgent agent;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
        navigationTime += Time.deltaTime;
        if (navigationTime > navigationUpdate) {
            agent.destination = target.position;
            navigationTime = 0;
        }
	}

    void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
    }
}
