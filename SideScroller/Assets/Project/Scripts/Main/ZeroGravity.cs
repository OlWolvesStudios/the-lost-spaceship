using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravity : MonoBehaviour {

    public bool inZeroGravityZone = false;
	
	void Update ()
    {
        if (inZeroGravityZone)
        {
            Time.timeScale = 0.7f;
            Physics.gravity = new Vector3(0, -0.1f, 0);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZeroGravityZone"))
        {
            inZeroGravityZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZeroGravityZone"))
        {
            inZeroGravityZone = false;
            Time.timeScale = 1f;
            Physics.gravity = new Vector3(0, -25, 0);
        }
    }
}
