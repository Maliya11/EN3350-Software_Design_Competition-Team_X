using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;
using TMPro;

public class PlayerProfileManager : MonoBehaviour
{
    // Reference to the RequestManager
    private RequestManager requestManager; 

    // UI elements
    public InputField firstNameInput;
    public InputField lastNameInput;
    public InputField nicInput;
    public InputField usernameInput;
    public InputField mobileNumberInput;
    public InputField emailInput;
    public GameObject notificationBar;
    public TextMeshProUGUI notificationText;
    public GameObject profilePanel;
    public GameObject mainMenuPanel;

    private void Start()
    {
        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();
    }

    // Method to initialize player profile view
    public void InitializeProfile()
    {
        string profileViewURL = "http://20.15.114.131:8080/api/user/profile/view";
        string profileViewMethod = "GET";

        requestManager.SendRequest(profileViewURL, profileViewMethod, null, this, null);
        StartCoroutine(WaitForProfileViewRequestCompletion());

        Debug.Log("Profile view request completed");
    }

    // Coroutine to wait for the profile view request completion
    private IEnumerator WaitForProfileViewRequestCompletion()
    {   
        while (!requestManager.IsRequestCompleted)
        {
            yield return null; 
        }

        if (requestManager.IsRequestSuccessful)
        {
            OnProfileFetchSuccess(requestManager.jsonResponse);
            Debug.Log("Profile fetch successful");
        }
        else
        {
            Debug.LogError("Profile fetch failed");
        }
    }

    // Callback method for successful profile fetch
    private void OnProfileFetchSuccess(JSONNode jsonResponse)
    {
        if (jsonResponse == null)
        {
            Debug.LogError("JSON Response is null");
            return;
        }

        PlayerProfileData profileData;
        try
        {
            profileData = JsonUtility.FromJson<PlayerProfileData>(jsonResponse.ToString());
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to parse JSON response to PlayerProfileData: " + ex.Message);
            return;
        }

        if (profileData == null || profileData.user == null)
        {
            Debug.LogError("ProfileData or user data is null");
            return;
        }

        // Accessing user properties
        string firstName = profileData.user.firstname;
        string lastName = profileData.user.lastname;
        string nic = profileData.user.nic;
        string username = profileData.user.username;
        string mobileNumber = profileData.user.phoneNumber;
        string email = profileData.user.email;

        // Check for null values before assigning to UI elements
        firstNameInput.text = firstName ?? "";
        lastNameInput.text = lastName ?? "";
        nicInput.text = nic ?? "";
        usernameInput.text = username ?? "";
        mobileNumberInput.text = mobileNumber ?? "";
        emailInput.text = email ?? "";

        Debug.Log("Profile data: " + firstName + " " + lastName + " " + nic + " " + username + " " + mobileNumber + " " + email);
    }

    // Method to check for missing fields
    public bool IsMissingFields()
    {
        // Initialize Profile View
        InitializeProfile();

        if (string.IsNullOrEmpty(firstNameInput.text) || string.IsNullOrEmpty(lastNameInput.text) || string.IsNullOrEmpty(nicInput.text) || string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(mobileNumberInput.text))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Method to update player profile with missing information
    public void UpdateProfile()
    {   
        if (IsMissingFields())
        {
            notificationBar.SetActive(true);
            notificationText.text = "Please fill in all the required fields";
            Debug.Log("Missing fields");
            return;
        }

        string profileUpdateURL = "http://20.15.114.131:8080/api/user/profile/update";
        string profileUpdateMethod = "PUT";

        UserData updtaedUserData = new UserData
        {
            firstname = firstNameInput.text,
            lastname = lastNameInput.text,
            nic = nicInput.text,
            username = usernameInput.text,
            phoneNumber = mobileNumberInput.text,
            email = emailInput.text
        };

        string body = JsonUtility.ToJson(updtaedUserData);

        // Send the request to update the profile
        requestManager.SendRequest(profileUpdateURL, profileUpdateMethod, body, this, null);
        Debug.Log("Profile update request sent");

        StartCoroutine(WaitForProfileUpdateRequestCompletion());

        Debug.Log("Profile update request completed");
    }

    // Coroutine to wait for the profile view request completion
    private IEnumerator WaitForProfileUpdateRequestCompletion()
    {   
        while (!requestManager.IsRequestCompleted)
        {
            yield return null; 
        }

        if (requestManager.IsRequestSuccessful)
        {   
            Debug.Log("Profile update successful");
            notificationText.text = "";
            profilePanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {  
            Debug.Log("Profile update failed");
            string errorMessage = requestManager.jsonResponse["message"];
            notificationBar.SetActive(true);
            notificationText.text = errorMessage;        
        }
    }

    // Method to close the notification bar and direct into profile view
    public void CloseNotificationBar()
    {
        notificationBar.SetActive(false);
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