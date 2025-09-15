using UnityEngine;
using UnityEngine.SceneManagement;

public class item : MonoBehaviour
{
    public float fallSpeed = 2f;

    void Update()
    {
        // Rơi xuống
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Xóa nếu ra khỏi màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    // Ăn item
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // Tăng điểm
            ScoreManager.instance.AddScore(10);

            // Xóa item
            Destroy(gameObject);
        }
    }
}