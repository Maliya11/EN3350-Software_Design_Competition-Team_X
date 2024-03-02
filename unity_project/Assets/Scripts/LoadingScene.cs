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
            yield return StartCoroutine(Authenticate((token) =>
            {
                // Store the token in the PlayerPrefs
                PlayerPrefs.SetString("JWTToken", token);
            }));

            // Check if the token is stored in the PlayerPrefs
            string token = PlayerPrefs.GetString("JWTToken");
            if (token != null && token.Length > 0)
            {
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
            else
            {
                Debug.LogError("Token not found");
                yield break;
            }   
        }
        else
        {
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