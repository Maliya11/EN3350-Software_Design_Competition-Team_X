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

    public bool isProfileInitialized = false;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Profile Initialization and View

    /// Method to view player profile panel
    public void ViewProfile()
    {
        StartCoroutine(ViewProfileCoroutine());
    }

    private IEnumerator ViewProfileCoroutine()
    {
        InitializeProfile();

        while (!isProfileInitialized)
        {
            yield return null;
        }

        // Only show the profile panel if the request was successful and profile is initialized
        if (isProfileInitialized)
        {
            profilePanel.SetActive(true);
            mainMenuPanel.SetActive(false);
        }

        yield return null;
    }

    // Method to initialize player profile fetch
    public void InitializeProfile()
    {
        string profileFetchURL = "http://20.15.114.131:8080/api/user/profile/view";
        string profileFetchMethod = "GET";

        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        requestManager.SendRequest(profileFetchURL, profileFetchMethod, null, this, null);
        StartCoroutine(WaitForProfileFetchRequestCompletion());

        Debug.Log("Profile view request completed");
    }

    // Coroutine to wait for the profile fetch request completion
    private IEnumerator WaitForProfileFetchRequestCompletion()
    {   
        while (!requestManager.isRequestCompleted)
        {
            yield return null; 
        }

        if (requestManager.isRequestSuccessful)
        {
            OnProfileFetchSuccess(requestManager.jsonResponse);
            Debug.Log("Profile fetch successful");
            isProfileInitialized = true;
        }
        else
        {
            Debug.Log("Profile fetch failed");
            Debug.Log("Error code: " + requestManager.errorCode);
        }
    }

    // Callback method for successful profile fetch
    private void OnProfileFetchSuccess(JSONNode jsonResponse)
    {
        if (jsonResponse == null)
        {
            Debug.Log("JSON Response is null");
            return;
        }

        PlayerProfileData profileData;
        
        profileData = JsonUtility.FromJson<PlayerProfileData>(jsonResponse.ToString());
        
        if (profileData == null || profileData.user == null)
        {
            Debug.Log("ProfileData or user data is null");
            return;
        }

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

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Profile Validation

    // Method to check for missing fields
    public IEnumerator CheckAndHandleMissingFields()
    {
        // Initialize the player profile
        InitializeProfile();

        while (!isProfileInitialized)
        {
            yield return null;
        }

        bool isMissingFields = string.IsNullOrEmpty(firstNameInput.text) || string.IsNullOrEmpty(lastNameInput.text) || string.IsNullOrEmpty(nicInput.text) || string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(mobileNumberInput.text);
        
        if (isMissingFields)
        {
            Debug.Log("Player profile is missing fields. Please fill in all the required fields before playing.");
            notificationBar.SetActive(true);
            string errorMessage = "Player profile is missing information.\n Please fill in all the required fields before playing.";
            notificationText.text = errorMessage;  
            while (notificationBar.activeSelf)
            {
                yield return null;
            }

            ViewProfile();

            while (profilePanel.activeSelf)
            {
                yield return null;
            }
        }
        else
        {
            Debug.Log("All player profile fields are complete. Proceeding to play the game...");
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Profile Update 

    // Method to update player profile with missing information
    public void UpdateProfile()
    {   
        string profileUpdateURL = "http://20.15.114.131:8080/api/user/profile/update";
        string profileUpdateMethod = "PUT";

        UserData updatedUserData = new UserData
        {
            firstname = firstNameInput.text,
            lastname = lastNameInput.text,
            nic = nicInput.text,
            username = usernameInput.text,
            phoneNumber = mobileNumberInput.text,
            email = emailInput.text
        };

        string body = JsonUtility.ToJson(updatedUserData);

        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>(); 

        requestManager.SendRequest(profileUpdateURL, profileUpdateMethod, body, this, null);
        Debug.Log("Profile update request sent");

        StartCoroutine(WaitForProfileUpdateRequestCompletion());

        Debug.Log("Profile update request completed");
    }

    // Coroutine to wait for the profile view request completion
    private IEnumerator WaitForProfileUpdateRequestCompletion()
    {   
        while (!requestManager.isRequestCompleted)
        {
            yield return null; 
        }

        if (requestManager.isRequestSuccessful)
        {   
            Debug.Log("Profile update successful");
            notificationText.text = "";
            profilePanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {  
            // Error message will be displayed for missing fields and incorrect format
            Debug.Log("Profile update failed");
            string errorMessage = requestManager.jsonResponse["message"];
            notificationBar.SetActive(true);
            notificationText.text = errorMessage;        
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Method to close the notification bar and direct into profile view
    public void CloseNotificationBar()
    {
        notificationBar.SetActive(false);
    }

}

// Data classes for player profile
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