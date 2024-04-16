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
        questionnaireManager.GetQuestionnaireStatus(0); // Argument 0 indicates that the request is from the Play Game button
        // Direct to the game scene
        SceneManager.LoadScene("GameScene");
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
        questionnaireManager.GetQuestionnaireStatus(1); // Argument 1 indicates that the request is from the Questionnaire button
        Debug.Log("Questionnaire"); 
    }
}
