﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour {

    public static bool isUpgraded = false;
    public static bool dataCollected = false;

    public Morale morale;

    public AudioClip researchGetSound;
    public AudioClip upgradeGetSound;
    private AudioSource aSource;

    public int researchCollected = 0;

    void Start()
    {
        researchCollected = 0;
        isUpgraded = false;
        aSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Upgrade"))
        {
            if (upgradeGetSound)
            {
                aSource.PlayOneShot(upgradeGetSound, 1.0f);
            }

            isUpgraded = true;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Research"))
        {
            morale.moraleS += 15f;
            if (researchGetSound)
            {
                aSource.PlayOneShot(researchGetSound, 1.0f);
            }

            dataCollected = true;
            researchCollected++;
            Destroy(other.gameObject);
        }
    }
}
