using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Reference to the PlayerProfileManager
    public PlayerProfileManager playerProfile;
    // Reference to the QuestionnaireManager
    public QuestionnaireManager questionnaireManager;

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
        yield return playerProfile.CheckAndHandleMissingFields();
        yield return questionnaireManager.GetQuestionnaireStatus(0); // Argument 0 indicates that the request is from the Play Game button
        // Direct to the game scene
        if (questionnaireManager.questionnaireStatus == 10 && playerProfile.isProfileCompleted)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Player Profile

    public void PlayerProfile()
    {
        Debug.Log("Player profile");
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
        // Remove the JWT token from the PlayerPrefs
        PlayerPrefs.DeleteKey("jwtToken");
        Debug.Log("JWT Token removed from PlayerPrefs");
        SceneManager.LoadScene("LoginScene");
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
