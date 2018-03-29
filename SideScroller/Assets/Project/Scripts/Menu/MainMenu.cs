using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject panels;

    public GameObject skipText;

    public GameObject fadeIn;

    float animTime = 21.15f;
    bool pressed = false;
    bool skip = false;

    void Start()
    {
        fadeIn.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Are you sure you want to quit?");
        }
        else if (Input.anyKeyDown)
        {
            pressed = true;
        }

        if (skip)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(1);
            }
        }

        if (pressed)
        {
            panels.SetActive(true);
            animTime -= Time.deltaTime;
            if (animTime <= 0)
            {
                pressed = false;
                SceneManager.LoadScene(1);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                skipText.SetActive(true);
                skip = true;
            }
        }
    }
}
