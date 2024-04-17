using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

// Represents an authentication manager responsible for handling authentication requests
public class AuthenticationManager : ScriptableObject
{
    // API key to authenticate with the server
    private string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";


    // Property to check if the authentication process is completed
    public bool IsAuthenticated { get; private set; }


    // Initialize the properties
    private void OnEnable()
    {
        // Set the API key in the PlayerPrefs
        PlayerPrefs.SetString("apiKey", apiKey);
        PlayerPrefs.Save();

        IsAuthenticated = false;
    }

    // Method to authenticate with the server using the provided API key
    public void Authenticate(MonoBehaviour monoBehaviour)
    {
        // Start the coroutine
        monoBehaviour.StartCoroutine(AuthenticateCoroutine(apiKey));
    }

    // Coroutine to handle the authentication process
    private IEnumerator AuthenticateCoroutine(string apiKey)
    {
        string url = "http://20.15.114.131:8080/api/login";

        string json = "{\"apiKey\":\"" + apiKey + "\"}";
        byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

        // Create a POST request with the API key
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(data);

            // Send the request asynchronously
            yield return request.SendWebRequest();

            // Check the result of the request
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Get the token from the response
                string responseText = request.downloadHandler.text;
                AuthenticationResponse responseData = JsonUtility.FromJson<AuthenticationResponse>(responseText);
                string token = responseData.token;

                // Set the token in the PlayerPrefs
                PlayerPrefs.SetString("jwtToken", token);
                PlayerPrefs.Save();

                string jwtToken = PlayerPrefs.GetString("jwtToken");
                Debug.Log("JWT Token from PlayerPrefs: " + jwtToken);
                IsAuthenticated = true;
            }
            else
            {
                string errorMessage = "Authentication failed. Error: " + request.error;
                Debug.LogError(errorMessage);
            }
        }
    }
}

// Represents the authentication response from the server
[Serializable]
public class AuthenticationResponse
{
    public string token;
}