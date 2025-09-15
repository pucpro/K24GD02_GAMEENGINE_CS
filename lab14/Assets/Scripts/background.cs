using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public float scrollSpeed = 2f;   // tốc độ cuộn
    public float height = 10f;       // chiều cao ảnh nền

    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Khi nền ra khỏi màn → dịch lên trên để nối tiếp
        if (transform.position.y < -height)
        {
            Vector3 newPos = transform.position;
            newPos.y += height * 2;
            transform.position = newPos;
        }
    }
}