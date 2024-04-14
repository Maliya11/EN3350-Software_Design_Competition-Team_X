using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Reference to the PlayerProfileManager
    public PlayerProfileManager playerProfile;

    private void Start()
    {
        // Create a new instance of the PlayerProfileManager
        playerProfile = FindObjectOfType<PlayerProfileManager>();
    }

    public void PlayGame()
    {
        if (playerProfile == null)
        {
            Debug.LogError("PlayerProfileManager reference is missing.");
            return;
        }
        
        // Check if there are any missing fields in the player profile
        if (playerProfile.IsMissingFields())
        {
            Debug.Log("Player profile is missing fields. Please fill in all the required fields before playing.");
            // You can also display a message to the player about the missing fields, e.g., using a UI element
        }
        else
        {
            Debug.Log("All player profile fields are complete. Proceeding to play the game...");
            // Add code here to load the game scene or perform other actions necessary to start the game
        }
    }

    public void PlayerProfile()
    {
        Debug.Log("Player profile");
    }

    public void Leaderboard()
    {
        Debug.Log("Leaderboard");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        // Remove the JWT token from the PlayerPrefs
        PlayerPrefs.DeleteKey("jwtToken");
        Debug.Log("JWT Token removed from PlayerPrefs");
    }
}
