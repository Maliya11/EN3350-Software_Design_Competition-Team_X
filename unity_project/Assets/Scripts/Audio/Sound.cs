using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound 
{
	// Name of the sound
	public string name;

	// Audio clip associated with the sound
	public AudioClip clip;

	// Mixer group to which the sound belongs
	public AudioMixerGroup mixer;

	// Volume of the sound (0.0 to 1.0)
	[Range(0f, 1f)]
	public float volume = 1;

	// Pitch of the sound (-3.0 to 3.0)
	[Range(-3f, 3f)]
	public float pitch = 1;

	// Flag to set if the sound should loop or not
	public bool loop = false;

	// Audiosource for the sound
	[HideInInspector]
	public AudioSource source;
}
