using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {

    public Health health;
    public GunMechanic gun;
    public Pickups pickups;
    public Lives lives;

    public GameObject[] currentLives;
    public GameObject fullHealth;
    public GameObject halfHealth;
    public GameObject lowHealth;
    public GameObject gameHud;

    public Slider rechargeSlider;
    public Slider refreshSlider;

    public GameObject rechargeSliderGO;
    public GameObject refreshSliderGO;

    public Slider dashSlider;

    public Text researchCollectedText;

    private bool showRecharge = false;
    private bool showRefresh = true;

    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            currentLives[i].SetActive(true);
        }
    }

    void Update()
    {
        if (PauseMenu.isPaused)
        {
            gameHud.SetActive(false);
        }
        else
        {
            gameHud.SetActive(true);

            DisplayLives();
            DisplayHealth();
            DisplayShotRecharge();
            DisplayDashRecharge();
            DisplayResearchCollected();
        }
    }

    void DisplayLives()
    {
        for (int i = 0; i < 3; i++)
        {
            currentLives[i].SetActive(false);
        }

        for (int i = 0; i < lives.lives; i++)
        {
            currentLives[i].SetActive(true);
        }
    }

    void DisplayHealth()
    {
        if (health.currentHealth == 3)
        {
            fullHealth.SetActive(true);
            halfHealth.SetActive(true);
            lowHealth.SetActive(true);
        }
        else if (health.currentHealth == 2)
        {
            fullHealth.SetActive(false);
            halfHealth.SetActive(true);
            lowHealth.SetActive(true);
        }
        else if (health.currentHealth == 1)
        {
            fullHealth.SetActive(false);
            halfHealth.SetActive(false);
            lowHealth.SetActive(true);
        }
    }

    void DisplayShotRecharge()
    {
        if (gun.shotsAvailable < 3 && gun.shotsAvailable > 0)
        {
            showRecharge = false;
            showRefresh = true;
        }

        if (!gun.canShoot && gun.shotsAvailable == 0)
        {
            showRefresh = false;
            showRecharge = true;
        }

        if (showRecharge)
        {
            refreshSliderGO.SetActive(false);
            rechargeSliderGO.SetActive(true);
        }
        else if (showRefresh)
        {
            rechargeSliderGO.SetActive(false);
            refreshSliderGO.SetActive(true);
        }

        if (rechargeSlider.value == rechargeSlider.maxValue)
        {
            rechargeSliderGO.SetActive(false);
            refreshSliderGO.SetActive(true);
        }

        if (showRecharge)
        {
            if (gun.shotRecharge == 0)
            {
                rechargeSlider.value = rechargeSlider.maxValue;
            }
            else
            {
                rechargeSlider.value = gun.shotRecharge;
            }
            refreshSlider.value = refreshSlider.maxValue;
        }
        if (showRefresh)
        {
            refreshSlider.value = gun.shotsAvailable;
        }
    }

    void DisplayDashRecharge()
    {
        if (gun.dashRecharge == 0)
        {
            dashSlider.value = dashSlider.maxValue;
        }
        else
        {
            dashSlider.value = gun.dashRecharge;
        }
    }

    void DisplayResearchCollected()
    {
        researchCollectedText.text = pickups.researchCollected.ToString();
    }
}

