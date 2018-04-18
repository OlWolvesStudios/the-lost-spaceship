using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour {

    private RectTransform reticleRectTransform;

    void Awake()
    {
        reticleRectTransform = transform as RectTransform;
    }

    void Update ()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        reticleRectTransform.position = mousePosition;
    }
}
