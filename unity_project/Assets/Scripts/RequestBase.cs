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
    public abstract void SendRequest(string url, string method, string body, MonoBehaviour monoBehaviour, Dictionary<string, string> parameters = null);

    // Coroutine to handle request sending and responses
    protected IEnumerator SendRequestCoroutine(string url, string method, string body, Dictionary<string, string> parameters)
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
            ConfigureRequest(request, data);

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
    private void ConfigureRequest(UnityWebRequest request, byte[] data)
    {
        request.downloadHandler = new DownloadHandlerBuffer();
        if (data != null)
        {
            request.uploadHandler = new UploadHandlerRaw(data);
        }
        request.SetRequestHeader("Content-Type", "application/json");

        // Set JWT token header if available
        if (!string.IsNullOrEmpty(jwtToken))
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

public class RequestManager : RequestBase
{
    // Override the abstract method to implement the specific request logic
    public override void SendRequest(string url, string method, string body, MonoBehaviour monoBehaviour, Dictionary<string, string> parameters = null)
    {
        Initialize(); // Initialize common properties
        monoBehaviour.StartCoroutine(SendRequestCoroutine(url, method, body, parameters));
    }
}

public class AuthenticationManager : RequestBase
{
    // API key for authentication
    private string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

    // Method to handle authentication
    public void Authenticate(MonoBehaviour monoBehaviour)
    {
        // Initialize common properties
        Initialize();
        
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
            isAuthenticated = true;
        }
        else
        {
            // Handle error response
            errorCode = (int)request.responseCode;
            errorMessage = request.error;
            Debug.Log("Error: " + errorCode + " - " + errorMessage);

            // Mark authentication as failed
            isAuthenticated = false;
        }

        // Mark the request as completed
        isCompleted = true;
    }
}

// Authentication response data class
[Serializable]
public class AuthenticationResponse
{
    public string token;
}
