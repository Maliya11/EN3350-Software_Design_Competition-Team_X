using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManagerPlayer : MonoBehaviour 
{
	/*
	This script is used to manage the sound effects for the player.
	*/

	// Instance of the AudioManagerPlayer
	public static AudioManagerPlayer instance; 

	public Sound[] sounds;  // Array of sounds
	[SerializeField] private Slider volumeSlider; // Volume slider
	
	private void Awake ()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		} else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			// Add an AudioSource component to the game object and set the properties
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	public void Play(string sound)
	{
		// Find the sound in the array and play it
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Play();
	}

	public void Stop(string sound)
	{
		// Find the sound in the array and stop it
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Stop();
	}

	public void SetVolumeAll()          
    {
		// Set the volume for all sounds
		float volume = volumeSlider.value;
        foreach (Sound s in sounds)
        {

            s.source.volume = volume;
        }
    }

}
