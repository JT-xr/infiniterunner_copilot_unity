/// <summary>
/// Controls the player character in an infinite runner game using either AR face tracking (head rotation) or keyboard input.
/// 
/// - Uses AR Foundation's <see cref="ARFaceManager"/> to track the player's head rotation and map it to horizontal movement within screen boundaries.
/// - Falls back to keyboard input (left/right arrows or A/D) if AR face tracking is unavailable.
/// - Keeps the player within the visible area of the UI canvas.
/// - Handles collision detection with objects tagged as "goodItemPrefab", updating the player's score and UI accordingly.
/// - Includes additional trigger debugger classes for logging collision events for debugging purposes.
/// </summary>

using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f; // Movement speed
    private float screenBoundaryLeft;
    private float screenBoundaryRight;
    private float playerWidth;
    private RectTransform rectTransform;

    private ARFaceManager arFaceManager; // AR Face Manager for head tracking

    void Start()
    {
        // Get the RectTransform component
        rectTransform = GetComponent<RectTransform>();

        // Calculate the player's width
        playerWidth = rectTransform.rect.width / 2;

        // Calculate screen boundaries in canvas space
        RectTransform canvasRect = rectTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        screenBoundaryLeft = -canvasRect.rect.width / 2 + playerWidth;
        screenBoundaryRight = canvasRect.rect.width / 2 - playerWidth;

        // Position the player at the bottom of the canvas
        Vector3 startPosition = rectTransform.anchoredPosition;
        startPosition.y = -canvasRect.rect.height / 2 + playerWidth;
        rectTransform.anchoredPosition = startPosition;

        // Initialize AR Face Manager
        arFaceManager = Object.FindFirstObjectByType<ARFaceManager>();
        if (arFaceManager == null)
        {
            Debug.LogError("ARFaceManager not found. Ensure AR Foundation is set up correctly.");
        }
    }

    void Update()
    {
        float horizontalInput = 0;

        // Declare newPosition at the beginning of the Update method
        Vector2 newPosition = rectTransform.anchoredPosition;

        if (arFaceManager != null && arFaceManager.trackables.count > 0)
        {
            // Get the first tracked face
            ARFace face = null;
            foreach (var trackedFace in arFaceManager.trackables)
            {
                face = trackedFace;
                break; // Get the first face and exit the loop
            }

            // Adjust horizontal input to align with the center of the screen when head rotation is zero
            if (face != null)
            {
                // Get the head rotation (yaw) in local space
                float headRotation = face.transform.localEulerAngles.y;

                // Normalize head rotation to a range of [-180, 180]
                if (headRotation > 180f)
                {
                    headRotation -= 360f;
                }

                // Inverse the mapping logic
                float normalizedRotation = Mathf.Clamp(-headRotation / 30f, -1f, 1f); // Normalize to [-1, 1] and invert
                float targetXPosition = Mathf.Lerp(screenBoundaryLeft, screenBoundaryRight, (normalizedRotation + 1f) / 2f); // Map to screen boundaries

                // Update position directly
                newPosition = rectTransform.anchoredPosition;
                newPosition.x = targetXPosition;
                rectTransform.anchoredPosition = newPosition;
            }
        }
        else
        {
            // Fallback to keyboard input
            horizontalInput = Input.GetAxisRaw("Horizontal");
            newPosition = rectTransform.anchoredPosition + new Vector2(horizontalInput * moveSpeed * Time.deltaTime, 0);
        }

        // Ensure newPosition is updated correctly based on AR or keyboard input
        if (arFaceManager != null && arFaceManager.trackables.count > 0) {
            // AR-based position update logic
            // ...existing code...
        } else {
            // Fallback to keyboard input
            horizontalInput = Input.GetAxisRaw("Horizontal");
            newPosition = rectTransform.anchoredPosition + new Vector2(horizontalInput * moveSpeed * Time.deltaTime, 0);
        }

        // Clamp position to screen boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, screenBoundaryLeft, screenBoundaryRight);

        // Update position
        rectTransform.anchoredPosition = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only keep logs for score and collision detectors
        if (collision.gameObject.CompareTag("goodItemPrefab"))
        {
            // Add points using ScoreManager (centralized score logic)
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(3);
            }
        }
    }
}
