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

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider slider;
    private float adjustLoadingTime = 1000000000.0f;

    public void LoadScene(int sceneID) 
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    public IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / adjustLoadingTime);
            slider.value = progressValue;
            yield return null; // Allow other tasks to execute
        }

    }

    private IEnumerator Authenticate()
    {
        string url = "http://20.15.114.131:8080/api/login";
        string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

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
            }
            else
            {
                Debug.LogError("Authentication failed. Error: " + request.error);
            }
        }
    }

    [Serializable]
    private class AuthenticationResponse
    {
        public string Token;
    }
}
