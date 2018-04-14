using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLights : MonoBehaviour {

    public ReverseGravity reverseGravity;
    private Light _light;
    private bool flickerInEffect = false;

	// Use this for initialization
	void Start ()
    {
        _light = GetComponent<Light>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckForLighFlicker();
    }

    void CheckForLighFlicker()
    {
        if (reverseGravity.inReversableGravityZone)
        {
            if (!flickerInEffect)
                StartCoroutine(LightFlicker());

            if (reverseGravity.nextGravityShiftIn > 1)
            {
                _light.enabled = true;
                flickerInEffect = false;
            }
        }
    }

    IEnumerator LightFlicker()
    {
        while (reverseGravity.nextGravityShiftIn <= 1)
        {
            flickerInEffect = true;
            _light.enabled = !_light.enabled;
            yield return new WaitForSeconds(0.15f);
        }
    }
}
