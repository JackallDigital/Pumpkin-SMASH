using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawnPoints;

    public float timeBetweenSpawns;
    private float nextSpawnTime;

    private Dictionary<GameObject, Transform> occupiedSpawnPoints = new Dictionary<GameObject, Transform>();

    public static Spawner Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject); // Ensure only one instance of Spawner exists
        }
    }

    void Update() {
        if (!HammerController.Instance.isDead && HammerController.Instance.livesLeftTillBellTolls > 0) { 
            if (Time.time > nextSpawnTime) {
                // Find an available spawn point
                Transform spawnPoint = GetAvailableSpawnPoint();

                if (spawnPoint != null) {
                    GameObject spawnedObject = Instantiate(enemy, spawnPoint.position, Quaternion.AngleAxis(-90, Vector3.up));
                    occupiedSpawnPoints.Add(spawnedObject, spawnPoint);
                }

                nextSpawnTime = Time.time + timeBetweenSpawns;
            }
        }
    }

    // Check if a spawn point is available
    Transform GetAvailableSpawnPoint() {
        List<Transform> availableSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints) {
            if (!occupiedSpawnPoints.ContainsValue(spawnPoint)) {
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        if (availableSpawnPoints.Count > 0) {
            return availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];
        }

        return null; // No available spawn points
    }

    // Call this method when an object is destroyed or despawned
    public void ReleaseSpawnPoint(GameObject spawnedObject) {
        if (occupiedSpawnPoints.ContainsKey(spawnedObject)) {
            occupiedSpawnPoints.Remove(spawnedObject);
        }
    }

    public void DestroyObjectAndReleaseSpawnPoint(GameObject objectToDestroy) {
        if (Spawner.Instance != null) {
            Spawner.Instance.ReleaseSpawnPoint(objectToDestroy);
        }
        Destroy(objectToDestroy);
    }
}