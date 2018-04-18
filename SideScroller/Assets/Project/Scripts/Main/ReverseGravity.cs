using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGravity : MonoBehaviour {

    public float timeToGravifyChange = 5f;
    [HideInInspector]
    public float nextGravityShiftIn;

    public bool inReversableGravityZone = false;

    public GameObject player;

    public CameraFollow mainCamera;

    void Start()
    {
        if (Mathf.Sign(Physics.gravity.y) == 1)
        {
            Physics.gravity *= -1;
        }
        nextGravityShiftIn = timeToGravifyChange;
    }

    void Update ()
    {
        if (inReversableGravityZone)
        {
            if (Mathf.Sign(Physics.gravity.y) == -1)
            {
                player.transform.localScale = new Vector3(1, 1, 1);
                mainCamera.targetOffset.y = 2;
            }
            else
            {
                player.transform.localScale = new Vector3(1, -1, 1);
                mainCamera.targetOffset.y = -2;
            }
            
            nextGravityShiftIn -= 1 * Time.deltaTime;

            if (nextGravityShiftIn <= 0)
            {
                Physics.gravity *= -1;
                nextGravityShiftIn = timeToGravifyChange;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GravityZone"))
        {
            inReversableGravityZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GravityZone"))
        {
            inReversableGravityZone = false;
            nextGravityShiftIn = timeToGravifyChange;

            if (Mathf.Sign(Physics.gravity.y) == 1)
            {
                Physics.gravity *= -1;
            }
        }
    }
}
