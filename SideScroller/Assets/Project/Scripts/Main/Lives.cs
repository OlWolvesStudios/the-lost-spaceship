using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Lives : MonoBehaviour {

    public Health health;

    public int lives = 3;

	void Update ()
    {
        if (lives <= 0)
        {
            SceneManager.LoadScene(2);
        }
	}
}
