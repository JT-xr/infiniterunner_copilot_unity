/// <summary>
/// Handles collision events between the player and UI-related objects in the game.
/// Specifically, it increments the player's score when colliding with objects tagged as "goodItemPrefab"
/// and destroys those objects upon collision.
/// </summary>
/// 
using UnityEngine;

public class PlayerUICollisionHandler : MonoBehaviour
{
    public int score = 0; // Player's score

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only keep logs for score and collision detectors
        if (other.CompareTag("goodItemPrefab")) // Check if the colliding object is a Good Item
        {
            score++; // Increment score
            Destroy(other.gameObject); // Destroy the Good Item
            Debug.Log("Score: " + score); // Keep score log
        }
        else if (other.CompareTag("baditemPrefab")) // Check if the colliding object is a Bad Item
        {
            Destroy(other.gameObject); // Hide the Bad Item
            // Flash the screen red
            ScreenFlash screenFlash = Object.FindFirstObjectByType<ScreenFlash>();
            if (screenFlash != null)
            {
                screenFlash.Flash();
            }
            // No log except for collision/score
        }
    }
}
