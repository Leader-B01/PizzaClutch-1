using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour
{
    [Header("Cloud Settings")]
    [SerializeField] private GameObject[] cloudPrefabs;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private float minY = -1f;
    [SerializeField] private float maxY = 3f;
    
    [Header("Spawn Area")]
    [SerializeField] private float spawnX = 12f;
    [SerializeField] private float despawnX = -12f;

    private List<Cloud> activeClouds = new List<Cloud>();

    private void Start()
    {
        // Validate that cloud prefabs are assigned
        if (cloudPrefabs == null || cloudPrefabs.Length == 0)
        {
            Debug.LogError("No cloud prefabs assigned to CloudGenerator!");
            return;
        }

        // Start cloud spawning coroutine
        StartCoroutine(SpawnClouds());
    }

    private IEnumerator SpawnClouds()
    {
        while (true)
        {
            // Generate random cloud properties
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            
            // Wait before spawning next cloud
            yield return new WaitForSeconds(waitTime);
            
            SpawnCloud();
        }
    }

    private void SpawnCloud()
    {
        // Select a random cloud prefab
        int prefabIndex = Random.Range(0, cloudPrefabs.Length);
        GameObject cloudPrefab = cloudPrefabs[prefabIndex];
        
        // Randomize position, with x at spawn point and random y within range
        Vector3 spawnPosition = new Vector3(spawnX, Random.Range(minY, maxY), 0f);
        
        // Create the cloud
        GameObject cloudObject = Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);
        cloudObject.transform.SetParent(transform);
        
        // Cloud uses its original scale from the prefab (no randomization)
        
        // Add to tracking list
        float speed = Random.Range(minSpeed, maxSpeed);
        Cloud cloud = new Cloud(cloudObject, speed);
        activeClouds.Add(cloud);
    }

    private void Update()
    {
        // Move all active clouds
        for (int i = activeClouds.Count - 1; i >= 0; i--)
        {
            Cloud cloud = activeClouds[i];
            
            // Move the cloud
            cloud.cloudObject.transform.Translate(Vector3.left * cloud.speed * Time.deltaTime);
            
            // Check if the cloud has moved beyond the despawn point
            if (cloud.cloudObject.transform.position.x < despawnX)
            {
                Destroy(cloud.cloudObject);
                activeClouds.RemoveAt(i);
            }
        }
    }

    // Simple class to track cloud objects and their properties
    private class Cloud
    {
        public GameObject cloudObject;
        public float speed;

        public Cloud(GameObject cloudObject, float speed)
        {
            this.cloudObject = cloudObject;
            this.speed = speed;
        }
    }
}