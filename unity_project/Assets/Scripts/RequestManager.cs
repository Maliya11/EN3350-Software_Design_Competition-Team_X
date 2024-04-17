using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class RequestManager : ScriptableObject
{   
    // Properties to check the status of the request
    public bool isRequestCompleted { get; private set; }
    public bool isRequestSuccessful { get; private set; }
    private string jwtToken;
    public JSONNode jsonResponse;
    public long errorCode;

    // Method to send a request to the server
    public void SendRequest(string url, string method, string body, MonoBehaviour monoBehaviour, Dictionary<string, string> parameters = null)
    {   
        // Initialize the properties
        isRequestCompleted = false;
        isRequestSuccessful = false;
        jwtToken = PlayerPrefs.GetString("jwtToken");
        if (jwtToken == null)
        {
            Debug.Log("JWT Token is null");
        }

        // If the token is available, send the request
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

        // Create a request with the URL and method
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
                // If the request is successful, get the response and store it in jsonResponse
                string responseText = request.downloadHandler.text;
                jsonResponse = JSON.Parse(responseText);

                // Set the request status to successful
                isRequestSuccessful = true;
                Debug.Log("Request successful: " + jsonResponse.ToString());
            }
            else
            {
                // If the request fails, get the error message, error code and store it in jsonResponse, errorCode
                string errorMessage = request.downloadHandler.text;
                errorCode = request.responseCode;
                jsonResponse = JSON.Parse(errorMessage);

                // Set the request status to failed
                isRequestSuccessful = false;
                Debug.Log("Request failed: " + jsonResponse.ToString());
            }
            
            // Set the request status to completed
            isRequestCompleted = true;
        }
    }   

}