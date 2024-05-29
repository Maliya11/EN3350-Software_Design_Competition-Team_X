using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// Fetch questions from the database for the treasure
public class TreasureQuestions : Singleton<TreasureQuestions>
{
    // Reference to the RequestManager
    private RequestManager requestManager;
    private bool includeToken = false; // jwt token is not needed for requesting from the questions database
    // Reference to the ErrorNotifications
    public ErrorNotifications errorNotifications;

    // URL related to the questions
    private string questionsFetchURL = "http://13.60.29.81:8080/question/allQuestions";
    private string questionsFetchMethod = "GET";

    // Flags to check if the questions are fetched
    public bool isQuestionsFetched { get; private set; }

    void Start()
    {
        isQuestionsFetched = false;
        // Fetch all the questions
        StartCoroutine(FetchQuestionsWithRetry());
    }

    // Coroutine to fetch all the questions with retry mechanism
    private IEnumerator FetchQuestionsWithRetry()
    {
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        // Keep trying to fetch questions until successful
        while (!isQuestionsFetched)
        {
            yield return FetchQuestions();

            if (!isQuestionsFetched)
            {
                // Log the retry attempt
                Debug.LogWarning("Fetching questions failed. Retrying...");

                // Wait for some time before retrying 
                yield return new WaitForSeconds(5);
            }
        }
    }

    // Coroutine to fetch all the questions
    private IEnumerator FetchQuestions()
    {
        /// Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        // Send a GET request to fetch all the questions
        requestManager.SendRequest(questionsFetchURL, questionsFetchMethod, "", this, includeToken);

        // Wait until the request is completed
        yield return new WaitUntil(() => requestManager.isRequestCompleted);

        // Check if the request is successful
        if (requestManager.isRequestSuccessful)
        {
            // Parse and store the JSON response
            Debug.Log("Questions fetched");
            string jsonResponse = requestManager.jsonResponse.ToString();
            Debug.Log("JSON response: " + jsonResponse);

            // Parse the JSON response
            List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(jsonResponse);

            // Store the questions as a list in PlayerPrefs
            PlayerPrefs.SetString("treasureQuestions", jsonResponse);
            PlayerPrefs.Save();

            // Display the questions
            foreach (Question question in questions)
            {
                Debug.Log(question.id + ", " + question.q +  question.corAns);
            }

            // Mark the questions as fetched
            isQuestionsFetched = true;
        }
        else
        {
            // Display an error notification
            errorNotifications.DisplayErrorMessage(requestManager);

            // Mark the questions as not fetched
            isQuestionsFetched = false;
        }
    }
}

// Define a class to match the structure of each question in the JSON response
[System.Serializable]
public class Question
{
    public int id;
    public string q;
    public string ans1;
    public string ans2;
    public string ans3;
    public string ans4;
    public int corAns;
    public string genFeed;
    public string feed1;
    public string feed2;
    public string feed3;
    public string feed4;
}

// Define a class to contain the list of questions
[System.Serializable]
public class QuestionResponse
{
    public List<Question> questions;
}
