using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    // Reference to the AuthenticationManager instance
    private AuthenticationManager authenticationManager;


    // Adjust the loading time
    public float adjustLoadingTime = 1f;
    private float target;


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
        if (SceneManager.GetActiveScene().buildIndex == 0 && sceneID == 1)
        {
            yield return AuthenticateAndLoadMainMenu(sceneID);
        }
        else
        {
            yield return LoadOtherScene(sceneID);
        }
    }

    // Coroutine to authenticate the user and load the main menu scene
    private IEnumerator AuthenticateAndLoadMainMenu(int sceneID)
    {     
        // Call Authenticate method from AuthenticationManager
        authenticationManager = ScriptableObject.CreateInstance<AuthenticationManager>();
        authenticationManager.Authenticate(this);

        LoadingScreen.SetActive(true);

        while (!authenticationManager.IsAuthenticated)
        {
            yield return null;
        }

        LoadingScreen.SetActive(false);

        // Proceed to load the main menu scene
        StartCoroutine(LoadOtherScene(sceneID));
    }

    // Coroutine to load a scene other than the main menu
    private IEnumerator LoadOtherScene(int sceneID)
    {
        target = 0;
        slider.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        LoadingScreen.SetActive(true);

        // Wait for scene to finish loading
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            target = progressValue;
            yield return null;
        }

        LoadingScreen.SetActive(false);
    }

    // Update method to handle slider animation
    private void Update()
    {
        slider.value = Mathf.MoveTowards(slider.value, target, Time.deltaTime * adjustLoadingTime);
    }
}