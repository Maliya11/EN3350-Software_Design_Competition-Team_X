using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManagerPlayer : MonoBehaviour {

	public static AudioManagerPlayer instance;     // instance of the AudioManagerPlayer

	public Sound[] sounds;             //array of sounds
	[SerializeField] private Slider volumeSlider;    // Volume slider
	void Awake ()
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
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Play();
	}
	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Stop();
	}

	public void SetVolumeAll()           // Set volume for all sounds
    {
		float volume = volumeSlider.value;
        foreach (Sound s in sounds)
        {
            s.source.volume = volume;
        }
    }

}
