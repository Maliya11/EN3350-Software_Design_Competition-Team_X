using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using TMPro;

public class QuestionnaireManager : Singleton<QuestionnaireManager>
{
    // Reference to the RequestManager
    private RequestManager requestManager; 
    private bool includeToken = false; // jwt token is not needed for requesting from the questionnaire database
    // Reference to the ErrorNotifications
    public ErrorNotifications errorNotifications;
    

    // URL related to the questionnaire web application
    // URL directing the user to the questionnaire
    private string questionnaireURL = "http://51.20.76.97:5173/";

    // URL to get the questionnaire status of the player from the database
    private string questionnaireStatusURL = "http://13.60.29.81:8080/player/authenticate";
    private string questionnaireStatusMethod = "POST";

    // URL to get the marks obtained by the player in the questionnaire
    private string questionnaireMarksURL = "http://13.60.29.81:8080/player/details";
    private string questionnaireMarksMethod = "GET";

    // URL to update the bonusGiven status in the database
    private string bonusPerksURL = "http://13.60.29.81:8080/player/bonus";
    private string bonusPerksMethod = "POST";


    private bool userValidity;
    /* User Validity:
     * true  - User is  a valid player in the database
     * false - User is not a valid player in the database
     */

    public int questionnaireStatus;
    /* Questionnaire Status:
     * 0   - User has not completed the questionnaire         => Prompt user to complete the questionnaire
     * 1-9 - User has partially completed the questionnaire   => Prompt user to complete the questionnaire
     * 10  - User has completed the questionnaire             => Proceed to the game
     */

    private int questionnaireMarks;
    /* Questionnaire Marks:
     * Marks are assigned out of 10
     */

    private int bonusGiven;
    /*
     * 0 - Bonus perks have not been given yet
     * 1 - Bonus perks have been given to the user
     */


    // UI Elements
    public GameObject notificationBar;
    public TextMeshProUGUI notificationText;
    public TextMeshProUGUI questionnaireButtonNormalText;
    public TextMeshProUGUI questionnaireButtonPressedText;
    public GameObject questionnaireButtonNormal;
    public GameObject questionnaireButtonPressed;
    public GameObject mainMenuPanel;


    // Method to get the questionnaire status from the database
    public IEnumerator GetQuestionnaireStatus(int promptingOrigin)
    {
        string apiKey = PlayerPrefs.GetString("apiKey");

        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        // Create the body of the request
        var requestBody = new JSONObject();
        requestBody["apiKey"] = apiKey;
        string jsonBody = requestBody.ToString();

        requestManager.SendRequest(questionnaireStatusURL, questionnaireStatusMethod, jsonBody, this, includeToken, null);

        yield return StartCoroutine(WaitForQuestionnaireStatusRequestCompletion(promptingOrigin));

        Debug.Log("Questionnaire status request sent");
    }

    private IEnumerator WaitForQuestionnaireStatusRequestCompletion(int promptingOrigin)
    {
        while (!requestManager.isRequestCompleted)
        {
            yield return null;
        }

        if (requestManager.isRequestSuccessful)
        {
            // If the request is successful, get the user validity and questionnaire status
            Debug.Log("Questionnaire status request successful");
            Debug.Log(requestManager.jsonResponse);
            userValidity = requestManager.jsonResponse["validKey"];
            questionnaireStatus = requestManager.jsonResponse["completedQuestions"];
            Debug.Log("User validity: " + userValidity);
            Debug.Log("Questionnaire status: " + questionnaireStatus);

            // Prompt the user based on the questionnaire status
            yield return PromptUser(promptingOrigin);
        }
        else
        {
            // Display the error message to the user
            errorNotifications.DisplayErrorMessage(requestManager);
            Debug.Log("Questionnaire status request failed");
        }
    }

    // Method to prompt the user based on the questionnaire status
    private IEnumerator PromptUser(int promptingOrigin)
    {
        /* Prompting Origin:
         * 0  - Prompting user from the play button
         * 1  - Prompting user from the questionnaire button
         */

        if (userValidity)
        {
            Debug.Log("User is valid");
            if (questionnaireStatus < 10)
            {
                // As the questionnaire is not completed yet, prompt the user to complete the questionnaire
                Debug.Log("Prompting user to complete the questionnaire...");

                OpenNotificationBar();
                notificationText.text = "Explore the fundamentals of energy efficiency with questions on electricity generation, transmission, and usage.\n All users must complete the questionnaire before proceeding to the game.";
                questionnaireButtonNormalText.text = "Fill the Questionnaire";
                questionnaireButtonPressedText.text = "Fill the Questionnaire";

                // Wait for a response by the user by either going to the questionnaire web application or closing the notification bar
                while (notificationBar.activeSelf)
                {
                    yield return null;
                }
            }
            else
            {
                // As the questionnaire is completed, direct the user to the game or prompt the user to review the questionnaire
                if (promptingOrigin == 1)
                {
                    // Prompt the user to review the questionnaire and get the marks obtained to the Unity environment
                    Debug.Log("User has already completed the questionnaire");
                    OpenNotificationBar();
                    notificationText.text = "Explore the fundamentals of energy efficiency with questions on electricity generation, transmission, and usage.\n You have already completed the questionnaire.";
                    questionnaireButtonNormalText.text = "Review the Questionnaire";
                    questionnaireButtonPressedText.text = "Review the Questionnaire";
                    GetQuestionnaireMarks();
                }
                else
                {
                    // Get the marks obtained to the Unity environment
                    Debug.Log("User has completed the questionnaire");
                    GetQuestionnaireMarks();
                }
            }
        }
        else
        {
            Debug.Log("User is invalid");
        }
    }

