using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageWin : MonoBehaviour {

    public GameObject fadeOut;

    private bool isLoading = false;

    public PauseMenu pauseMenu;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isLoading)
            {
                StartCoroutine(EndGame());
            }
        }
    }

    IEnumerator EndGame()
    {
        pauseMenu.canBePaused = false;
        isLoading = true;
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(3);
        yield return null;
    }
}
