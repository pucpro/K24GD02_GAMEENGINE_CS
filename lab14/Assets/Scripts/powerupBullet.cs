using UnityEngine;

public class PowerUpBullet : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        // Item rơi xuống
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Xóa khi ra khỏi màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }
}