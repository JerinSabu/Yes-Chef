using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverScoreText;
    public GameObject newHighScoreNotification; 

    private int currentScore = 0;
    private int highScore = 0;
    private float gameTimer = 180f; 
    public bool isGameActive = false;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0); 
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        isGameActive = false;
        Time.timeScale = 1f;

        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void StartGame()
    {
        isGameActive = true;
        isPaused = false;
        gameTimer = 180f;
        currentScore = 0;
        Time.timeScale = 1f;

        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

        
        CustomerWindow[] windows = FindObjectsByType<CustomerWindow>(FindObjectsSortMode.None);
        foreach (CustomerWindow window in windows)
        {
            window.ResetWindow();
        }

        UpdateUI();
    }

    private void Update()
    {
        if (isGameActive && !isPaused)
        {
            gameTimer -= Time.deltaTime;
            UpdateUI();

            if (gameTimer <= 0) EndGame();
        }
    }

    public void TogglePause()
    {
        if (!isGameActive) return;

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; 
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); 
    }

    public int AddScoreForOrder(Order completedOrder, float timeTaken)
    {
        if (!isGameActive || isPaused) return 0;

        int timePenalty = Mathf.FloorToInt(timeTaken);
        int finalScore = completedOrder.BaseScore - timePenalty;

        currentScore += finalScore;
        UpdateUI();

        return finalScore;
    }

    private void EndGame()
    {
        isGameActive = false;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; 

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            newHighScoreNotification.SetActive(true); 
        }
        else
        {
            newHighScoreNotification.SetActive(false);
        }

        gameOverScoreText.text = $"Final Score: {currentScore}\nHigh Score: {highScore}";
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {currentScore}\nHigh Score: {highScore}";
        if (timerText != null) timerText.text = $"Time: {Mathf.CeilToInt(gameTimer)}s";
    }
}