using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 50f;
    public float playerHealth = 100f;

    private GameObject spawnManager;
    private float SpawnDelay = 2f;
    private float timer = 0;
    private bool startbombSpawnTimer = false;

    private bool multipleBombPowerUp = false;
    private float multipleBombPowerUpTimer = 10f;
    private float multipleBombPowerUpTimerCount = 0;

    private int lives = 3;

    public GameObject bomb;

    private bool stickyBombPowerUp = false;
    private int stickBombs = 3;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnManager.GetComponent<SpawnManager>().gameOver)
        {
            PlayerMovement();

            BombControls();

            MultipleBombPowerUpControls();

            StickyBombPowerUpControls();
        }

        if (playerHealth <= 0)
        {
            if(lives <= 0)
            {
                spawnManager.GetComponent<SpawnManager>().gameOver = true;
                Destroy(gameObject);
            }
            lives--;
        }

    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * Time.deltaTime * horizontalInput * rotationSpeed);
        transform.Translate(Vector3.forward * Time.deltaTime * VerticalInput * speed);
    }

    void BombControls()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !spawnManager.GetComponent<SpawnManager>().gameOver && (timer == 0 || multipleBombPowerUp) && transform.childCount == 0)
        {
            bomb = spawnManager.GetComponent<SpawnManager>().SpawnBomb(transform);
            if(stickyBombPowerUp)
            {
                bomb.GetComponent<BombScript>().isSticky = true;
            }
            startbombSpawnTimer = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && transform.childCount > 0)
        {
            bomb.GetComponent<BombScript>().ThrowBomb();
            if(stickBombs > 0)
            {
                stickBombs--;
                Debug.Log(stickBombs);
            }
        }

        if (startbombSpawnTimer)
        {
            timer += Time.deltaTime;
        }

        if (timer > SpawnDelay)
        {
            timer = 0;
            startbombSpawnTimer = false;
        }
    }

    void MultipleBombPowerUpControls()
    {
        if(multipleBombPowerUp)
        {
            multipleBombPowerUpTimerCount += Time.deltaTime;
            if(multipleBombPowerUpTimerCount > multipleBombPowerUpTimer)
            {
                multipleBombPowerUp = false;
                multipleBombPowerUpTimerCount = 0;
            }
        }
    }

    void StickyBombPowerUpControls()
    {
        if(stickyBombPowerUp && stickBombs <= 0)
        {
            stickyBombPowerUp = false;
        }
        
    }

    public void DecreaseHealth(float decAmount)
    {
        playerHealth -= decAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Multiple Bomb"))
        {
            multipleBombPowerUp = true;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Lives"))
        {
            lives++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Sticky Bomb"))
        {
            stickyBombPowerUp = true;
            stickBombs = 3;
            Destroy(collision.gameObject);
        }
    }
}
