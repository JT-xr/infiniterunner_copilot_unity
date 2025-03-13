/* This script spawns spheres at random x positions within the screen width. 
 The spheres are instantiated at a specified spawn position and fall with a set speed. 
 The spawning continues at a defined rate until a maximum number of spheres is reached or the total spawn duration elapses.*/

using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject spherePrefab; // Sphere prefab to spawn
    public float spawnDuration = 20f; // Total spawn duration
    public float spawnRate = 0.2f; // Interval between spawns
    public int spawnLimit = 6; // Max number of spheres to spawn
    public float startLifetime = 5f; // Lifetime of each sphere
    public float startSpeed = 5f; // Initial speed of the sphere

    private float spawnTimer; // Timer for spawn duration
    private int currentSpawnCount = 0; // Current number of spawned spheres
    private float screenWidth; // Screen width in world units

    public Vector3 spawnPosition = new Vector3(0, 0, 0); // Spawn position

    void Start()
    {
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect; // Calculate screen width
        InvokeRepeating("SpawnSphere", 0f, spawnRate); // Start spawning spheres
    }

    void SpawnSphere()
    {
        if (currentSpawnCount >= spawnLimit) return; // Check spawn limit

        float spawnX = Random.Range(-screenWidth, screenWidth); // Random x position
        spawnPosition.x = spawnX; // Set x value of spawn position
        spawnPosition.z = 0; // Ensure z value is 0
        GameObject sphere = Instantiate(spherePrefab, spawnPosition, Quaternion.identity); // Instantiate sphere
        Rigidbody rb = sphere.AddComponent<Rigidbody>(); // Add Rigidbody component
        rb.linearVelocity = new Vector3(0, -startSpeed, 0); // Set initial velocity
        Destroy(sphere, startLifetime); // Destroy sphere after lifetime
        currentSpawnCount++; // Increment spawn count
        spawnTimer += spawnRate; // Increment spawn timer

        if (spawnTimer >= spawnDuration)
        {
            CancelInvoke("SpawnSphere"); // Cancel spawning
        }
    }

    void Update()
    {
        currentSpawnCount = GameObject.FindGameObjectsWithTag("Sphere").Length; // Update spawn count
    }
}

