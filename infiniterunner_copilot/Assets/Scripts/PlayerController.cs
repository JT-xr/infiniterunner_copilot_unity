using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f; // Movement speed
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
    }
}
