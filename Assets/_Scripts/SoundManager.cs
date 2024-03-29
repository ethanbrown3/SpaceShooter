﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance = null;

    public AudioClip 
        gunFire,
        upgradedGunFire,
        hurt,
        alienDeath,
        marineDeath,
        victory,
        elevatorArrived,
        powerUpPickup,
        powerUpAppear;
    

    private AudioSource soundEffectAudio;

	// Use this for initialization
	void Start () {
		if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource source in sources) {
            if (source.clip == null) {
                soundEffectAudio = source;
            }
        }
	}

    public void PlayOneShot (AudioClip clip) {
        soundEffectAudio.PlayOneShot(clip);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
