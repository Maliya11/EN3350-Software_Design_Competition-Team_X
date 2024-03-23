using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PostMethod : MonoBehaviour
{
    private string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";
    private string endpoint = "http://20.15.114.131:8080/api/login";

    private string jwt;

    private IEnumerator PostRequest()
    {
        // Create a JSON object with the api key
        string json = "{\"apiKey\":\"" + apiKey + "\"}";

        // Convert the JSON object to bytes
        byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

        // Create a custom certificate handler that can validate the server certificate
        MyCertificateHandler certificateHandler = new MyCertificateHandler();

        // Create a post request with the data and the certificate handler
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(endpoint, ""))
        {
            // Assign the certificate handler to the request
            request.certificateHandler = certificateHandler;

            // Set the content type header to application/json
            request.SetRequestHeader("Content-Type", "application/json");

            // Set the upload handler to send the data
            request.uploadHandler = new UploadHandlerRaw(data);

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check the status code of the response
            if (request.responseCode == 200) // OK
            {
                // Get the response text
                string response = request.downloadHandler.text;
                // Store the response as JWT
                jwt = response;
                // Log the JWT
                Debug.Log("You are successfully authenticated. Your JWT token is:" + jwt);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

            }
            else if (request.responseCode == 400) // Bad Request
            {
                // Log the error message
                Debug.LogError("Bad Request: " + request.downloadHandler.text);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
            }
            else if (request.responseCode == 401) // Unauthorized
            {
                // Log the error message
                Debug.LogError("Unauthorized: " + request.downloadHandler.text);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);

            }
            else // Other errors
            {
                // Log the error message
                Debug.LogError("Request failed: " + request.error);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);
            }
        }
    }

    public void OnButtonClick()
    {
        StartCoroutine(PostRequest());
    }

}

// A custom certificate handler that can validate the server certificate
public class MyCertificateHandler : CertificateHandler
{
    // Override this function to implement custom certificate validation logic
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        // TODO: Add your own certificate validation logic here
        // For example, you can compare the certificateData with a known certificate
        return true; // Return true if the certificate is valid, false otherwise
    }
}