using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text scoreText;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject restartButton;

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int delta)
    {
        score += delta;
        scoreText.text = $"Score: {score}";
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
        restartButton.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowLose()
    {
        losePanel.SetActive(true);
        restartButton.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}