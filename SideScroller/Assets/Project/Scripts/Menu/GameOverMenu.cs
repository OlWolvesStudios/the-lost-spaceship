using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {

    public GameObject fadeIn;
    public GameObject panel;
    public GameObject fadeOut;

    private float fadeTime = 1.5f;

    private bool isLoading = false;

    void Start()
    {
        fadeIn.SetActive(true);
    }

    void Update()
    {
        fadeTime -= Time.deltaTime;

        if (fadeTime <= 0)
        {
            fadeIn.SetActive(false);
        }
        
    }

    public void TryAgain()
    {
        if (!isLoading)
        {
            StartCoroutine(PlayAgain());
        }        
    }

    public void GiveUp()
    {
        if (!isLoading)
        {
            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        isLoading = true;
        panel.SetActive(true);
        yield return new WaitForSeconds(5.45f);
        SceneManager.LoadScene(0);
        yield return null;
    }

    IEnumerator PlayAgain()
    {
        isLoading = true;
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
        yield return null;
    }
}
