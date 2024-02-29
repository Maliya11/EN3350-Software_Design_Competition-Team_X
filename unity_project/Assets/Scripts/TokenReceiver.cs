using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenReceiver : MonoBehaviour
{
    // This method will be called from JavaScript to receive the token
    public string ReceiveToken(string token)
    {
        Debug.Log("Received token: " + token);
        return token;
    }
}
