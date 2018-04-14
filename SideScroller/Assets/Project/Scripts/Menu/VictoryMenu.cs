using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour {

    public GameObject panels;
    public GameObject fadeOut;

    private float fadeTime = 8.75f;

    private bool animFinished = false;
    private bool isLoading = false;

	void Start ()
    {
        panels.SetActive(true);
	}

    void Update()
    {
        if (!animFinished)
        {
            fadeTime -= Time.deltaTime;

            if (fadeTime <= 0)
            {
                panels.SetActive(false);
                animFinished = true;
            }
        }

        if (animFinished)
        {
            if (!isLoading)
            {
                if (Input.anyKeyDown)
                {
                    StartCoroutine(LoadMenu());
                }
            }
        }
    }

    IEnumerator LoadMenu()
    {
        isLoading = true;
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
        yield return null;
    }
}
