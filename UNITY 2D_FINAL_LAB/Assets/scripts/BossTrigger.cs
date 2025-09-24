using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private BossController boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.ActivateBoss();
            gameObject.SetActive(false); 
        }
    }
}