using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableEnvironment : MonoBehaviour {

    private const float DASH_LIMIT = 0.2f; 

    public GunMechanic gun;
    public GameObject[] spawnOnDeath;

    public int health = 1;

    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
            SpawnOnDeath();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gun.dashing && gun.dashIsCounting)
            {
                health--;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gun.dashing && gun.dashIsCounting)
            {
                health--;
            }
        }
    }

    void SpawnOnDeath()
    {
        if (spawnOnDeath.Length != 0)
        {
            foreach (GameObject obj in spawnOnDeath)
                Instantiate(obj, transform.position, Quaternion.Euler(Vector3.zero));
        }
    }
}
