using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
    public Light light;

    private float interpolationPeriod;
    private float time = 0.0f;

    // Use this for initialization
    void Start () {
        interpolationPeriod = 1.0f * Random.Range(1.0f, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            time = 0.0f;

            StartCoroutine("runFLicker");
        }

        StopCoroutine("runFlicker");
    }

    IEnumerator runFlicker()
    {
        light.intensity = light.intensity / 8;
        light.range = light.range / 8;

        yield return new WaitForSeconds(1.0f);

        light.intensity = light.intensity * 6;
        light.range = light.range * 6;

        yield return new WaitForSeconds(0.5f);

        light.intensity = light.intensity / 6;
        light.range = light.range / 6;

        yield return new WaitForSeconds(0.5f);

        light.intensity = light.intensity * 8;
        light.range = light.range * 8;

        yield return new WaitForSeconds(0.5f);

        light.intensity = light.intensity / 6;
        light.range = light.range / 6;

        yield return new WaitForSeconds(1.0f);

        light.intensity = light.intensity * 6;
        light.range = light.range * 6;
    }
}
