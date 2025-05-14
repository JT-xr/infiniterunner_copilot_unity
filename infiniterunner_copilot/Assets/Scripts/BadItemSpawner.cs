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

    public float minAngularVelocity = -30f; // Minimum angular velocity (degrees per second)
    public float maxAngularVelocity = 30f;  // Maximum angular velocity (degrees per second)

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

        // Try up to 10 times to find a non-overlapping spawn position
        int maxTries = 10;
        bool foundNonOverlap = false;
        float spawnX = 0f;
        Rect newRect = new Rect();
        RectTransform prefabRect = imagePrefab.GetComponent<RectTransform>();
        Vector2 itemSize = prefabRect != null ? prefabRect.sizeDelta : new Vector2(50, 50);
        for (int i = 0; i < maxTries; i++)
        {
            spawnX = Random.Range(-screenWidth, screenWidth);
            spawnPosition.x = spawnX;
            spawnPosition.y = canvas.GetComponent<RectTransform>().rect.height / 2;
            spawnPosition.z = 0;
            newRect = new Rect(new Vector2(spawnPosition.x - itemSize.x / 2, spawnPosition.y - itemSize.y / 2), itemSize);

            // Check overlap with all good and bad items
            bool overlap = false;
            foreach (var obj in GameObject.FindGameObjectsWithTag("baditemPrefab"))
            {
                RectTransform rt = obj.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Rect otherRect = new Rect(rt.anchoredPosition - rt.sizeDelta / 2, rt.sizeDelta);
                    if (newRect.Overlaps(otherRect)) { overlap = true; break; }
                }
            }
            if (!overlap)
            {
                foreach (var obj in GameObject.FindGameObjectsWithTag("goodItemPrefab"))
                {
                    RectTransform rt = obj.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        Rect otherRect = new Rect(rt.anchoredPosition - rt.sizeDelta / 2, rt.sizeDelta);
                        if (newRect.Overlaps(otherRect)) { overlap = true; break; }
                    }
                }
            }
            if (!overlap) { foundNonOverlap = true; break; }
        }
        if (!foundNonOverlap) return; // Give up if can't find a spot

        GameObject image = Instantiate(imagePrefab, spawnPosition, Quaternion.identity, canvas.transform); // Instantiate image as child of canvas

        // Add a random initial rotation so items don't rotate in unison
        float initialZRotation = Random.Range(0f, 360f);
        image.transform.rotation = Quaternion.Euler(0f, 0f, initialZRotation);

        image.tag = "baditemPrefab"; // Set tag for game end logic
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = spawnPosition; // Set anchored position

        Rigidbody2D rb = image.AddComponent<Rigidbody2D>(); // Add Rigidbody2D component
        rb.linearVelocity = new Vector2(0, -startSpeed); // Set initial velocity

        // Add a slight and slow random rotation using public variables
        rb.angularVelocity = Random.Range(minAngularVelocity, maxAngularVelocity);

        BoxCollider2D collider = image.AddComponent<BoxCollider2D>(); // Add BoxCollider2D component
        collider.isTrigger = true; // Enable trigger for collision detection

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
        currentSpawnCount = GameObject.FindGameObjectsWithTag("baditemPrefab").Length; // Update spawn count
    }

    public void StopSpawning()
    {
        CancelInvoke("SpawnImage");
    }

    public void RestartSpawning()
    {
        CancelInvoke("SpawnImage");
        InvokeRepeating("SpawnImage", 0f, spawnRate);
    }
}

