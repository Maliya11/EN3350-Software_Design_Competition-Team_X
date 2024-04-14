using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireManager : MonoBehaviour
{
    // Reference to the RequestManager
    private RequestManager requestManager;

    public void GetQuestionnaireStatus()
    {
        string questionnaireStatusURL = "http://13.50.5.167:8080/player/authenticate";
        string questionnaireStatusMethod = "POST";
        string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ";

        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        var requestBody = new
        {
            apiKey = apiKey
        };
        string jsonBody = JsonUtility.ToJson(requestBody);

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
        }
        else
        {
            Debug.Log("Questionnaire status request failed");
        }
    }
}
