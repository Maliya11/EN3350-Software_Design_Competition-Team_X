/* public class RequestManager : ScriptableObject
{   
    // Properties to check the status of the request
    public bool isRequestCompleted { get; private set; }
    public bool isRequestSuccessful { get; private set; }
    private string jwtToken;
    public JSONNode jsonResponse { get; private set; }
    public int errorCode { get; private set; }
    public string errorMessage { get; private set; }

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
                string responseText = request.downloadHandler.text;
                errorCode = (int)request.responseCode;
                errorMessage = request.error;
                jsonResponse = JSON.Parse(responseText);

                // Set the request status to failed
                isRequestSuccessful = false;
                Debug.Log("Request failed: " + jsonResponse.ToString());
            }
            
            // Set the request status to completed
            isRequestCompleted = true;
        }
    }   

} */

/* // Represents an authentication manager responsible for handling authentication requests
public class AuthenticationManager : ScriptableObject
{
    // API key to authenticate with the server
    private string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";


    // Properties to check the status of the authentication
    public bool isCompleted { get; private set; }
    public bool isAuthenticated { get; private set; }
    public int errorCode { get; private set; }
    public string errorMessage { get; private set; }


    // Initialize the properties
    private void OnEnable()
    {
        // Set the API key in the PlayerPrefs
        PlayerPrefs.SetString("apiKey", apiKey);
        PlayerPrefs.Save();

        isCompleted = false;
        isAuthenticated = false;
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

                // Set the authentication to successful
                isAuthenticated = true;
            }
            else
            {
                // Get the error code and message
                errorCode = (int)request.responseCode;
                errorMessage = request.error;
                Debug.Log("Error: " + errorCode + " - " + errorMessage);

                // Set the authentication to failed
                isAuthenticated = false;
            }

            // Set the request to be completed
            isCompleted = true;
        }
    }
}

// Represents the authentication response from the server
[Serializable]
public class AuthenticationResponse
{
    public string token;
} */