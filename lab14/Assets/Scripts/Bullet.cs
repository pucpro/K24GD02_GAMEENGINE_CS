using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        if (transform.position.y > max.y)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemy enemy = col.GetComponent<enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);

            Destroy(gameObject);
        }

        if (col.CompareTag("Boss"))
        {
            BossController boss = col.GetComponent<BossController>();
            if (boss != null)
                boss.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}