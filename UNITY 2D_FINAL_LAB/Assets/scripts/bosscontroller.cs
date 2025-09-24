using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    [SerializeField] private GameObject deathAnimation;

    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;
    [SerializeField] private Transform pos3;
    [SerializeField] private float shootInterval = 2f;

    [Header("Circle Bullet Skill")]
    [SerializeField] private int circleBulletCount = 12;
    [SerializeField] private float circleBulletSpeed = 5f;
    [SerializeField] private float circleSkillInterval = 6f;

    private bool isActive = false; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ActivateBoss()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(ShootRoutine());
            StartCoroutine(CircleBulletRoutine());
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isActive) return; 

        currentHealth -= damage;
        StartCoroutine(HitEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    IEnumerator HitEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 5; i++)
        {
            sr.color = new Color(1, 1, 1, 0.5f); 
            yield return new WaitForSecondsRealtime(0.1f);
            sr.color = new Color(1, 1, 1, 1f);   
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }


    private void Die()
    {
        SoundManager.Instance.PlayEnemyDeathSound();
        GameObject effect = Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f); 
        Destroy(gameObject);
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);
            ShootRandomPattern();
        }
    }

    void ShootRandomPattern()
    {
        int pattern = Random.Range(0, 3);

        switch (pattern)
        {
            case 0:
                ShootFrom(pos1);
                ShootFrom(pos2);
                break;
            case 1:
                ShootFrom(pos2);
                ShootFrom(pos3);
                break;
            case 2:
                ShootFrom(pos1);
                ShootFrom(pos3);
                break;
        }
    }

    void ShootFrom(Transform firePoint)
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    IEnumerator CircleBulletRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(circleSkillInterval);
            ShootCircleBullets();
        }
    }

    void ShootCircleBullets()
    {
        Vector3 origin = transform.position;

        for (int i = 0; i < circleBulletCount; i++)
        {
            float angle = i * (360f / circleBulletCount);
            float rad = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject bullet = Instantiate(bulletPrefab, origin, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * circleBulletSpeed;
        }
    }
}