    // Mthod to open the notification bar
    public void OpenNotificationBar()
    {
        // Disable the main menu panel and enable the notification bar
        mainMenuPanel.SetActive(false);
        notificationBar.SetActive(true);

        // Before the user responds to the notification, show the normal questionnaire button
        questionnaireButtonNormal.gameObject.SetActive(true);
        questionnaireButtonPressed.gameObject.SetActive(false);
    }

    // Method to direct the user to the questionnaire
    public void DirectToQuestionnaire()
    {
        Debug.Log("Directing user to the questionnaire...");

        // Change the button appearance to show that the user has clicked the button
        questionnaireButtonNormal.gameObject.SetActive(false);
        questionnaireButtonPressed.gameObject.SetActive(true);

        StartCoroutine(WaitAndOpenQuestionnaire());
    }

    private IEnumerator WaitAndOpenQuestionnaire()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(0.5f);

        // Open the URL in the default browser
        Application.OpenURL(questionnaireURL);

        CloseNotificationBar();
    }

    // Method to close the notification bar 
    public void CloseNotificationBar()
    {
        // Disable the notification bar and enable the main menu panel
        notificationBar.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Method to get the marks obtained by the user in the questionnaire
    public void GetQuestionnaireMarks()
    {
        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        requestManager.SendRequest(questionnaireMarksURL, questionnaireMarksMethod, null, this, includeToken, null);

        StartCoroutine(WaitForQuestionnaireMarksRequestCompletion());   
    }

    private IEnumerator WaitForQuestionnaireMarksRequestCompletion()
    {
        while (!requestManager.isRequestCompleted)
        {
            yield return null;
        }

        if (requestManager.isRequestSuccessful)
        {
            // If the request is successful, get the marks obtained by the user in the questionnaire and check if the bonus perks have been given
            Debug.Log("Questionnaire marks request successful");
            questionnaireMarks = requestManager.jsonResponse["marks"];
            bonusGiven = requestManager.jsonResponse["bonusGiven"];
            Debug.Log("Questionnaire marks: " + questionnaireMarks);
            Debug.Log("Bonus given: " + bonusGiven);

            // Assign bonus perks to the user if they have not been given yet
            AssignBonusPerks();
            Debug.Log("Bonus perks: " + PlayerPrefs.GetInt("questionnaireBonus"));
        }
        else
        {
            // Display the error message to the user
            errorNotifications.DisplayErrorMessage(requestManager);
            Debug.Log("Questionnaire marks request failed");
        }
    }

    // Method to assign bonus perks based on the marks obtained by the user in the questionnaire
    private void AssignBonusPerks()
    {
        if (bonusGiven == 0)
        {
            // If the bonus perks have not been given yet, assign the bonus perks to the user
            Debug.Log("Assigning bonus perks to the user...");

            // Set the bonus perks in the PlayerPrefs
            PlayerPrefs.SetInt("revivalKeys", (questionnaireMarks * 5));
            PlayerPrefs.Save();
            Debug.Log("Bonus perks assigned to the user");    

            // Send a request to update the bonusGiven status in the database
            // Create a new instance of the RequestManager
            requestManager = ScriptableObject.CreateInstance<RequestManager>();

            // Create the body of the request
            var requestBody = new JSONObject();
            requestBody["bonusGiven"] = 1;
            string jsonBody = requestBody.ToString();

            requestManager.SendRequest(bonusPerksURL, bonusPerksMethod, jsonBody, this, includeToken, null);

            StartCoroutine(WaitForBonusPerksRequestCompletion());
        }
        else
        {
            Debug.Log("Bonus perks have already been assigned to the user");
        }
    }

    private IEnumerator WaitForBonusPerksRequestCompletion()
    {
        // Wait for the bonus perks request to be completed
        while (!requestManager.isRequestCompleted)
        {
            yield return null;
        }

        if (requestManager.isRequestSuccessful)
        {
            Debug.Log("Bonus given status update request successful");
        }
        else
        {
            // Display the error message to the user
            errorNotifications.DisplayErrorMessage(requestManager);
            Debug.Log("Bonus given status update request failed");
        }
    }
}
