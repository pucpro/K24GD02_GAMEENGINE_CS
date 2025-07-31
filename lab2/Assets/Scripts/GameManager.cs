using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Game Over")]
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private TextMeshProUGUI _scoreTextGameOver;

    [Header("Audio")]
    [SerializeField] private AudioClip scoreSound;

    private AudioSource _audioSource;
    private int _score;
    private bool hasLightShaken = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        _audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    public void AddScore(int value)
    {
        _score += value;
        if (scoreSound != null)
            _audioSource.PlayOneShot(scoreSound);
    }

    public void GameOver()
    {
        if (!hasLightShaken)
        {
            hasLightShaken = true;
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        var shake = Camera.main.GetComponent<CameraShake>();
        if (shake != null)
            yield return StartCoroutine(shake.Shake(0.2f, 0.05f));

        if (_scoreTextGameOver != null)
            _scoreTextGameOver.text = $"Score: {_score}";

        _gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}