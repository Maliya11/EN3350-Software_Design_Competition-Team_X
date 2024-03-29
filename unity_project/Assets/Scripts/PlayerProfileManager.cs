using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;

public class PlayerProfileManager : MonoBehaviour
{
    private RequestManager requestManager; // Reference to the RequestManager

    // UI elements
    public InputField firstNameInput;
    public InputField lastNameInput;
    public InputField nicInput;
    public InputField usernameInput;
    public InputField mobileNumberInput;
    public InputField emailInput;
    public Toggle demandResponseToggle;
    public Text notificationText;

    // Method to initialize player profile view
    public void InitializeProfile()
    {
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        string url = "http://20.15.114.131:8080/api/user/profile/view";
        string method = "GET";

        requestManager.SendRequest(url, method, null, this, null);
        Debug.Log("Request sent");

        StartCoroutine(WaitForRequestCompletion());
    }

    // Coroutine to wait for the request completion
    private IEnumerator WaitForRequestCompletion()
    {   
        while (!requestManager.IsRequestCompleted)
        {
            yield return null; 
        }
        Debug.Log("Request completed. Status: " + requestManager.IsRequestCompleted);
        OnProfileFetchSuccess(requestManager.jsonResponse);
        Debug.Log("OnProfileFetchSuccess called");
    }

    // Callback method for successful profile fetch
    private void OnProfileFetchSuccess(JSONNode jsonResponse)
    {   
        Debug.Log("JSON Response inside OnProfileFetchSuccess: " + jsonResponse);
        PlayerProfileData profileData = JsonUtility.FromJson<PlayerProfileData>(jsonResponse);

        // Accessing user properties
        string firstName = profileData.user.firstname;
        string lastName = profileData.user.lastname;
        string nic = profileData.user.nic;
        string username = profileData.user.username;
        string mobileNumber = profileData.user.phoneNumber;
        string email = profileData.user.email;

        Debug.Log("First Name: " + firstName);
        Debug.Log("Last Name: " + lastName);
        Debug.Log("NIC: " + nic);
        Debug.Log("Username: " + username);
        Debug.Log("Mobile Number: " + mobileNumber);
        Debug.Log("Email: " + email);
        
        //CheckAndPromptMissingFields();
    }

    // Method to check for missing fields and prompt the player to complete them
    private void CheckAndPromptMissingFields()
    {
        if (string.IsNullOrEmpty(firstNameInput.text) || string.IsNullOrEmpty(nicInput.text) || string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(mobileNumberInput.text))
        {
            notificationText.text = "Please complete missing information.";
        }
        else
        {
            notificationText.text = "";
        }
    }

    // Method to update player profile with missing information
    public void UpdateProfile()
    {
        // Implement update profile logic here (send updated data to the server)
        // For brevity, this part is left as an exercise for you

        notificationText.text = "Profile updated successfully!";
    }
}

[System.Serializable]
public class PlayerProfileData
{
    public UserData user;
}

[System.Serializable]
public class UserData
{
    public string firstname;
    public string lastname;
    public string username;
    public string nic;
    public string phoneNumber;
    public string email;
    public string profilePictureUrl;
}