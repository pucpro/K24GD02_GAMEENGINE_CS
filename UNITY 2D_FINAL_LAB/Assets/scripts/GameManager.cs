using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score Settings")]
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private IconHandle iconHandle;

    [Header("UI Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private StarDisplay starDisplay;
    private int coinCount = 0;
    [SerializeField] private Image nextlevelImage;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;
    public Slider bgVolumeSlider;
    public Slider sfxVolumeSlider;
    [SerializeField] private GameObject coinUI;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject healthUI;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        currentHealth = maxHealth;

        if (iconHandle == null)
            iconHandle = Object.FindFirstObjectByType<IconHandle>();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        nextlevelImage.enabled = false;
    }

    public void AddCoin()
    {
        coinCount++;
    }

    private void Start()
    {
        UpdateScore();
        bgVolumeSlider.value = SoundManager.Instance.GetMusicVolume();
        sfxVolumeSlider.value = SoundManager.Instance.GetSFXVolume();
        bgVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);

    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            iconHandle.UseShot(maxHealth - currentHealth);
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void WinGame()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        int starCount = CalculateStars(coinCount);
        starDisplay.ShowStars(starCount);

        Time.timeScale = 0f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevels = SceneManager.sceneCountInBuildSettings;
        if (currentSceneIndex + 1 < maxLevels)
        {
            nextlevelImage.enabled = true;
        }
        if (coinText != null)
            coinText.text =  coinCount.ToString();
        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (coinUI != null)
            coinUI.SetActive(false);
    }
    private int CalculateStars(int coins)
    {
        if (coins >= 6) return 3;
        if (coins >= 4) return 2;
        return 1;
    }
    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (coinUI != null)
            coinUI.SetActive(false);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); 
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (coinUI != null)
            coinUI.SetActive(false);
        if (healthUI != null)
            healthUI.SetActive(false);

    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        if (pauseButton != null)
            pauseButton.SetActive(true);

        if (coinUI != null)
            coinUI.SetActive(true);
        if (healthUI != null)
            healthUI.SetActive(true);
    }

}