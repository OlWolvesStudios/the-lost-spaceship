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
    public GameObject dashReady;

    public GameObject fadeIn;

    public Slider rechargeSlider;
    public Slider refreshSlider;

    public GameObject rechargeSliderGO;
    public GameObject refreshSliderGO;
    public GameObject dashSliderGO;

    public Slider dashSlider;

    private bool showRecharge = false;
    private bool showRefresh = true;

    private bool fadeInFinished = false;

    private float fadeInTime = 1.5f;

    void Start()
    {
        fadeIn.SetActive(true);

        for (int i = 0; i < 2; i++)
        {
            currentLives[i].SetActive(true);
        }
    }

    void Update()
    {
        if (!fadeInFinished)
        {
            fadeInTime -= Time.deltaTime;
        }

        if (fadeInTime <= 0)
        {
            fadeIn.SetActive(false);
            fadeInFinished = true;
        }

        if (PauseMenu.isPaused)
        {
            gameHud.SetActive(false);
            fadeIn.SetActive(false);
            fadeInFinished = true;
        }
        else
        {
            gameHud.SetActive(true);

            DisplayLives();
            DisplayHealth();
            DisplayShotRecharge();
            DisplayDashRecharge();
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
            halfHealth.SetActive(false);
            lowHealth.SetActive(false);
        }
        else if (health.currentHealth == 2)
        {
            fullHealth.SetActive(false);
            halfHealth.SetActive(true);
            lowHealth.SetActive(false);
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
            rechargeSlider.value = rechargeSlider.minValue;
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
                showRefresh = true;
                showRecharge = false;                
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
        if (Pickups.isUpgraded)
        {
            dashSliderGO.SetActive(true);

            if (gun.dashRecharge > 0)
            {
                dashReady.SetActive(false);
            }
            else
            {
                if (gun.canDash && gun.dashesAvailable != 0)
                {
                    dashReady.SetActive(true);
                }
            }

            if (gun.dashRecharge != 0)
            {
                dashSlider.value = gun.dashRecharge;
                dashSlider.maxValue = 5;
            }
        }
        else
        {
            dashSliderGO.SetActive(false);
        }
        
    }
}

