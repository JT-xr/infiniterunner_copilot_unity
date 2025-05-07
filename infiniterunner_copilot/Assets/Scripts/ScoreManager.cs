/// <summary>
/// The ScoreManager class is responsible for managing the player's score in the game.
/// It provides functionality to add points, reset the score, and update the score display.
/// This class implements the Singleton pattern to ensure only one instance exists during runtime.
/// </summary>

using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    [SerializeField] private Text scoreText;
    private int currentScore = 0;
    public bool canUpdateScore = true; // Controls if score can be updated

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddPoints(int points)
    {
        // Prevent score update if not allowed or game is not active
        if (!canUpdateScore || !GameManager.isGameActive)
        {
            Debug.Log("AddPoints called but ignored because canUpdateScore is false or game is not active.");
            return;
        }
        currentScore += points;
        UpdateScoreDisplay();
    }

    public void ResetScore()
    {
        currentScore = 0;
        canUpdateScore = true; // Ensure score can be updated after reset
        Debug.Log("ScoreManager: ResetScore called. Score reset to 0.");
        if (scoreText == null)
        {
            scoreText = FindFirstObjectByType<Text>(); // Try to recover reference if lost
            Debug.LogWarning("ScoreManager: scoreText reference was null. Attempted to recover.");
        }
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + currentScore.ToString();
    }
}
