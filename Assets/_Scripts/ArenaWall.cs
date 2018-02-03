using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaWall : MonoBehaviour {
    private Animator arenaAnimator;

	// Use this for initialization
	void Start () {
        GameObject arena = transform.parent.gameObject;
        arenaAnimator = arena.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter (Collider other) {
        if (other.GetComponent<PlayerController>())
            arenaAnimator.SetBool("IsLowered", true);
    }

    private void OnTriggerExit (Collider other) {
        if (other.GetComponent<PlayerController>())
            arenaAnimator.SetBool("IsLowered", false);
    }
}
