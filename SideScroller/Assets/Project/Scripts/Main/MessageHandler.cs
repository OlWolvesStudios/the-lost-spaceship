using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler : MonoBehaviour {

    // NOTE: Always keep intro message to 0, upgrade message to 1 and first data to 2 and final data to 3
    public GameObject[] upgradeMessage;

    public GameObject data;

    public Pickups pickups;

    public LookAt lookAt;

    // Use this for initialization
    void Start ()
    {
		upgradeMessage[0].SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Pickups.isUpgraded)
        {
            if (lookAt.controller)
            {
                upgradeMessage[4].SetActive(true);
            }
            else
            {
                upgradeMessage[1].SetActive(true);
            }
        }
        if (Pickups.dataCollected)
        {
            if (pickups.researchCollected == 1)
            {
                upgradeMessage[2].SetActive(true);
            }
            else if (data.transform.childCount == 0)
            {
                upgradeMessage[3].SetActive(true);
            }
        }
    }
}
