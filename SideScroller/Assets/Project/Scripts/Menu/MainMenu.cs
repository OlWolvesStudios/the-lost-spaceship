using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Sounds
    public AudioSource aSource = null;
    public AudioSource bSource = null;

    public GameObject panels;

    public GameObject skipText;

    public GameObject fadeIn;

    public GameObject fadeOut;

    float animTime = 46.25f;
    bool pressed = false;
    bool skip = false;

    void Start()
    {
        fadeIn.SetActive(true);
        if(aSource.clip)
        {
            aSource.Play();
        }
        if (bSource.clip)
        {
            bSource.Play();
            bSource.mute = true;
        }
    }

    void Update()
    {
        if (skip)
        {
            if (Input.GetButtonDown("Escape") || Input.GetButtonDown("BButton") || Input.GetButtonDown("StartButton"))
            {
                StartCoroutine(LoadLevel());
            }
        }

        if (pressed)
        {
            aSource.mute = true;
            bSource.mute = false;

            panels.SetActive(true);
            animTime -= Time.deltaTime;
            if (animTime <= 0)
            {
                pressed = false;
                SceneManager.LoadScene(1);
            }
            else if (Input.GetButtonDown("Escape") || Input.GetButtonDown("BButton") || Input.GetButtonDown("StartButton"))
            {
                skipText.SetActive(true);
                skip = true;
            }
        }
        if (Input.anyKeyDown)
        {
            pressed = true;
        }
    }

    IEnumerator LoadLevel()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
        yield return null;
    }
}
