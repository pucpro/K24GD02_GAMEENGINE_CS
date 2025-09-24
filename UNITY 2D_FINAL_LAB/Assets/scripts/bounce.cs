using UnityEngine;

public class bounce : MonoBehaviour
{
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private Rigidbody2D playerRb;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("item"))
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
        }
    }
}