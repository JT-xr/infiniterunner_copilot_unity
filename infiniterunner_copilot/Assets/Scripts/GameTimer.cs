using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    private float timeRemaining = 60f;
    private bool timerIsRunning = false;

    private void Start()
    {
        // Start the timer
        timerIsRunning = true;
        UpdateTimerText();
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
}