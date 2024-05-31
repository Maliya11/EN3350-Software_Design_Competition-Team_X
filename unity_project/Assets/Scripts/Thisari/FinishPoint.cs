using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishPoint : Singleton<FinishPoint>
{
    // Reference to the LoadingScene
    private LoadingScene loadingScene;

    // UI Elements
    public GameObject gameOverPanel;
    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI gameOverText;
    public Button quitButtonRight;
    public TextMeshProUGUI quitButtonRightText;
    public Button restartButtonLeft;
    public TextMeshProUGUI restartButtonLeftText;
    public PlayerManager playerManager;
    public TreasureManager treasureManager;

    // Variable 
    public static bool isGameOver;

    private void Start()
    {
        // Initialize the flags
        isGameOver = false;

        // Enable the buttons
        quitButtonRight.interactable = true;
        restartButtonLeft.interactable = true;

        playerManager = FindObjectOfType<PlayerManager>();
        treasureManager = FindObjectOfType<TreasureManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached the finish point!");

            // Flag the game as over
            isGameOver = true;

            // Display the game over panel
            gameOverPanel.SetActive(true);

            // Display the title and game over text
            panelTitleText.text = "YAYYY!";
            gameOverText.text = "Congratulations!\nYou have reached the finish point!\n";
            quitButtonRightText.text = "Return";
            restartButtonLeftText.text = "Play Again";

            // Remove all the listeners
            quitButtonRight.onClick.RemoveAllListeners();
            restartButtonLeft.onClick.RemoveAllListeners();

            // Add listeners to the buttons
            quitButtonRight.onClick.AddListener(ReturnToMainMenu);
            restartButtonLeft.onClick.AddListener(PlayAgain);

            // Add points and potions to the player
            AddPointsAndPotions();
        }
    }

    private void AddPointsAndPotions()
    {
        // Add points for finishing the level
        playerManager.AddPoints(100);

        // Update high score
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        string highScoreKey = "HighScore_Level_" + currentLevelIndex;
        int highestPoints = PlayerPrefs.GetInt(highScoreKey, 0);

        if(playerManager.numberOfPoints > highestPoints)
        {
            PlayerPrefs.SetInt(highScoreKey, playerManager.numberOfPoints);
            PlayerPrefs.Save();
        }

        Debug.Log("Points: " + playerManager.numberOfPoints);

        // Add potions collected to the inventory
        int noOfPotionsCollected = treasureManager.potionsCollected;
        int currentPotions = PlayerPrefs.GetInt("revivalPotions", 0);
        PlayerPrefs.SetInt("revivalPotions", currentPotions + noOfPotionsCollected);
        PlayerPrefs.Save();

        Debug.Log("Potions collected: " + noOfPotionsCollected);
        Debug.Log("Total potions: " + PlayerPrefs.GetInt("revivalPotions", 0));
    }

    private void ReturnToMainMenu()
    {
        // Remove all the listeners
        quitButtonRight.onClick.RemoveAllListeners();
        restartButtonLeft.onClick.RemoveAllListeners();

        // Hide the canvas details
        playerManager.HideCanvasDetails();

        // Load the Main Menu
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene("MainMenu");
    }

    private void PlayAgain()
    {
        // Remove all the listeners
        quitButtonRight.onClick.RemoveAllListeners();
        restartButtonLeft.onClick.RemoveAllListeners();

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
