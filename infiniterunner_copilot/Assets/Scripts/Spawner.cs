/* This script spawns UI images at random x positions within the screen width. 
 The images are instantiated at a specified spawn position and fall with a set speed. 
 The spawning continues at a defined rate until a maximum number of images is reached or the total spawn duration elapses.*/

using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject imagePrefab; // UI Image prefab to spawn
    public float spawnDuration = 20f; // Total spawn duration
    public float spawnRate = 0.2f; // Interval between spawns
    public int spawnLimit = 6; // Max number of images to spawn
    public float startLifetime = 5f; // Lifetime of each image
    public float startSpeed = 5f; // Initial speed of the image
    public Canvas canvas; // Reference to the Canvas

    private float spawnTimer; // Timer for spawn duration
    private int currentSpawnCount = 0; // Current number of spawned images
    private float screenWidth; // Screen width in world units

    public Vector3 spawnPosition = new Vector3(0, 0, 0); // Spawn position

    void Start()
    {
        if (canvas == null)
        {
            canvas = FindFirstObjectByType<Canvas>(); // Default to the first Canvas found if not set
        }

        screenWidth = canvas.GetComponent<RectTransform>().rect.width / 2; // Calculate screen width
        InvokeRepeating("SpawnImage", 0f, spawnRate); // Start spawning images
    }

    void SpawnImage()
    {
        if (currentSpawnCount >= spawnLimit) return; // Check spawn limit

        float spawnX = Random.Range(-screenWidth, screenWidth); // Random x position
        spawnPosition.x = spawnX; // Set x value of spawn position
        spawnPosition.y = canvas.GetComponent<RectTransform>().rect.height / 2; // Set y value to top of the canvas
        spawnPosition.z = 0; // Ensure z value is 0

        GameObject image = Instantiate(imagePrefab, spawnPosition, Quaternion.identity, canvas.transform); // Instantiate image as child of canvas
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = spawnPosition; // Set anchored position

        Rigidbody2D rb = image.AddComponent<Rigidbody2D>(); // Add Rigidbody2D component
        rb.linearVelocity = new Vector2(0, -startSpeed); // Set initial velocity
        Destroy(image, startLifetime); // Destroy image after lifetime
        currentSpawnCount++; // Increment spawn count
        spawnTimer += spawnRate; // Increment spawn timer

        if (spawnTimer >= spawnDuration)
        {
            CancelInvoke("SpawnImage"); // Cancel spawning
        }
    }

    void Update()
    {
        currentSpawnCount = GameObject.FindGameObjectsWithTag("Sphere").Length; // Update spawn count
    }
}

