using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public AudioSource music;

    public Slider masterSlider;
    public Slider musicSlider;

    void Update ()
    {
        AudioListener.volume = masterSlider.value;
        music.volume = musicSlider.value;
    }
}
