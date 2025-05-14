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

    public float minAngularVelocity = -30f; // Minimum angular velocity (degrees per second)
    public float maxAngularVelocity = 30f;  // Maximum angular velocity (degrees per second)

    public AudioClip goodItemCollectSfx; // Sound effect to play on player collision

    private float spawnTimer; // Timer for spawn duration
    private int currentSpawnCount = 0; // Current number of spawned good items
    private float screenWidth; // Screen width in canvas units

    public Vector3 spawnPosition = new Vector3(0, 0, 0); // Spawn position

    void Start()
    {
        if (canvas == null)
        {
            canvas = FindFirstObjectByType<Canvas>(); // Default to the first Canvas found if not set
        }

        screenWidth = canvas.GetComponent<RectTransform>().rect.width / 2; // Calculate screen width
        InvokeRepeating("SpawnGoodItem", 0f, spawnRate); // Start spawning good items
    }

    void SpawnGoodItem()
    {
        if (currentSpawnCount >= spawnLimit) return; // Check spawn limit

        // Try up to 10 times to find a non-overlapping spawn position
        int maxTries = 10;
        bool foundNonOverlap = false;
        float spawnX = 0f;
        Rect newRect = new Rect();
        RectTransform prefabRect = goodItemPrefab.GetComponent<RectTransform>();
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
            foreach (var obj in GameObject.FindGameObjectsWithTag("goodItemPrefab"))
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
                foreach (var obj in GameObject.FindGameObjectsWithTag("baditemPrefab"))
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

        GameObject goodItem = Instantiate(goodItemPrefab, canvas.transform); // Instantiate good item as child of canvas

        // Add a random initial rotation so items don't rotate in unison
        float initialZRotation = Random.Range(0f, 360f);
        goodItem.transform.rotation = Quaternion.Euler(0f, 0f, initialZRotation);

        RectTransform rectTransform = goodItem.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = spawnPosition; // Set anchored position

        Rigidbody2D rb = goodItem.AddComponent<Rigidbody2D>(); // Add Rigidbody2D component
        rb.gravityScale = 0; // Disable gravity for UI elements
        rb.linearVelocity = new Vector2(0, -startSpeed); // Set initial velocity

        // Add a slight and slow random rotation using public variables
        rb.angularVelocity = Random.Range(minAngularVelocity, maxAngularVelocity);

        BoxCollider2D collider = goodItem.AddComponent<BoxCollider2D>(); // Add BoxCollider2D component
        collider.isTrigger = true; // Enable trigger for collision detection

        // Attach the sound effect script and set the AudioClip
        var sfx = goodItem.AddComponent<GoodItemSfxOnPlayerCollision>();
        sfx.sfxClip = goodItemCollectSfx;

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
        // Removed Debug.Log for Current Spawn Count as requested
    }

    public void StopSpawning()
    {
        CancelInvoke("SpawnGoodItem");
    }

    public void RestartSpawning()
    {
        CancelInvoke("SpawnGoodItem");
        InvokeRepeating("SpawnGoodItem", 0f, spawnRate);
    }
}
