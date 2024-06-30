using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class AllUserLoader : MonoBehaviour
{
    /*
    This script is used to load the information of all the users from the server.
    */

    // Reference to the RequestManager
    private RequestManager requestManager;
    private bool includeToken = true; // jwt token is needed for requesting from the player database
    // Reference to the ErrorNotifications
    public ErrorNotifications errorNotifications;

    // URL to fetch users' information
    private string allUsersURL = "http://20.15.114.131:8080/api/user/profile/list";
    private string allUsersMethod = "GET";

    public void fetchAllUsers()
    {
        // Initialize the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        requestManager.SendRequest(allUsersURL, allUsersMethod, "", this, includeToken);
        StartCoroutine(WaitForAllUsers());
    }

    private IEnumerator WaitForAllUsers()
    {
        while (!requestManager.isRequestCompleted)
        {
            yield return null;
        }

        if (requestManager.isRequestSuccessful)
        {
            // Parse the JSON response
            JSONNode allUsers = JSON.Parse(requestManager.jsonResponse);

            // Display the information of all the users
            for (int i = 0; i < allUsers.Count; i++)
            {
                Debug.Log("User " + (i + 1) + ":");
                Debug.Log("First Name: " + allUsers[i]["firstName"]);
                Debug.Log("Last Name: " + allUsers[i]["lastName"]);
                Debug.Log("NIC: " + allUsers[i]["nic"]);
                Debug.Log("Username: " + allUsers[i]["username"]);
                Debug.Log("Mobile Number: " + allUsers[i]["mobileNumber"]);
                Debug.Log("Email: " + allUsers[i]["email"]);
                Debug.Log("------------------------------------------------");
            }
        }
        else
        {
            errorNotifications.DisplayErrorMessage(requestManager);
        }
    }
}
