using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour {

    public static bool isUpgraded = false;

    public int researchCollected = 0;

    void Start()
    {
        researchCollected = 0;
        isUpgraded = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Upgrade"))
        {
            isUpgraded = true;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Research"))
        {
            researchCollected++;
            Destroy(other.gameObject);
        }
    }
}
