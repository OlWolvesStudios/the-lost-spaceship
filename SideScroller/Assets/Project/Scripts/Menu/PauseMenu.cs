using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    public GameObject reticle;

    public GameObject GameHUD;

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
        SceneManager.LoadScene(0);
    }
}
