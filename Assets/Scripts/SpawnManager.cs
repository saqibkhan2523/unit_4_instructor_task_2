using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject bombPrefab;
    public GameObject enemyPrefab;

    public GameObject[] powerUps;

    public bool spawnBomb = false;

    private GameObject player;

    private int waveNumber = 1;

    private float spawnRange = 17f;

    public int enemiesPresentInGame = 0;

    public bool gameOver = false;

    public float enemyWaveHealthRate = 10;

    public float newWaveSpawnTime = 3f;
    private float timer = 0;

    private float powerUpSpawnTimer = 5f;
    private float powerUpSpawnTimerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            powerUpSpawnTimerCount += Time.deltaTime;
            if(powerUpSpawnTimerCount > powerUpSpawnTimer)
            {
                int randomIndexForPowerUp = Random.Range(0, powerUps.Length);
                Instantiate(powerUps[randomIndexForPowerUp], GenerateRandomSpawnPosition(), powerUps[randomIndexForPowerUp].transform.rotation);
                powerUpSpawnTimerCount = 0;
            }

            if (enemiesPresentInGame <= 0)
            {
                timer += Time.deltaTime;
                if (timer >= newWaveSpawnTime)
                {
                    SpawnEnemy();
                }
            }
        }
    }

    public GameObject SpawnBomb(Transform parentTransform)
    {
        if(parentTransform.CompareTag("Enemy"))
            Debug.Log("2");
        Vector3 bombSpawnPosition = new Vector3(parentTransform.position.x, -1.7f, parentTransform.position.z);
        GameObject bomb = Instantiate(bombPrefab, bombSpawnPosition, parentTransform.rotation);

        bomb.transform.SetParent(parentTransform);

        return bomb;
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, GenerateRandomSpawnPosition(), enemyPrefab.transform.rotation);
            enemy.GetComponent<Enemy>().enemyHealth += (waveNumber - 1) * enemyWaveHealthRate;
        }
        enemiesPresentInGame = waveNumber;
        waveNumber++;
        timer = 0;
        //StopCoroutine(DelayInSpawningEnemy());
    }

    Vector3 GenerateRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
    }

    IEnumerator DelayInSpawningEnemy()
    {
        yield return new WaitForSeconds(3f);
        SpawnEnemy();
    }
}
