using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Morale : MonoBehaviour {

    [HideInInspector]
    public float moraleM = 1f;
    [HideInInspector]
    public float moraleS = 30f;

    public Text time;

    public Slider moraleSlider;

    public Lives lives;

    void Update()
    {
        moraleS -= Time.deltaTime;

        if (moraleS > 59)
        {
            if (moraleM < 3)
            {
                moraleM++;
                if (moraleM >= 3)
                {
                    moraleS = 0;
                }
                else
                {
                    moraleS = moraleS - 59;
                }
            }
        }

        if (moraleS <= 0)
        {
            moraleM--;
            moraleS = 59;
        }

        moraleSlider.value = moraleM * 59 + moraleS;

        if (Mathf.Round(moraleS) <= 9)
        {
            if (moraleM > 0)
            {
                time.text = Mathf.Round(moraleM) + ":0" + Mathf.Round(moraleS);
            }
            else if (moraleM == 0)
            {
                time.text = "0:" + "0" + Mathf.Round(moraleS);
            }
        }
        else
        {
            if (moraleM > 0)
            {
                time.text = Mathf.Round(moraleM) + ":" + Mathf.Round(moraleS);
            }
            else if (moraleM == 0)
            {
                time.text = "0:" + Mathf.Round(moraleS).ToString();
            }
        }

        if (Mathf.Round(moraleM) <= 0 && Mathf.Round(moraleS) <= 0)
        {
            lives.lives = 0;
        }
	}
}
