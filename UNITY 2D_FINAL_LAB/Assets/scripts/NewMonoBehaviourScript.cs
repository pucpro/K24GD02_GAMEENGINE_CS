using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private GameObject deathAnimation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerRb == null)
            {
                playerRb = collision.GetComponent<Rigidbody2D>();
            }

            if (playerRb != null)
            {
                GameObject effect = Instantiate(deathAnimation, transform.position, Quaternion.identity);
                Destroy(effect, 0.5f); 
                Destroy(transform.parent.gameObject);
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
            }
            else
            {
                Debug.LogError("Không tìm thấy Rigidbody2D của Player!");
            }
        }
    }
}