using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundWrap : MonoBehaviour {

    public float scrollSpeed = 0.5f;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update ()
    {
        float offset = -scrollSpeed * Time.deltaTime;
        rend.material.mainTextureOffset += new Vector2(offset, 0);
	}
}
