using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Prefab to spawn
    public GameObject objectToSpawn;
    // Spawn rate in seconds
    public float spawnRate = 1.0f;
    // Maximum number of objects to spawn simultaneously
    public int spawnLimit = 6;
    // Initial delay before spawning starts
    public float spawnDelay = 2.0f;
    // Area within which objects will be spawned
    public Vector2 spawnArea = new Vector2(10, 10);

    // Time for the next spawn
    private float nextSpawnTime;
    // Number of currently active objects
    private int activeObjects = 0;

    void Start()
    {
        // Set the initial spawn time
        nextSpawnTime = Time.time + spawnDelay;
    }

    void Update()
    {
        // Check if it's time to spawn a new object and if the limit is not reached
        if (Time.time >= nextSpawnTime && activeObjects < spawnLimit)
        {
            // Spawn a new object
            SpawnObject();
            // Set the time for the next spawn
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnObject()
    {
        // Calculate a random spawn position within the screen width
        Vector3 spawnPosition = new Vector3(
            Random.Range(0, Screen.width),
            Screen.height,
            0
        );

        // Convert screen position to world position
        spawnPosition = Camera.main.ScreenToWorldPoint(spawnPosition);
        spawnPosition.z = 0; // Ensure the object is on the same plane

        // Instantiate the object at the spawn position
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        // Increment the count of active objects
        activeObjects++;

        // Add a script to handle the object's behavior (e.g., falling and destroying itself when it goes off-screen)
        spawnedObject.AddComponent<FallingObject>().spawner = this;
    }

    public void ObjectDestroyed()
    {
        // Decrement the count of active objects
        activeObjects--;
    }
}

public class FallingObject : MonoBehaviour
{
    // Reference to the spawner
    public Spawner spawner;
    // Speed at which the object falls
    public float fallSpeed = 5.0f;

    void Update()
    {
        // Move the object downwards
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Check if the object has fallen below a certain point
        if (Camera.main.WorldToScreenPoint(transform.position).y < 0)
        {
            // Notify the spawner that the object is destroyed
            spawner.ObjectDestroyed();
            // Destroy the object
            Destroy(gameObject);
        }
    }
}
