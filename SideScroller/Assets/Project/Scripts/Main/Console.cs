using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour {

    public Text text;

    bool inRange = false;

    void Update()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log("Door Opens...");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.text = "Press Shift To Open The Door";
            inRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.text = "";
        }
    }
}
