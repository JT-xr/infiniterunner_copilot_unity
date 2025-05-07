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

    // Add references to your spawner scripts in the Inspector or find them at runtime
    public Spawner badItemSpawner;
    public GoodItemSpawner goodItemSpawner;

    private GameObject playerObject; // cache the player object

    private void Start()
    {
        GameManager.isGameActive = true; // Allow score updates on initial session

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
        // Optionally auto-find spawners if not assigned
        if (badItemSpawner == null)
            badItemSpawner = FindFirstObjectByType<Spawner>();
        if (goodItemSpawner == null)
            goodItemSpawner = FindFirstObjectByType<GoodItemSpawner>();

        playerObject = GameObject.FindWithTag("Player");
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

                // --- GAME END CONDITIONS ---
                StopAllFallingItems();
                HidePlayer();
                StopSpawnersAndHideItems();
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

    private void StopAllFallingItems()
    {
        // Stop all bad items (tag: "baditemPrefab")
        var badItems = GameObject.FindGameObjectsWithTag("baditemPrefab");
        foreach (var item in badItems)
        {
            var rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
        // Stop all good items (tag: "goodItemPrefab")
        var goodItems = GameObject.FindGameObjectsWithTag("goodItemPrefab");
        foreach (var item in goodItems)
        {
            var rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
    }

    private void HidePlayer()
    {
        if (playerObject != null)
        {
            playerObject.SetActive(false);
        }
    }

    private void ShowPlayer()
    {
        if (playerObject != null)
        {
            playerObject.SetActive(true);
        }
    }

    private void StopSpawnersAndHideItems()
    {
        // Stop spawning new items
        if (badItemSpawner != null)
            badItemSpawner.StopSpawning();
        if (goodItemSpawner != null)
            goodItemSpawner.StopSpawning();

        // Hide all existing bad items
        var badItems = GameObject.FindGameObjectsWithTag("baditemPrefab");
        foreach (var item in badItems)
        {
            item.SetActive(false);
        }
        // Hide all existing good items
        var goodItems = GameObject.FindGameObjectsWithTag("goodItemPrefab");
        foreach (var item in goodItems)
        {
            item.SetActive(false);
        }
    }

    private void RestartGame()
    {
        StartCoroutine(RestartGameCoroutine());
    }

    private System.Collections.IEnumerator RestartGameCoroutine()
    {
        // Set game inactive before reset
        GameManager.isGameActive = false;
        // Reset the timer to the initial value set in Inspector
        timeRemaining = initialTime;
        timerIsRunning = true;
        UpdateTimerText();

        // Reset the score using ScoreManager
        ScoreManager.Instance.ResetScore();

        // Show the player again
        ShowPlayer();

        // Optionally re-enable all items (if you want them to reappear on restart)
        var badItems = GameObject.FindGameObjectsWithTag("baditemPrefab");
        foreach (var item in badItems)
        {
            item.SetActive(true);
        }
        var goodItems = GameObject.FindGameObjectsWithTag("goodItemPrefab");
        foreach (var item in goodItems)
        {
            item.SetActive(true);
        }

        // Restart spawning
        if (badItemSpawner != null)
            badItemSpawner.RestartSpawning();
        if (goodItemSpawner != null)
            goodItemSpawner.RestartSpawning();

        // Wait a short time to clear lingering collisions
        yield return new WaitForSeconds(0.5f);

        // Set game active after everything is ready and collisions are cleared
        GameManager.isGameActive = true;

        // Hide the restart button
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }
    }
}