using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour {

    public Transform target;
    public float navigationUpdate;
    public UnityEvent OnDestroy;

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
        Die();
    }

    public void Die() {
        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
        Destroy(gameObject);
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);

    }
}
