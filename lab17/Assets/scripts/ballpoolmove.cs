using UnityEngine;

public class BallPoolMover : MonoBehaviour
{
    public float speed = 0.1f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}