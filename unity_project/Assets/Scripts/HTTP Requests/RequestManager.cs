using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class RequestManager : RequestBase
{
    /*
    This class is used to manage the HTTP requests to the server.
    */
    
    // Override the abstract method to implement the specific request logic
    public override void SendRequest(string url, string method, string body, MonoBehaviour monoBehaviour, bool includeToken = true, Dictionary<string, string> parameters = null)
    {
        // Initialize the Common Properties
        Initialize(); 

        // Start the Coroutine for Sending the Request
        monoBehaviour.StartCoroutine(SendRequestCoroutine(url, method, body, includeToken, parameters));
    }
}
