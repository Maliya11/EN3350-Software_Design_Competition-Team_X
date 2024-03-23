using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider slider;
    
    private float adjustLoadingTime = 10.0f;
    private float target;

    public void LoadScene(int sceneID) 
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    /// If the sceneID is 1, the method will authenticate the user and load the main menu scene.
    /// For loading the main menu from login page, timeout is set to 5 seconds.
    /// If the timeout is passed and the token is not received, will be directed back to the login page.
    public IEnumerator LoadSceneAsync(int sceneID)
    {
        if (sceneID == 1)
        {
            target = 0;
            slider.value = 0;
            float startTime = Time.time;
            float authenticationTimeout = 5.0f; // seconds

            AsyncOperation operation = null;
            LoadingScreen.SetActive(true);

            StartCoroutine(Authenticate((token) =>
            {
                // Store the token in PlayerPrefs
                PlayerPrefs.SetString("JWTToken", token);
                Debug.Log("Token set to Player Prefs: " + token);
            }));

            // Wait for authentication to complete or timeout
            while (Time.time - startTime < authenticationTimeout)
            {
                float progressValue = (Time.time - startTime) / authenticationTimeout;
                target = progressValue;

                // Check if the token is received
                string token = PlayerPrefs.GetString("JWTToken");
                if (!string.IsNullOrEmpty(token))
                {
                    operation = SceneManager.LoadSceneAsync(sceneID);
                    break;
                }
                yield return null;
            }

            if (operation == null)
            {
                Debug.LogError("Authentication timed out or token not received. Main menu will not be loaded.");
                LoadingScreen.SetActive(false);
                yield break;
            }
        }
        else
        {
            // For other scenes, proceed with loading
            target = 0;
            slider.value = 0;

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
            LoadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
                target = progressValue;
                yield return null;
            }
        }
    }


    private IEnumerator Authenticate(Action<string> onTokenReceived)
    {
        string url = "http://20.15.114.131:8080/api/login";
        string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

        string json = "{\"apiKey\":\"" + apiKey + "\"}";
        byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

        /* WWWForm form = new WWWForm();
        form.AddField("apiKey", apiKey);
        form.AddField("content-type", "application/json"); */

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(data);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                AuthenticationResponse responseData = JsonUtility.FromJson<AuthenticationResponse>(responseText);
                string jwtToken = responseData.token;
                Debug.Log("Authentication successful. Token: " + jwtToken);
                onTokenReceived(jwtToken); 
            }
            else
            {
                Debug.LogError("Authentication failed. Error: " + request.error);
            }
        }
    }

    private void Update()
    {
        slider.value = Mathf.MoveTowards(slider.value, target, Time.deltaTime * adjustLoadingTime);
    }
}

[Serializable]
public class AuthenticationResponse
{
    public string token;
}
