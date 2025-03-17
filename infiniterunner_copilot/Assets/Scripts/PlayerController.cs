using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f; // Moderate movement speed
    private float screenBoundaryLeft;
    private float screenBoundaryRight;
    private float playerWidth;

    void Start()
    {
        // Calculate screen boundaries in world coordinates
        Camera mainCamera = Camera.main;
        playerWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        screenBoundaryLeft = -screenBounds.x + playerWidth;
        screenBoundaryRight = screenBounds.x - playerWidth;

        // Position player at the bottom of the screen
        Vector3 startPosition = transform.position;
        startPosition.y = -screenBounds.y + playerWidth;
        transform.position = startPosition;
    }

    void Update()
    {
        // Get horizontal input (left/right arrow keys)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        
        // Calculate new position
        Vector3 newPosition = transform.position + new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);
        
        // Clamp position to screen boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, screenBoundaryLeft, screenBoundaryRight);
        
        // Update position
        transform.position = newPosition;
    }
}
