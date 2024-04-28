using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class LoadingScene : Singleton<LoadingScene>
{
    // Reference to the AuthenticationManager instance
    private AuthenticationManager authenticationManager;
    // Reference to the ErrorNotifications
    public ErrorNotifications errorNotifications;

    // UI Elements
    public GameObject LoadingScreen;
    public Slider slider;


    // Method to load a scene asynchronously
    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    // Coroutine to load a scene asynchronously
    private IEnumerator LoadSceneAsync(int sceneID)
    {
        // If the current scene is Login scene and the scene to be loaded is the main menu scene, authenticate the user first
        if (SceneManager.GetActiveScene().buildIndex == 0 && sceneID == 1)
        {
            yield return AuthenticateAndLoadMainMenu(sceneID);
        }
        // If the current scene is not the Login scene, load the scene directly
        else
        {
            yield return LoadOtherScene(sceneID);
        }
    }

    // Coroutine to authenticate the user and load the main menu scene
    private IEnumerator AuthenticateAndLoadMainMenu(int sceneID)
    {     
        // Call Authenticate method from AuthenticationManager to send the authentication request to the server and get the JWT token
        authenticationManager = new AuthenticationManager();
        authenticationManager.Authenticate(this);

        while (!authenticationManager.isRequestCompleted)
        {
            yield return null;
        }

        if (authenticationManager.isRequestSuccessful)
        {
            // After the user is authenticated, proceed to load the main menu scene
            StartCoroutine(LoadOtherScene(sceneID));
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
    private IEnumerator LoadOtherScene(int sceneID)
    {
        slider.value = 0;

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

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