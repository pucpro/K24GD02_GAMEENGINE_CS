using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Chỉ số")]
    public int hp = 500;

    [Header("Bắn")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float nextFireTime;

    [Header("Di chuyển")]
    public float moveSpeed = 2f;
    private Vector3 moveDirection = Vector3.right; 

    void Update()
    {
        Move();
        ShootLogic();
    }

    void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector3 pos = transform.position;
        if (pos.x < min.x + 0.5f && moveDirection.x < 0)
        {
            moveDirection.x = -moveDirection.x;
        }
        else if (pos.x > max.x - 0.5f && moveDirection.x > 0)
        {
            moveDirection.x = -moveDirection.x;
        }
        if (pos.y > max.y - 0.5f && moveDirection.y > 0)
        {
            moveDirection.y = -moveDirection.y;
        }
        else if (pos.y < max.y - 2f && moveDirection.y < 0) 
        {
            moveDirection.y = -moveDirection.y;
        }
    }

    void ShootLogic()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}