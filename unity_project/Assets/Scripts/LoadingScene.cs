using System.Collections;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
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

    public async Task LoadScene(int sceneID) 
    {
        await LoadSceneAsync(sceneID);
    }

    public async Task LoadSceneAsync(int sceneID)
    {
        if (sceneID == 1)
        {
            bool tokenReceived = await CallAuthenticate();

            if (tokenReceived)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
                LoadingScreen.SetActive(true);

                while (!operation.isDone)
                {
                    float progressValue = Mathf.Clamp01(operation.progress / adjustLoadingTime);
                    slider.value = progressValue;
                    await Task.Yield(); // Allow other tasks to execute
                }
            }
            else
            {
                Debug.LogError("Authentication failed.");
            }
        }
        else
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
            LoadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01(operation.progress / adjustLoadingTime);
                slider.value = progressValue;
                await Task.Yield(); // Allow other tasks to execute
            }
        }

    }

    public async Task<bool> CallAuthenticate()
    {
        string api_key = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

        try
        {
            using (var client = new HttpClient())
            {
                var jsonData = JsonConvert.SerializeObject(new { apiKey = this.apiKey });
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://20.15.114.131:8080/api/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<AuthenticationResponse>(responseContent);
                    this.jwtToken = data.Token;
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication failed: {ex.Message}");
            return false;
        }
    
    }

    private class AuthenticationResponse
    {
        public string Token { get; set; }
    }
}
