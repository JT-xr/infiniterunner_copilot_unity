using UnityEngine;
using UnityEngine.UI; // For UI elements

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f; // Movement speed
    [SerializeField] private Text scoreText; // UI Text to display the score
    private int score = 0; // Player's score
    private float screenBoundaryLeft;
    private float screenBoundaryRight;
    private float playerWidth;
    private RectTransform rectTransform;

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
    }

    void Update()
    {
        // Get horizontal input (left/right arrow keys)
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Debugging user input
        if (horizontalInput < 0)
        {
            Debug.Log("left");
        }
        else if (horizontalInput > 0)
        {
            Debug.Log("right");
        }

        // Calculate new position
        Vector2 newPosition = rectTransform.anchoredPosition + new Vector2(horizontalInput * moveSpeed * Time.deltaTime, 0);

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
