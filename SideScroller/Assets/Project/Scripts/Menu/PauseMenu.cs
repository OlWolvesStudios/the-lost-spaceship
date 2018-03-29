using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;
    private bool isFading = false;

    public GameObject pauseMenuUI;

    public GameObject reticle;

    public GameObject GameHUD;

    public GameObject fadeOut;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }	
	}

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        reticle.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        reticle.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Options()
    {
        Debug.Log("Options...");
    }

    public void MainMenu()
    {
        if (!isFading)
        {
            StartCoroutine(ReturnToMenu());
        }
    }

    IEnumerator ReturnToMenu()
    {
        isFading = true;
        fadeOut.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        pauseMenuUI.SetActive(false);
        reticle.SetActive(true);
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        yield return null;
    }
}
