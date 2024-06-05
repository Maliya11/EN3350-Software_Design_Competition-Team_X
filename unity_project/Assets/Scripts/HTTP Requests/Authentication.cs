using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class AuthenticationManager : RequestBase
{
    /*
    This class is used to handle the authentication process with the server.
    */
    
    // API key for authentication
    private string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

    public override void SendRequest(string url, string method, string body, MonoBehaviour monoBehaviour, bool includeToken = false, Dictionary<string, string> parameters = null)
    {
        throw new NotImplementedException("AuthenticationManager does not support sending generic requests.");
    }

    // Method to handle authentication
    public void Authenticate(MonoBehaviour monoBehaviour)
    {
        // Initialize common properties
        Initialize();

        // Store the API key in the PlayerPrefs
        PlayerPrefs.SetString("apiKey", apiKey);
        
        // Start the authentication coroutine
        monoBehaviour.StartCoroutine(AuthenticateCoroutine(apiKey));
    }

    // Coroutine to handle authentication process
    private IEnumerator AuthenticateCoroutine(string apiKey)
    {
        string url = "http://20.15.114.131:8080/api/login";

        string json = "{\"apiKey\":\"" + apiKey + "\"}";
        byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

        // Create a POST request for authentication
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(data);

            // Send the request asynchronously
            yield return request.SendWebRequest();

            // Handle the response
            HandleAuthenticationResponse(request);
        }
    }

    // Method to handle authentication response
    private void HandleAuthenticationResponse(UnityWebRequest request)
    {
        // Check if the request is successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse and store the JSON response
            string responseText = request.downloadHandler.text;
            AuthenticationResponse responseData = JsonUtility.FromJson<AuthenticationResponse>(responseText);
            string token = responseData.token;

            // Save the JWT token
            PlayerPrefs.SetString("jwtToken", token);
            PlayerPrefs.Save();

            Debug.Log("JWT Token saved: " + token);

            // Mark authentication as successful
            isRequestSuccessful = true;
        }
        else
        {
            // Handle the error response
            string responseText = request.downloadHandler.text;
            errorCode = (int)request.responseCode;
            errorMessage = request.error;
            jsonResponse = JSON.Parse(responseText);

            // Mark authentication as failed
            isRequestSuccessful = false;
        }

        // Mark the request as completed
        isRequestCompleted = true;
    }
}

// Authentication response data class
[Serializable]
public class AuthenticationResponse
{
    public string token;
}

