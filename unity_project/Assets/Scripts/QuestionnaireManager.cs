using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using TMPro;

public class QuestionnaireManager : MonoBehaviour
{
    // Reference to the RequestManager
    private RequestManager requestManager;
    
    private bool userValidity;
    /* User Validity:
     * true  - User is  a valid player in the database
     * false - User is not a valid player in the database
     */

    private int questionnaireStatus;
    /* Questionnaire Status:
     * 0   - User has not completed the questionnaire         => Prompt user to complete the questionnaire
     * 1-9 - User has partially completed the questionnaire   => Prompt user to complete the questionnaire
     * 10  - User has completed the questionnaire             => Proceed to the game
     */

    // UI elements
    public GameObject notificationBar;
    public TextMeshProUGUI notificationText;
    public GameObject questionnaireButtonNormal;
    public GameObject questionnaireButtonPressed;

    // Method to get the questionnaire status from the database
    public void GetQuestionnaireStatus()
    {
        string questionnaireStatusURL = "http://16.170.233.8:8080/player/authenticate";
        string questionnaireStatusMethod = "POST";
        string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        // Create the body of the request
        var requestBody = new JSONObject();
        requestBody["apiKey"] = apiKey;
        string jsonBody = requestBody.ToString();

        requestManager.SendRequest(questionnaireStatusURL, questionnaireStatusMethod, jsonBody, this, null);

        StartCoroutine(WaitForQuestionnaireStatusRequestCompletion());

        Debug.Log("Questionnaire status request sent");
    }

    private IEnumerator WaitForQuestionnaireStatusRequestCompletion()
    {
        while (!requestManager.isRequestCompleted)
        {
            yield return null;
        }

        if (requestManager.isRequestSuccessful)
        {
            Debug.Log("Questionnaire status request successful");
            Debug.Log(requestManager.jsonResponse);
            userValidity = requestManager.jsonResponse["validKey"];
            questionnaireStatus = requestManager.jsonResponse["completedQuestions"];
            PromptUser();
        }
        else
        {
            Debug.Log("Questionnaire status request failed");
        }
    }

    // Method to prompt the user based on the questionnaire status
    public void PromptUser()
    {
        if (userValidity)
        {
            Debug.Log("User is valid");
            if (questionnaireStatus < 10)
            {
                Debug.Log("Prompting user to complete the questionnaire...");
                notificationBar.SetActive(true);
                notificationText.text = "Explore the fundamentals of energy efficiency with questions on electricity generation, transmission, and usage.\n All users must complete the questionnaire before proceeding to the game.";
            }
            else
            {
                Debug.Log("User has completed the questionnaire");
            }
        }
        else
        {
            Debug.Log("User is invalid");
        }
    }

    // Method to direct the user to the questionnaire
    public void DirectToQuestionnaire()
    {
        Debug.Log("Directing user to the questionnaire...");
        questionnaireButtonNormal.gameObject.SetActive(false);
        questionnaireButtonPressed.gameObject.SetActive(true);
        Application.OpenURL("https://www.google.com/");
    }

    // Method to close the notification bar 
    public void CloseNotificationBar()
    {
        notificationBar.SetActive(false);
        questionnaireButtonNormal.gameObject.SetActive(true);
        questionnaireButtonPressed.gameObject.SetActive(false);
    }
}
