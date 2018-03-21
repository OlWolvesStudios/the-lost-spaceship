using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollow : MonoBehaviour {

    public Transform reticle;

    void Update ()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        transform.LookAt(reticle);
    }
}
