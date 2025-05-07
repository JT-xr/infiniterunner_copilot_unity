/// <summary>
/// Singleton GameManager class responsible for managing core game state.
/// Ensures only one instance persists across scenes using DontDestroyOnLoad.
/// Provides a method to reset the player's score by delegating to the ScoreManager.
/// </summary>


using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool isGameActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetScore()
    {
        // Delegate the score reset to the ScoreManager
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
    }
}