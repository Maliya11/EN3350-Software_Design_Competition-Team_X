using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class RequestManager : ScriptableObject
{    
    public bool IsRequestCompleted { get; private set; }
    public bool IsRequestSuccessful { get; private set; }
    private string jwtToken;
    public JSONNode jsonResponse;

    // Method to send a request to the server
    public void SendRequest(string url, string method, string body, MonoBehaviour monoBehaviour, Dictionary<string, string> parameters = null)
    {   
        IsRequestCompleted = false;
        IsRequestSuccessful = false;
        jwtToken = PlayerPrefs.GetString("jwtToken");
        if (jwtToken == null)
        {
            Debug.Log("JWT Token is null");
        }
        monoBehaviour.StartCoroutine(SendRequestCoroutine(url, method, body, parameters));
    }

    // Coroutine to handle the request
    private IEnumerator SendRequestCoroutine(string url, string method, string body, Dictionary<string, string> parameters)
    {
        string parametersString = "";
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                parametersString += "&" + param.Key + "=" + param.Value;
            }
            parametersString = parametersString.TrimStart('&');
        }

        // Construct URL with parameters if they exist
        if (!string.IsNullOrEmpty(parametersString))
        {
            url += "?" + parametersString;
        }

        byte[] data = null;
        if (!string.IsNullOrEmpty(body))
        {
            data = System.Text.Encoding.UTF8.GetBytes(body);
        }

        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            
            if (data != null)
            {
                request.uploadHandler = new UploadHandlerRaw(data);
            }

            request.SetRequestHeader("Content-Type", "application/json");

            // Set JWT token if available
            if (!string.IsNullOrEmpty(jwtToken))
            {   
                request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            }

            // Send the request asynchronously
            yield return request.SendWebRequest();

            // Check the result of the request
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                IsRequestSuccessful = true;
                jsonResponse = JSON.Parse(responseText);
                Debug.Log("Request successful: " + jsonResponse.ToString());
            }
            else
            {
                string errorMessage = request.downloadHandler.text;
                IsRequestSuccessful = false;
                jsonResponse = JSON.Parse(errorMessage);
                Debug.LogError("Request failed: " + jsonResponse.ToString());
            }
            
            IsRequestCompleted = true;
        }
    }   

}