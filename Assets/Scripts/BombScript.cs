using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float destroyBombTime = 7f;

    public float radius = 5.0f;
    public float power = 100.0f;

    public Rigidbody bombRb;
    public float throwSpeed = 5f;

    public bool isSticky = false;

    public GameObject explosionPS;

    // Start is called before the first frame update
    void Start()
    {
        bombRb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyBomb());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ThrowBomb()
    {
        transform.parent = null;
        if (bombRb != null)
        {
            Debug.Log("4");
            bombRb.AddForce(Vector3.forward * throwSpeed);
        }
    }

    void ExplodeBomb()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            //Debug.Log("Name: " + hit.gameObject.name);
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f);
                if (hit.gameObject.CompareTag("Player"))
                {
                    hit.gameObject.GetComponent<PlayerController>().DecreaseHealth(20f);
                }

                if (hit.gameObject.CompareTag("Enemy"))
                {
                    hit.gameObject.GetComponent<Enemy>().DecreaseHealth(20f);
                }
            }
        }
        explosionPS.SetActive(true);
        Destroy(gameObject);
    }

    IEnumerator DestroyBomb()
    {
        yield return new WaitForSeconds(destroyBombTime);
        
        ExplodeBomb();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isSticky && !collision.gameObject.CompareTag("Player"))
        {

            transform.SetParent(collision.transform);
            bombRb.velocity = Vector3.zero ;
            bombRb.useGravity = false;
            Debug.Log("StickyBomb");
        }
    }


}
