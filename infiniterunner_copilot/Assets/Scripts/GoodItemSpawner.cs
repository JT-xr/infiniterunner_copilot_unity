using UnityEngine;

public class GoodItemSpawner : MonoBehaviour
{
    public GameObject goodItemPrefab; // Prefab to spawn for good items
    public float spawnDuration = 20f; // Total spawn duration
    public float spawnRate = 0.5f; // Interval between spawns
    public int spawnLimit = 10; // Max number of good items to spawn
    public float startLifetime = 5f; // Lifetime of each good item
    public float startSpeed = 3f; // Initial speed of the good item
    public Camera orthographicCamera; // Reference to the orthographic camera

    private float spawnTimer; // Timer for spawn duration
    private int currentSpawnCount = 0; // Current number of spawned good items
    private float screenWidth; // Screen width in world units

    public Vector3 spawnPosition = new Vector3(0, 0, 0); // Spawn position

    void Start()
    {
        if (orthographicCamera == null)
        {
            orthographicCamera = Camera.main; // Default to main camera if not set
        }

        screenWidth = orthographicCamera.orthographicSize * orthographicCamera.aspect; // Calculate screen width
        InvokeRepeating("SpawnGoodItem", 0f, spawnRate); // Start spawning good items
    }

    void SpawnGoodItem()
    {
        if (currentSpawnCount >= spawnLimit) return; // Check spawn limit

        float spawnX = Random.Range(-screenWidth, screenWidth); // Random x position
        spawnPosition.x = spawnX; // Set x value of spawn position
        spawnPosition.z = 0; // Ensure z value is 0

        GameObject goodItem = Instantiate(goodItemPrefab, spawnPosition, Quaternion.identity); // Instantiate good item
        Rigidbody rb = goodItem.AddComponent<Rigidbody>(); // Add Rigidbody component
        rb.linearVelocity = new Vector3(0, -startSpeed, 0); // Set initial velocity
        Destroy(goodItem, startLifetime); // Destroy good item after lifetime
        currentSpawnCount++; // Increment spawn count
        spawnTimer += spawnRate; // Increment spawn timer

        if (spawnTimer >= spawnDuration)
        {
            CancelInvoke("SpawnGoodItem"); // Cancel spawning
        }
    }

    void Update()
    {
        currentSpawnCount = GameObject.FindGameObjectsWithTag("GoodItem").Length; // Update spawn count
    }
}
