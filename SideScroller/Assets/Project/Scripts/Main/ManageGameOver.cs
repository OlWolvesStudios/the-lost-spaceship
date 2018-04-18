using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageGameOver : MonoBehaviour {

    public Lives lives;

    public GameObject gameOver;

    public GameObject player;

    public GameObject HUD;

    public PauseMenu pauseMenu;

    public bool playerIsDead = false;

    float fadeTime = 3;

    void Update()
    {
        if (lives.lives <= 0)
        {
            pauseMenu.canBePaused = false;
            playerIsDead = true;
            HUD.SetActive(false);
            Destroy(player);
            gameOver.SetActive(true);

            fadeTime -= Time.deltaTime;

            if (fadeTime <= 0)
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}