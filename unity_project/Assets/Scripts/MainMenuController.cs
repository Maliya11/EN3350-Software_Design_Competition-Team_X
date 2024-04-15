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
        StartCoroutine(CheckAndHandleMissingFields());
        questionnaireManager.GetQuestionnaireStatus();
    }

    private IEnumerator CheckAndHandleMissingFields()
    {
        // Initialize the player profile
        playerProfile.InitializeProfile();

        while (!playerProfile.isProfileInitialized)
        {
            yield return null;
        }

        bool isMissingFields = playerProfile.CheckAndPromptMissingFields();

        if (isMissingFields)
        {
            Debug.Log("Player profile is missing fields. Please fill in all the required fields before playing.");

        }
        else
        {
            Debug.Log("All player profile fields are complete. Proceeding to play the game...");
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
    }
}
