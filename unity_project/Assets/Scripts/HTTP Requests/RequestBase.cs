using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

public abstract class RequestBase : ScriptableObject
{
    // Common properties
    public bool isRequestCompleted { get; protected set; }
    public bool isRequestSuccessful { get; protected set; }
    public JSONNode jsonResponse { get; protected set; }
    public int errorCode { get; protected set; }
    public string errorMessage { get; protected set; }
    public string jwtToken;

    // Initialize common properties
    protected void Initialize()
    {
        isRequestCompleted = false;
        isRequestSuccessful = false;
        jwtToken = PlayerPrefs.GetString("jwtToken");
    }

    // Abstract method for sending requests
    public abstract void SendRequest(string url, string method, string body, MonoBehaviour monoBehaviour, bool includeToken = true, Dictionary<string, string> parameters = null);

    // Coroutine to handle request sending and responses
    protected IEnumerator SendRequestCoroutine(string url, string method, string body, bool includeToken, Dictionary<string, string> parameters)
    {
        // Construct the URL with parameters if provided
        string parametersString = BuildParametersString(parameters);
        url = ConstructUrlWithParameters(url, parametersString);

        // Create request data from the body
        byte[] data = GetDataFromBody(body);

        // Create a new request
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            // Set the request properties
            ConfigureRequest(request, data, includeToken);

            // Send the request asynchronously
            yield return request.SendWebRequest();

            // Handle the request response
            HandleResponse(request);
        }
    }

    // Method to build a string of parameters for the URL
    private string BuildParametersString(Dictionary<string, string> parameters)
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
        return parametersString;
    }

    // Method to construct the URL with parameters
    private string ConstructUrlWithParameters(string url, string parametersString)
    {
        if (!string.IsNullOrEmpty(parametersString))
        {
            url += "?" + parametersString;
        }
        return url;
    }

    // Method to get data from the request body
    private byte[] GetDataFromBody(string body)
    {
        if (!string.IsNullOrEmpty(body))
        {
            return System.Text.Encoding.UTF8.GetBytes(body);
        }
        return null;
    }

    // Method to configure the request
    private void ConfigureRequest(UnityWebRequest request, byte[] data, bool includeToken)
    {
        request.downloadHandler = new DownloadHandlerBuffer();
        if (data != null)
        {
            request.uploadHandler = new UploadHandlerRaw(data);
        }
        request.SetRequestHeader("Content-Type", "application/json");

        // Set JWT token header if available
        if (includeToken && !string.IsNullOrEmpty(jwtToken))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
        }
    }

    // Method to handle the request response
    private void HandleResponse(UnityWebRequest request)
    {
        // Check if the request is successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse and store the JSON response
            string responseText = request.downloadHandler.text;
            jsonResponse = JSON.Parse(responseText);

            // Mark the request as successful
            isRequestSuccessful = true;
            Debug.Log("Request successful: " + jsonResponse.ToString());
        }
        else
        {
            // Handle the error response
            string responseText = request.downloadHandler.text;
            errorCode = (int)request.responseCode;
            errorMessage = request.error;
            jsonResponse = JSON.Parse(responseText);

            // Mark the request as failed
            isRequestSuccessful = false;
            Debug.Log("Request failed: " + jsonResponse.ToString());
        }

        // Mark the request as completed
        isRequestCompleted = true;
    }
}