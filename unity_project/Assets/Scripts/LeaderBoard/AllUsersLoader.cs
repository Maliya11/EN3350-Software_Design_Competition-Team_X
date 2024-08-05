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

    // Variables to store the usernames of the users
    public List<string> usernames;

    public IEnumerator fetchAllUsers()
    {
        // Initialize the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        requestManager.SendRequest(allUsersURL, allUsersMethod, "", this, includeToken);
        yield return StartCoroutine(WaitForAllUsers());
    }

    private IEnumerator WaitForAllUsers()
    {
        while (!requestManager.isRequestCompleted)
        {
            yield return null;
        }

        if (requestManager.isRequestSuccessful)
        {
            Debug.Log("JSON Response within AllUsersLoader: " + requestManager.jsonResponse.ToString());

            // Check if userViews array exists
            JSONNode userViews = requestManager.jsonResponse["userViews"];
            if (userViews == null || !userViews.IsArray)
            {
                Debug.LogError("userViews array is missing or not an array.");
                yield break;
            }

            Debug.Log("Total users found: " + userViews.Count);

            // Clear the list to avoid duplication
            usernames.Clear();

            // Display the information of all the users and save usernames
            for (int i = 0; i < userViews.Count; i++)
            {
                JSONNode user = userViews[i];
                //Debug.Log("User " + (i + 1) + ": " + user.ToString());

                if (user["username"] != null)
                {
                    //Debug.Log("Username: " + user["username"]);
                    usernames.Add(user["username"]);
                }
                else
                {
                    Debug.LogError("Username not found for user " + (i + 1));
                }
            }
            Debug.Log("All users fetched successfully");
        }
        else
        {
            errorNotifications.DisplayErrorMessage(requestManager);
        }
    }
}
