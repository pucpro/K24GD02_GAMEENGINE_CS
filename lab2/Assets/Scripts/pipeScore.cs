using UnityEngine;

public class pipeScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Score.instance.UpdateScore();
            GameManager.instance.AddScore(1);
        }
    }
}
