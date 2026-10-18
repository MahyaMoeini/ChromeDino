using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    
    // GAME SPEED
    

    [Header("Game Speed")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float dashMultiplier = 1.8f;

    public float WorldSpeed { get; private set; }


    
    // SCORE
    

    [Header("Score")]
    [SerializeField] private float scorePerSecond = 10f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    private float score;
    private int highScore;


    
    // HEALTH
    

    [Header("Health")]
    [SerializeField] private Slider healthSlider;


    // GAME OVER
    

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    public bool IsGameOver { get; private set; }


    
    // PAUSE
    

    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;

    private bool isPaused;


    // AWAKE
  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        WorldSpeed = baseSpeed;
    }


    
    // START
    

    private void Start()
    {
        Time.timeScale = 1f;

        isPaused = false;
        IsGameOver = false;

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Game Over Panel 
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Pause Panel 
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        UpdateScoreUI();
    }


    // UPDATE
    

    private void Update()
    {
        if (IsGameOver)
            return;

        if (isPaused)
            return;

        score += scorePerSecond * Time.deltaTime;

        UpdateScoreUI();
    }


    
    // DASH
   

    public void SetDash(bool active)
    {
        if (IsGameOver)
        {
            WorldSpeed = 0f;
            return;
        }

        if (active)
        {
            WorldSpeed = baseSpeed * dashMultiplier;
        }
        else
        {
            WorldSpeed = baseSpeed;
        }
    }


    
    // SCORE
    

    public void AddScore(int amount)
    {
        if (IsGameOver)
            return;

        score += amount;

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        int currentScore = Mathf.FloorToInt(score);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }

        if (highScoreText != null)
        {
            int displayedHighScore =
                Mathf.Max(highScore, currentScore);

            highScoreText.text =
                "High Score: " + displayedHighScore;
        }
    }


  
    // HEALTH
  

    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (healthSlider == null)
            return;

        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }


    
    // GAME OVER
    

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;
        isPaused = false;

        WorldSpeed = 0f;

        int finalScore = Mathf.FloorToInt(score);

        if (finalScore > highScore)
        {
            highScore = finalScore;

            PlayerPrefs.SetInt(
                "HighScore",
                highScore
            );

            PlayerPrefs.Save();
        }

        UpdateScoreUI();

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }


    
    // RESTART
    

    public void RestartGame()
    {
        Time.timeScale = 1f;

        isPaused = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }


    
    // PAUSE
    

    public void TogglePause()
    {
        if (IsGameOver)
            return;

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }


    public void PauseGame()
    {
        if (IsGameOver)
            return;

        isPaused = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            Debug.LogError(
                "PausePanel داخل GameManager تنظیم نشده!"
            );
        }

        Time.timeScale = 0f;

        Debug.Log("GAME PAUSED");
    }


    public void ResumeGame()
    {
        Debug.Log("RESUME BUTTON PRESSED");

        isPaused = false;

        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);

            Debug.Log(
                "PausePanel Active = " +
                pausePanel.activeSelf
            );
        }
        else
        {
            Debug.LogError(
                "PausePanel reference is NULL!"
            );
        }
    }


    
    // MAIN MENU
    

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }


    
    // QUIT


    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}