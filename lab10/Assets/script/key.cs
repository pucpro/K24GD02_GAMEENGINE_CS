using UnityEngine;

public class key : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (winPanel != null)
                winPanel.SetActive(true); 
            Destroy(gameObject); 
        }
    }
}