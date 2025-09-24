using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 5f;
    private Vector3 startPos;
    private bool moveRight = true;
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private GameObject deathAnimation;
    private int currentHealth;
    void Start()
    {
        startPos = transform.position;
        startPos = transform.position;
        currentHealth = maxHealth;
    }
    void Update()
    {
        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;
        if (moveRight)
        {
            transform.Translate(Vector2.right*speed*Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                moveRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                moveRight=true;
                Flip();
            }
        }
    }
    void Flip()
    {
        Vector3 scaler=transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SoundManager.Instance.PlayEnemyDeathSound();
        GameObject effect = Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f); 
        Destroy(gameObject);  
    }
}
