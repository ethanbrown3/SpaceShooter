using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject alien;
    public int maxAliensOnScreen;
    public int maxAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;
    public GameObject upgradePrefab;
    public Gun gun;
    public float upgradeMaxTimeSpawn = 7.5f;

    private int aliensOnScreen = 0;
    private int totalAliensSpawned = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;
    private bool spawnedUpgrade = false;
    private float actualUpgradeTime = 0;
    private float currentUpgradeTime = 0;

	// Use this for initialization
	void Start () {
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f, upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
	}
	
	// Update is called once per frame
	void Update () {
        currentUpgradeTime += Time.deltaTime;

        if (currentUpgradeTime > actualUpgradeTime) {
            if (!spawnedUpgrade) { // After random time passes, check if upgrade has spawned
                // The upgrade will appear in one of the alien spawn points.
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                // This section handles spawning and associating the gun with it.
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                // set spawned flag
                spawnedUpgrade = true;

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        aliensOnScreen = GameObject.FindGameObjectsWithTag("Alien").Length;

        currentSpawnTime += Time.deltaTime;
        if (currentSpawnTime > generatedSpawnTime) {
            currentSpawnTime = 0;
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            if ((aliensPerSpawn > 0) && (totalAliensSpawned < maxAliens)) {
                List<int> previousSpawnLocations = new List<int>();
                if (aliensPerSpawn > spawnPoints.Length) {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }
                aliensPerSpawn = (aliensPerSpawn > maxAliens) ? aliensPerSpawn - maxAliens : aliensPerSpawn;
                // spawn the aliens
                for (int i = 0; i < aliensPerSpawn; i++) {
                    if (aliensOnScreen < maxAliensOnScreen) {
                        totalAliensSpawned++;
                        int spawnPoint = -1;
                        while (spawnPoint == -1) {
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            if (!previousSpawnLocations.Contains(randomNumber)) {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }
                        GameObject spawnLocation = spawnPoints[spawnPoint];
                        GameObject newAlien = Instantiate(alien) as GameObject;
                        newAlien.transform.position = spawnLocation.transform.position;

                        Alien alienScript = newAlien.GetComponent<Alien>();
                        alienScript.target = player.transform;
                        Vector3 targetRotation = new Vector3(player.transform.position.x, newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);
       
                    }
                }
            }
        }

        
       
	}
}
