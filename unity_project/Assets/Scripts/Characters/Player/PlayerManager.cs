using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    /*
    This script is used to manage player properties => player selection, player points, player stars, player potions,
    tarnsition between scenes and panels, reviving the player
    */

    // Reference to the LoadingScene
    private LoadingScene loadingScene;

    // UI Elements
    public GameObject HeartContainer;
    public GameObject Buttons;
    public GameObject Points;
    public GameObject Treasures;
    public GameObject Potions;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI potionText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI gameOverText;
    public Button quitButtonRight; 
    public TextMeshProUGUI quitButtonRightText;
    public Button keepPlayingButtonLeft; 
    public TextMeshProUGUI keepPlayingButtonLeftText;

    // Game Objects
    public GameObject[] playerPrefabs;
    int characterIndex;
    public static GameObject player;
    public static Vector3 playerSafePosition;
    public CinemachineVirtualCamera VCam;

    // Static variables
    public static bool isPlayerDead;
    public int numberOfPoints = 0;
    public int numberOfStars = 0;
    public int numberOfEnemiesOfTheScene;
    public int enemyKills = 0;

    // Number of potions
    private int numberOfPotions;

    // Flag to control Update execution
    private bool isUpdatePaused;
    public static bool isNinja;

    private void Awake()
    {
        Vector3 initialPlayerPosition = new Vector3(-62.05f, -0.14f, 1);
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);  //get selected character from character selection panel in the mainmenu
        Debug.Log("Character Index in GameLevel: " + characterIndex);
        player = Instantiate(playerPrefabs[characterIndex], initialPlayerPosition, Quaternion.identity);  //instantiate the player using the index
        VCam.m_Follow = player.transform;  //set the virtual cam to follow the player

        //check the index to identify whether the player is a ninja or a robot
        if(characterIndex == 2)
        {
            isNinja = false;
        }
        else
        {
            isNinja = true;
        }

        // Initialize the flags
        isPlayerDead = false;
        isUpdatePaused = false;

        // Get the player position
        playerSafePosition = player.transform.position;
        UpdatePointsUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePotionUI();  //update player points in the canvas

        if (isUpdatePaused) return;

        //pointsText.text = numberOfPoints.ToString();
        if(isPlayerDead)
        {
            // Pause the update
            isUpdatePaused = true;

            // Pause the Treasure Manager and Energy Manager
            TreasureManager.isPausedTM = true;
            EnergyManager.isPausedEM = true;

            ShowGameOverPanel();   //if player is dead show gameOverPanel
        }
    }

    public void AddPoints(int points)
    {
        //this is called in enemy scripts and finishpoints.cs to increase the player points
        numberOfPoints += points;  
        UpdatePointsUI();  //update the canvas
    }

    public void AddStars(int star)
    {
        //increment the stars, used in the finishpoint.cs
        numberOfStars += star;
    }

    private void UpdatePointsUI()
    {
        //updates the points text in the canvas
        pointsText.text = numberOfPoints.ToString();
    }

    private void UpdatePotionUI()
    {
        //updates the potions text in the canvas
        potionText.text = PlayerPrefs.GetInt("revivalPotions", 0).ToString();
    }

    private void ShowGameOverPanel()
    {
        // Display the game over panel
        gameOverPanel.SetActive(true);

        // Display the title and game over text
        panelTitleText.text = "OOPS!";
        gameOverText.text = $"Game Over\nYou have been killed!\n Keep going using a Revival Potion? \n You have {PlayerPrefs.GetInt("revivalPotions", 0)} revival potions";
        quitButtonRightText.text = "Quit";
        keepPlayingButtonLeftText.text = "Keep Playing";

        // Remove all the listeners
        quitButtonRight.onClick.RemoveAllListeners();
        keepPlayingButtonLeft.onClick.RemoveAllListeners();

        // Make the keep playing button not interactable if there are no revival potions
        if (PlayerPrefs.GetInt("revivalPotions", 0) <= 0)
        {
            Debug.Log("No revival potions available");
            keepPlayingButtonLeft.interactable = false;
        }
        else
        {
            Debug.Log("Revival potions available");
            keepPlayingButtonLeft.interactable = true;
        }

        // Wait for button click
        quitButtonRight.onClick.AddListener(QuitGame);
        keepPlayingButtonLeft.onClick.AddListener(KeepPlaying);
    }

    private void QuitGame()
    {
        // Remove all the listeners
        quitButtonRight.onClick.RemoveAllListeners();
        keepPlayingButtonLeft.onClick.RemoveAllListeners();

        // Hide the canvas details
        HideCanvasDetails();
        
        // Load the Main Menu
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene("SelectionScene");

        // Restore the physics of the player
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Resume the update
        isUpdatePaused = false;
    }

    private void KeepPlaying()
    {
        // Resume the Treasure Manager and Energy Manager
        TreasureManager.isPausedTM = false;
        EnergyManager.isPausedEM = false;

        // Restore the player
        RestorePlayer();

        // Remove all the listeners
        quitButtonRight.onClick.RemoveAllListeners();
        keepPlayingButtonLeft.onClick.RemoveAllListeners();

        // Disable the game over panel
        gameOverPanel.SetActive(false);

        // Resume the update
        isUpdatePaused = false;
    }

    private void RestorePlayer()
    {
        // Get the number of potions from the player preferences
        numberOfPotions = PlayerPrefs.GetInt("revivalPotions", 0);
        Debug.Log("Number of potions before revival: " + numberOfPotions);

        // Reduce the no. of revivalPotions
        numberOfPotions--;
        numberOfPotions = Mathf.Max(0, numberOfPotions);
        PlayerPrefs.SetInt("revivalPotions", numberOfPotions);
        PlayerPrefs.Save();
        Debug.Log("Number of potions after revival: " + PlayerPrefs.GetInt("revivalPotions", 0));

        // Revive the player
        isPlayerDead = false;

        // Restore player health
        HealthManager.health = 3;

        // Respwan the player
        player.transform.position = playerSafePosition;

        // Restore the physics of the player
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void HideCanvasDetails()
    {
        // Hide the canvas details other than the loading screen 
        HeartContainer.SetActive(false);
        Buttons.SetActive(false);
        Points.SetActive(false);
        Treasures.SetActive(false);
        Potions.SetActive(false);
    }
}