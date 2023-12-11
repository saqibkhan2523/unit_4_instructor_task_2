using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHealth = 30f;
    public float speed = 1f;

    private GameObject player;
    private Rigidbody enemyRb;
    private GameObject spawnManager;

    private float bombSpawnTimer = 3f;
    private float spawnTimerCount = 0;
    public GameObject bomb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        spawnManager = GameObject.Find("SpawnManager");
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        if(enemyHealth <= 0)
        {
            spawnManager.GetComponent<SpawnManager>().enemiesPresentInGame--;
            Destroy(gameObject);
        }

        spawnTimerCount += Time.deltaTime;
        if(spawnTimerCount > bombSpawnTimer && transform.childCount == 0)
        {
            Debug.Log("1");
            bomb = spawnManager.GetComponent<SpawnManager>().SpawnBomb(transform);
            if (bomb != null)
            {
                Debug.Log("3");
                bomb.GetComponent<BombScript>().ThrowBomb();
            }
            spawnTimerCount = 0;
        }
        else if(spawnTimerCount > bombSpawnTimer && transform.childCount > 0)
        {
            spawnTimerCount = 0;
        }
    }


    public void DecreaseHealth(float decAmount)
    {
        enemyHealth -= decAmount;
    }
}
