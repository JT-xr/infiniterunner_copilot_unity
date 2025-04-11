using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    private float timeRemaining = 60f;
    private bool timerIsRunning = false;
    public Button restartButton; // Reference to the restart button

    private void Start()
    {
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
        // Reset the timer
        timeRemaining = 60f;
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