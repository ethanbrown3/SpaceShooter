using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject alien;
    public int maxAliensOnScreen;
    public int totalAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;
    public GameObject upgradePrefab;
    public Gun gun;
    public int upgradeMaxTimeSpawn = 10;
    public int maxSpawnedUpgrades = 1;

    private int aliensKilled = 0;
    private int aliensOnScreen = 0;
    private int totalAliensSpawned = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;
    private bool isPickupSpawned = false;
    private float actualUpgradeTime = 0;
    private float currentTime = 0;

	// Use this for initialization
	void Start () {
        ResetPickupActualSpawnTime();
    }
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        isPickupSpawned = GameObject.FindGameObjectsWithTag("Pickup").Length >= maxSpawnedUpgrades;

        if (!isPickupSpawned) {
            if (currentTime > actualUpgradeTime) { // After random time passes, check if upgrade has spawned
                // The upgrade will appear in one of the alien spawn points.
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                // This section handles spawning and associating the gun with it.
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                // set spawned flag

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
                ResetPickupActualSpawnTime(); 
            } 
        } else { // keep resetting time to respawn until picked up.
            actualUpgradeTime += Time.deltaTime;
        }

        currentSpawnTime += Time.deltaTime;
        if (currentSpawnTime > generatedSpawnTime) {
            currentSpawnTime = 0;
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            if ((aliensPerSpawn > 0) && (totalAliensSpawned < totalAliens)) {
                List<int> previousSpawnLocations = new List<int>();
                if (aliensPerSpawn > spawnPoints.Length) {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ? aliensPerSpawn - totalAliens : aliensPerSpawn;
                // spawn the aliens
                for (int i = 0; i < aliensPerSpawn; i++) {
                    if (aliensOnScreen < maxAliensOnScreen) {
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
                        totalAliensSpawned++;
                        aliensOnScreen++;
                        newAlien.transform.position = spawnLocation.transform.position;

                        Alien alienScript = newAlien.GetComponent<Alien>();
                        alienScript.target = player.transform;
                        Vector3 targetRotation = new Vector3(player.transform.position.x, newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);
                        alienScript.OnDestroy.AddListener(AlienDestroyed);
       
                    }
                }
            }
        }         
	}

    // Create a random pickup spawn time
    void ResetPickupActualSpawnTime() {
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0F, upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
        actualUpgradeTime += currentTime;
    }

    public void AlienDestroyed() {
        aliensOnScreen -= 1;
        aliensKilled++;
        Debug.Log("aliens on screen: " + aliensOnScreen + "\nAliens left: " + (totalAliens - aliensKilled));
    }
}
