using UnityEngine;

public class EnemyGroupController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 5; 
    public float moveSpeed = 2f;

    private Vector3 direction = Vector3.right;

    void Start()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        SpawnEnemies();
    }

    void Update()
    {
        MoveHorizontal();
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

    void MoveHorizontal()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        foreach (Transform enemy in transform)
        {
            if (enemy != null)
            {
                if (enemy.position.x < min.x + 0.5f && direction.x < 0)
                {
                    direction = Vector3.right;
                    break;
                }
                else if (enemy.position.x > max.x - 0.5f && direction.x > 0)
                {
                    direction = Vector3.left;
                    break;
                }
            }
        }
    }

    void SpawnEnemies()
    {
        int mid = numberOfEnemies / 2;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float offsetY = -Mathf.Abs(i - mid) * 0.5f; 
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position + new Vector3(i, offsetY, 0), Quaternion.identity);
            newEnemy.transform.parent = transform;
        }
    }
}