using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject eggBeak; // Prefab chứa animation nổ
    public float speed = 4f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        // Xóa nếu ra khỏi màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            if (eggBeak != null)
            {
                GameObject explosion = Instantiate(eggBeak, transform.position, Quaternion.identity);
                Destroy(explosion, 0.5f); // hoặc thời gian đúng với animation
            }
            Destroy(gameObject);
        }
    }
}