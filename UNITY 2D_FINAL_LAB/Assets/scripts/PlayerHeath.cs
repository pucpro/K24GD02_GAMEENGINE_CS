using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    private bool isInvincible = false;
    [SerializeField] private float invincibleDuration = 2f;

    [Header("UI Settings")]
    [SerializeField] private GameObject gameOverPanel;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        currentHealth--;
        GameManager.Instance.TakeDamage();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityTimer());
        }
    }
   IEnumerator InvincibilityTimer()
{
    isInvincible = true;

    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    for (int i = 0; i < 5; i++)
    {
        sr.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSecondsRealtime(0.2f);
        sr.color = new Color(1, 1, 1, 1f);
        yield return new WaitForSecondsRealtime(0.2f);
    }

    isInvincible = false;
}

    public void Die()
    {
        GetComponent<Move>().enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}