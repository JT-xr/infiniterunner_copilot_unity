/// <summary>
/// Controls a countdown game timer, updates the timer UI, and manages the restart button.
/// 
/// - Displays the remaining time in seconds on a UI Text element.
/// - Changes the timer text color to red when 20 seconds or less remain.
/// - Shows a restart button when the timer reaches zero.
/// - Resets the timer and score when the restart button is pressed.
/// 
/// Dependencies:
/// - Requires a reference to a UI Text for displaying the timer.
/// - Requires a reference to a UI Button for restarting the game.
/// - Assumes a singleton ScoreManager with a ResetScore() method.
/// </summary>
 
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    public float timeRemaining = 60f; // Made public for Inspector access
    private float initialTime; // Store the initial value set in Inspector
    private bool timerIsRunning = false;
    public Button restartButton; // Reference to the restart button

    private void Start()
    {
        initialTime = timeRemaining; // Save the value set in Inspector
        // Start the timer
        timerIsRunning = true;
        UpdateTimerText();

        // Hide the restart button initially
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerText();

                // Show the restart button when the timer reaches zero
                if (restartButton != null)
                {
                    restartButton.gameObject.SetActive(true);
                }
            }
        }
    }

    private void UpdateTimerText()
    {
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = "Clock: " + seconds.ToString("00");

        // Change color to red when time is less than or equal to 20 seconds
        if (timeRemaining <= 20)
        {
            timerText.color = Color.red;
        }
    }

    private void RestartGame()
    {
        // Reset the timer to the initial value set in Inspector
        timeRemaining = initialTime;
        timerIsRunning = true;
        UpdateTimerText();

        // Reset the score using ScoreManager
        ScoreManager.Instance.ResetScore();

        // Hide the restart button
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }
    }
}