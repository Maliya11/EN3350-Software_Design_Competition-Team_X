using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
//using req_manager.js;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider slider;
    private float adjustLoadingTime = 1000000000.0f;

    private readonly string authentication_url = "http://20.15.114.131:8080/api/login";
    private readonly string api_key = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

    public void Awake() 
    {
        Application.targetFrameRate = 60;
    }

    public void LoadScene(int sceneID) 
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID)
    {
        if (sceneID == 1) 
        {
            // Call the JavaScript function defined in req_manager.js
            UnityEngine.Coroutine token = StartCoroutine(CallAuthenticate());

            if (token != null)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
                LoadingScreen.SetActive(true);

                while (!operation.isDone)
                {
                    float progressValue = Mathf.Clamp01(operation.progress / adjustLoadingTime);
                    slider.value = progressValue;
                }
            }
            else
            {
                Debug.LogError("Authentication failed.");
            }
            
            yield return null;
        }


        else
        {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / adjustLoadingTime);
            slider.value = progressValue;
            yield return null;
        }
        }
    }

    IEnumerator CallAuthenticate()
    {
        string jsonData = "{\"apiKey\": \"" + this.api_key + "\"}";

        UnityWebRequest www = new UnityWebRequest(this.authentication_url, "POST");

        www.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
        }

        yield return  www.SendWebRequest();
    }
}
