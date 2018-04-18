using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootAtController : MonoBehaviour {

    void Update()
    {
        transform.position = new Vector3(Input.GetAxis("RightJoystickX"), Input.GetAxis("RightJoystickY"), 1);
    }
}
