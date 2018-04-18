using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    public CameraFollow offset;

    Vector3 originalPos;

    public float shakeDuration = 0;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    
    void Start()
    {
        originalPos = transform.localPosition;    
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
        }
    }

}
