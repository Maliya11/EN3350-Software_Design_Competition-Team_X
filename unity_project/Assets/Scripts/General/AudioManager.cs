using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Static instance of AudioManager
    public static AudioManager instance;

    // Audio sources for different scenes
    public AudioClip mainMenuClip;
    public AudioClip cgyClip;
    public AudioClip msClip;

    // Audio source to play the clips
    private AudioSource audioSource;

    private void Awake()
    {
        // Check if an instance of AudioManager already exists
        if (instance == null)
        {
            // If not, set the instance to this instance 
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Add AudioSource component
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;  // Ensure the audio loops

            // Subscribe to the sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Initialize the audio based on the starting scene
            Scene currentScene = SceneManager.GetActiveScene();
            OnSceneLoaded(currentScene, LoadSceneMode.Single);
        }
        else
        {
            // If an instance already exists, destroy this instance
            Destroy(this.gameObject);
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
        // Stop the current audio
        audioSource.Stop();

        // Play the appropriate audio based on the scene
        if (scene.name == "LoginScene" || scene.name == "MainMenu" || scene.name == "SelectionScene")
        {
            audioSource.clip = mainMenuClip;
        }
        else if (scene.name == "CursedGraveYardLevel1" || scene.name == "CursedGraveYardLevel2" || scene.name == "CursedGraveYardLevel3")
        {
            audioSource.clip = cgyClip;
        }
        else if (scene.name == "MysticSeaLevel1" || scene.name == "MysticSeaLevel2" || scene.name == "MysticSeaLevel3")
        {
            audioSource.clip = msClip;
        }
        audioSource.Play();
    }
}
