using UnityEngine;

public class PlayerUICollisionHandler : MonoBehaviour
{
    public int score = 0; // Player's score

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("goodItemPrefab")) // Check if the colliding object is a Good Item
        {
            score++; // Increment score
            Destroy(other.gameObject); // Destroy the Good Item
            Debug.Log("Score: " + score); // Log the updated score
        }
    }
}
