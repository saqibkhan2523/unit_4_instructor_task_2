using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    public Vector3 offset;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Optionally, to have the camera also smoothly rotate towards the player's direction
        // transform.LookAt(player.transform);
    }
}
