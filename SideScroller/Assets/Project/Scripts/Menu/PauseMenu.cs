using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;
    private bool isFading = false;

    private bool optionsOpen = false;

    public bool canBePaused = true;

    public GameObject pauseMenuUI;

    public GameObject reticle;

    public GameObject GameHUD;

    public GameObject fadeOut;

    public GameObject options;

    public GameObject messages;

    public GameObject lookAt;

    public Button defaultSelected;

    public Rigidbody rigid;

    public PlayerMove playerMove;

    public ZeroGravity zeroGravity;

    void Start()
    {
        defaultSelected.Select();
    }

    void Update ()
    {
        if (canBePaused)
        {
            if (isPaused)
            {
                if (messages.activeSelf)
                {
                    if (Input.GetButtonDown("Escape") || Input.GetButtonDown("BButton") || Input.GetButtonDown("StartButton"))
                    {
                        messages.SetActive(false);
                        optionsOpen = false;
                    }
                }
                else if (Input.GetButtonDown("Escape") || Input.GetButtonDown("BButton") || Input.GetButtonDown("StartButton"))
                {
                    Resume();
                }
            }
            else
            {
                if (Input.GetButtonDown("Escape") || Input.GetButtonDown("StartButton"))
                {
                    Pause();
                }
            }
        }
	}

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (messages.activeSelf)
        {
            messages.SetActive(false);
            optionsOpen = false;
        }
        lookAt.SetActive(true);
        reticle.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        if (zeroGravity.inZeroGravityZone)
        {
            rigid.AddForce(playerMove.playerVelocityBeforePause * 50);
        }
    }

    void Pause()
    {
        lookAt.SetActive(false);
        pauseMenuUI.SetActive(true);
        reticle.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Options()
    {
        if (!optionsOpen)
        {
            options.SetActive(true);
            optionsOpen = true;
        }
        else
        {
            options.SetActive(false);
            optionsOpen = false;
        }
    }

    public void MainMenu()
    {
        if (messages.activeSelf)
        {
            messages.SetActive(false);
        }
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
