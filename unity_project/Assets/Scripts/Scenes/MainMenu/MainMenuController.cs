using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    /*
    This script is used to manage the features of the Main Menu.
    */

    // Reference to the PlayerProfileManager
    private PlayerProfileManager playerProfile;
    // Reference to the QuestionnaireManager
    private QuestionnaireManager questionnaireManager;
    // Reference to the LoadingScene
    private LoadingScene loadingScene;

    private void Start()
    {
        playerProfile = FindObjectOfType<PlayerProfileManager>();
        questionnaireManager = FindObjectOfType<QuestionnaireManager>();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Play Game

    public void PlayGame()
    {
        Debug.Log("Play Game");
        StartCoroutine(PlayGameCoroutine());
    }

    private IEnumerator PlayGameCoroutine()
    {
        // Check for missing fields in the player profile
        yield return StartCoroutine(playerProfile.CheckAndHandleMissingFields());

        // Check if the player has completed the questionnaire
        // Argument 0 indicates that the request is from the Play Game button
        //yield return StartCoroutine(questionnaireManager.GetQuestionnaireStatus(0)); 

        // Once the Player profile is completed and the Questionnaire is filled => Load the Game Scene
        /* if (questionnaireManager.questionnaireStatus == 10 && playerProfile.isProfileCompleted)
        {
            loadingScene = FindObjectOfType<LoadingScene>();
            loadingScene.LoadScene(2);
        } */
        if (playerProfile.isProfileCompleted)
        {
            loadingScene = FindObjectOfType<LoadingScene>();
            loadingScene.LoadScene("SelectionScene");
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Display Leaderboard

    public void Leaderboard()
    {
        Debug.Log("Leaderboard");
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Exit the game

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        // Remove the apiKey and jwtToken from PlayerPrefs 
        PlayerPrefs.DeleteKey("apiKey");
        PlayerPrefs.DeleteKey("jwtToken");
        PlayerPrefs.DeleteKey("treasureQuestions");
        Debug.Log("Saved data removed from PlayerPrefs");

        // Load the Initial login scene
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene("LoginScene");
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Directing to the Questionnaire
    public void DirectToQuestionnaire()
    {
        StartCoroutine(DirectToQuestionnaireCoroutine()); 
    }

    private IEnumerator DirectToQuestionnaireCoroutine()
    {
        yield return questionnaireManager.GetQuestionnaireStatus(1); // Argument 1 indicates that the request is from the Questionnaire button
    }
}
