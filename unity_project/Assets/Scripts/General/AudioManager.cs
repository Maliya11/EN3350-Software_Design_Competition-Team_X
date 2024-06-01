using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioSource;

    public AudioClip mainMenuClip;
    public AudioClip cgyClip;
    public AudioClip msClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Find the AudioSource component on the child object
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("AudioSource component missing from the child object. Please add an AudioSource component to the child object.");
            }
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Change the audio clip based on the loaded scene
        if (scene.name == "LoginScene" || scene.name == "MainMenu" ||  scene.name == "SelectionScene")
        {
            PlayClip(mainMenuClip);
        }
        else if (scene.name == "CursedGraveYardLevel1" || scene.name == "CursedGraveYardLevel2" || scene.name == "CursedGraveYardLevel3")
        {
            PlayClip(cgyClip);
        }
        else if (scene.name == "MysticSeaLevel1" || scene.name == "MysticSeaLevel2" || scene.name == "MysticSeaLevel3")
        {
            PlayClip(msClip);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
