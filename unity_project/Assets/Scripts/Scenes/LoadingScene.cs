using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class LoadingScene : Singleton<LoadingScene>
{
    /*
    This script is used to load scenes asynchronously,
    and display the loading screen with a progress bar.
    */

    // Reference to the AuthenticationManager instance
    private AuthenticationManager authenticationManager;
    // Reference to the ErrorNotifications
    public ErrorNotifications errorNotifications;

    // UI Elements
    public GameObject LoadingScreen;
    public Slider slider;

    // Method to load a scene asynchronously
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Coroutine to load a scene asynchronously
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // If the current scene is Login scene and the scene to be loaded is the main menu scene, authenticate the user first
        if (SceneManager.GetActiveScene().buildIndex == 0 && sceneName == "MainMenu")
        {
            yield return AuthenticateAndLoadMainMenu(sceneName);
        }
        // If the current scene is not the Login scene, load the scene directly
        else
        {
            yield return LoadOtherScene(sceneName);
        }
    }

    // Coroutine to authenticate the user and load the main menu scene
    private IEnumerator AuthenticateAndLoadMainMenu(string sceneName)
    {     
        // Call Authenticate method from AuthenticationManager to send the authentication request to the server and get the JWT token
        authenticationManager = ScriptableObject.CreateInstance<AuthenticationManager>();
        authenticationManager.Authenticate(this);

        while (!authenticationManager.isRequestCompleted)
        {
            yield return null;
        }

        if (authenticationManager.isRequestSuccessful)
        {
            // After the user is authenticated, proceed to load the main menu scene
            StartCoroutine(LoadOtherScene(sceneName));
        }
        else
        {
            // If the authentication is not successful,
            Debug.Log("Authentication failed.");
            // Display the Error message
            errorNotifications.DisplayErrorMessage(authenticationManager);
        }
    }

    // Coroutine to load a scene other than the main menu
    private IEnumerator LoadOtherScene(string sceneName)
    {
        slider.value = 0;

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        LoadingScreen.SetActive(true);

        // Wait for scene to finish loading
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / (0.9f));
            slider.value = progressValue ;
            yield return null;
        }

        LoadingScreen.SetActive(false);
    }
}