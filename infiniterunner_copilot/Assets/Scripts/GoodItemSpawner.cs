/// <summary>
/// Spawns "good item" prefabs at random horizontal positions along the top of a UI Canvas at regular intervals,
/// up to a specified limit and within a total spawn duration. Each spawned item is given a Rigidbody2D for movement,
/// a BoxCollider2D for collision detection, and is destroyed after a set lifetime. The script tracks the number of
/// active good items and stops spawning when the duration or limit is reached.
/// </summary>

using UnityEngine;

public class GoodItemSpawner : MonoBehaviour
{
    public GameObject goodItemPrefab; // Prefab to spawn for good items
    public float spawnDuration = 20f; // Total spawn duration
    public float spawnRate = 0.5f; // Interval between spawns
    public int spawnLimit = 10; // Max number of good items to spawn
    public float startLifetime = 5f; // Lifetime of each good item
    public float startSpeed = 3f; // Initial speed of the good item
    public Canvas canvas; // Reference to the Canvas

    private float spawnTimer; // Timer for spawn duration
    private int currentSpawnCount = 0; // Current number of spawned good items
    private float screenWidth; // Screen width in canvas units

    public Vector3 spawnPosition = new Vector3(0, 0, 0); // Spawn position

    void Start()
    {
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>(); // Default to the first Canvas found if not set
        }

        screenWidth = canvas.GetComponent<RectTransform>().rect.width / 2; // Calculate screen width
        InvokeRepeating("SpawnGoodItem", 0f, spawnRate); // Start spawning good items
    }

    void SpawnGoodItem()
    {
        if (currentSpawnCount >= spawnLimit) return; // Check spawn limit

        float spawnX = Random.Range(-screenWidth, screenWidth); // Random x position
        spawnPosition.x = spawnX; // Set x value of spawn position
        spawnPosition.y = canvas.GetComponent<RectTransform>().rect.height / 2; // Set y value to top of the canvas
        spawnPosition.z = 0; // Ensure z value is 0

        GameObject goodItem = Instantiate(goodItemPrefab, canvas.transform); // Instantiate good item as child of canvas
        RectTransform rectTransform = goodItem.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = spawnPosition; // Set anchored position

        Rigidbody2D rb = goodItem.AddComponent<Rigidbody2D>(); // Add Rigidbody2D component
        rb.gravityScale = 0; // Disable gravity for UI elements
        rb.linearVelocity = new Vector2(0, -startSpeed); // Set initial velocity

        BoxCollider2D collider = goodItem.AddComponent<BoxCollider2D>(); // Add BoxCollider2D component
        collider.isTrigger = true; // Enable trigger for collision detection

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
        currentSpawnCount = GameObject.FindGameObjectsWithTag("goodItemPrefab").Length; // Update spawn count
        Debug.Log("Current Spawn Count: " + currentSpawnCount);
    }
}
