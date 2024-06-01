using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
public class AudioSFX : MonoBehaviour
{
    [SerializeField] private  AudioSource audioSource0;
    [SerializeField] private  AudioSource audioSource1;
    [SerializeField] private  AudioSource audioSource2;
    [SerializeField] private Slider volumeSlider;
    // Update is called once per frame
    public void SetVolume()
    {
        float volume = volumeSlider.value;
        audioSource0.volume = volume;
        audioSource1.volume = volume;
        // audioSource2.volume = volume;
        // audioSource3.volume = volume;
        // audioSource4.volume = volume;
        
        
        
        
    }
}
