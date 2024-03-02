using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
//using req_manager.js;


[System.Serializable] public class AuthenticationResponse
{
    public string Token;
}

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
        string url = "https://20.15.114.131:8080/api/login";
        string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        WWWForm form = new WWWForm();
        form.AddField("apiKey", apiKey);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                AuthenticationResponse data = JsonUtility.FromJson<AuthenticationResponse>(responseText);
                string jwtToken = data.Token;
                Debug.Log("Authentication successful. Token: " + jwtToken);
                onTokenReceived(jwtToken); // Pass the token to the callback function
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