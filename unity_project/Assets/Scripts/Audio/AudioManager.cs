using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Static instance of AudioManager
    public static AudioManager instance;

    // Audio source to play the clips
    private AudioSource audioSource;

    // Audio clip for the shared scenes
    public AudioClip audioClip;

    private void Awake()
    {
        // Check if an instance of AudioManager already exists
        if (instance == null)
        {
            // If not, set the instance to this instance
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Get the AudioSource component
            audioSource = GetComponent<AudioSource>();

            // Subscribe to the sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Start playing the audio initially
            PlayAudio();
        }
        else
        {
            // If an instance already exists, destroy this instance
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the scene is LoginScene, MainMenu, or SelectionScene
        if (scene.name == "LoginScene" || scene.name == "MainMenu" || scene.name == "SelectionScene")
        {
            // Only set and play the clip if it's not already playing
            if (audioSource.clip != audioClip || !audioSource.isPlaying)
            {
                PlayAudio();
            }
        }
        else
        {
            // Stop the audio if it's not one of the specified scenes
            audioSource.Stop();
        }
    }

    // Method to set the audioclip to the source and play it
    private void PlayAudio()
    {
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
