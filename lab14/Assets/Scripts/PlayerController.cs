using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class playercontrol : MonoBehaviour
{
    [Header("Prefabs đạn (3 loại)")]
    public GameObject[] bulletPrefabs;
    public GameObject BulletPosition;

    [Header("Hiệu ứng")]
    public GameObject ShieldEffect;

    [Header("Di chuyển")]
    public float speed = 5f;

    [Header("Bắn nhiều viên")]
    public int bulletCount = 1;
    public float bulletSpreadAngle = 15f;

    [Header("Sát thương đạn")]
    public int bulletDamage = 20;
    private int powerUpCount = 0;

    [Header("Mạng sống")]
    public int lives = 3;
    public TMP_Text livesText; 

    [Header("UI Game Over")]
    public GameObject gameOverUI; 

    private int currentBulletIndex = 0;
    private bool isProtected = false;

    void Start()
    {
        UpdateLivesUI();
        if (gameOverUI != null)
            gameOverUI.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Move(new Vector2(x, y).normalized);
    }

    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x -= 0.225f; min.x += 0.225f;
        max.y -= 0.285f; min.y += 0.285f;

        Vector2 pos = transform.position + (Vector3)(direction * speed * Time.deltaTime);
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    void Shoot()
    {
        if (bulletPrefabs.Length == 0) return;
        GameObject bulletPrefab = bulletPrefabs[currentBulletIndex];

        if (bulletCount == 1)
        {
            SpawnBullet(bulletPrefab, Quaternion.identity);
        }
        else
        {
            float startAngle = -(bulletSpreadAngle * (bulletCount - 1) / 2);
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + i * bulletSpreadAngle;
                Quaternion rot = Quaternion.Euler(0, 0, angle);
                SpawnBullet(bulletPrefab, rot);
            }
        }
    }

    void SpawnBullet(GameObject prefab, Quaternion rotation)
    {
        GameObject bullet = Instantiate(prefab, BulletPosition.transform.position, rotation);
        bullet bulletScript = bullet.GetComponent<bullet>();
        if (bulletScript != null)
            bulletScript.damage = bulletDamage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PowerUpBullet"))
        {
            powerUpCount++;
            if (powerUpCount == 1) bulletDamage += 10;
            else if (powerUpCount == 2) bulletDamage += 20;

            currentBulletIndex = Mathf.Min(currentBulletIndex + 1, bulletPrefabs.Length - 1);
            Destroy(col.gameObject);
            return;
        }

        if (isProtected) return;

        if (col.CompareTag("Enemy") || col.CompareTag("EnemyBullet"))
        {
            LoseLife();
        }
    }

    void LoseLife()
    {
        lives--;
        UpdateLivesUI();

        if (lives > 0)
        {
            ActivateShield();
        }
        else
        {
            GameOver();
        }
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "X" + lives;
    }

    void ActivateShield()
    {
        isProtected = true;

        if (ShieldEffect != null)
        {
            GameObject shield = Instantiate(ShieldEffect, transform.position, Quaternion.identity);
            shield.transform.SetParent(transform);
            Destroy(shield, 1.5f);
        }

        Invoke(nameof(DisableProtection), 1.5f);
    }

    void DisableProtection()
    {
        isProtected = false;
    }

    void GameOver()
    {
        gameObject.SetActive(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
        Time.timeScale = 0;

        Debug.Log("GAME OVER");
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("ck");
        Time.timeScale = 1;
    }
}