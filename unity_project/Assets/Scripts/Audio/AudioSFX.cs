using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class AudioSFX : MonoBehaviour
{
    /*
    This script is used to manage the sound effects of the game using the settings panel.
    */

    // Audio source for the sound
    [SerializeField] private  AudioSource audioSource0;      
    [SerializeField] private  AudioSource audioSource1;
    [SerializeField] private  AudioSource audioSource2;
    [SerializeField] private  AudioSource audioSource3;
    [SerializeField] private Slider volumeSlider;  // Volume slider
    
    public void SetVolume()
    {
        float volume = volumeSlider.value;   // Get the volume from the slider

        // Set the volume for all the audio sources
        audioSource0.volume = volume;             
        audioSource1.volume = volume;
        audioSource2.volume = volume;
        audioSource3.volume = volume;
    }
}
