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

    private string[] continuousScenes = { "LoginScene", "MainMenu", "SelectionScene" };
    private string currentSceneName;

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
            audioSource.loop = true;

            // Subscribe to the sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Initialize the current scene name
            currentSceneName = SceneManager.GetActiveScene().name;
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
        // Check if the new scene is one of the continuous scenes
        bool isContinuousScene = IsContinuousScene(scene.name);

        // If the current scene is a continuous scene and the new scene is also a continuous scene, do nothing
        if (IsContinuousScene(currentSceneName) && isContinuousScene)
        {
            currentSceneName = scene.name;
            return;
        }

        // Update the current scene name
        currentSceneName = scene.name;

        // Stop the current audio
        audioSource.Stop();

        // Play the appropriate audio based on the scene
        if (isContinuousScene)
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

        // Play the selected clip if it is not already playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private bool IsContinuousScene(string sceneName)
    {
        foreach (string continuousScene in continuousScenes)
        {
            if (sceneName == continuousScene)
            {
                return true;
            }
        }
        return false;
    }
}
