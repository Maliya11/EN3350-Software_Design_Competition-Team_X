using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private  AudioSource audioSource0;    // Audio source for the sound

    [SerializeField] private Slider volumeSlider;      // Volume slider

    public void SetVolume()
    {
        float volume = volumeSlider.value;      // Get the volume from the slider
        audioSource0.volume = volume;         // Set the volume for the audio source
    }



}
