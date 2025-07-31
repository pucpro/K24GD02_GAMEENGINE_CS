using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score instance;

    [SerializeField] private TextMeshProUGUI _currentScoreText;

    private int _score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        _currentScoreText.text = _score.ToString();
    }

    public void UpdateScore()
    {
        _score++;
        _currentScoreText.text = _score.ToString();
    }
}