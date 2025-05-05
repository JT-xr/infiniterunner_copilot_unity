

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
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f; // Movement speed
    [SerializeField] private Text scoreText; // UI Text to display the score
    private int score = 0; // Player's score
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

                Debug.Log($"Head rotation: {headRotation}, Target X Position: {targetXPosition}");
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

        Debug.Log("Player position: " + rectTransform.anchoredPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name); // Log the name of the collided object

        if (collision.gameObject.CompareTag("goodItemPrefab")) // Check if the tag matches
        {
            Debug.Log("Collision with goodItemPrefab detected!"); // Log if the tag matches

            // Update score by 3 points
            score += 3;

            // Update the score UI
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
                Debug.Log("Score updated to: " + score); // Log the updated score
            }
            else
            {
                Debug.LogWarning("ScoreText UI element is not assigned!"); // Warn if scoreText is null
            }
        }
        else
        {
            Debug.Log("Collision with an object that does not have the 'goodItemPrefab' tag."); // Log if the tag doesn't match
            Debug.Log("Collided object's tag: " + collision.gameObject.tag); // Log the tag of the collided object
        }
    }
}

public class TriggerDebugger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Global Trigger detected with: " + collision.gameObject.name);
    }
}

public class GlobalTriggerDebugger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Global Trigger detected with: " + collision.gameObject.name);
    }
}
