using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Waves & Boss")]
    public EnemyGroupController[] wavePrefabs; 
    public GameObject bossPrefab;             
    public Transform spawnPoint;               

    private int currentWave = 0;
    private GameObject currentWaveObj;
    private bool bossSpawned = false;
    private bool bossDefeated = false;

    void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        if (currentWaveObj == null)
        {
            if (bossSpawned && !bossDefeated)
            {
                bossDefeated = true;
                return;
            }

            NextWave();
        }
    }

    void SpawnWave()
    {
        if (currentWave < wavePrefabs.Length)
        {
            currentWaveObj = Instantiate(wavePrefabs[currentWave].gameObject, spawnPoint.position, Quaternion.identity);
        }
        else if (!bossSpawned)
        {
            currentWaveObj = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            bossSpawned = true;
        }
    }

    void NextWave()
    {
        currentWave++;
        SpawnWave();
    }
}