using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    // Reference to the LoadingScene
    private LoadingScene loadingScene;


    // UI Elements
    public TextMeshProUGUI pointsText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI gameOverText;
    public Button quitButtonRight; 
    public TextMeshProUGUI quitButtonRightText;
    public Button keepPlayingButtonLeft; 
    public TextMeshProUGUI keepPlayingButtonLeftText;
    public GameObject player;


    // Static variables
    public static bool isGameOver;
    public int numberOfPoints = 0;
    public static Vector3 playerSafePosition;

    // Number of keys
    private int numberOfKeys;

    // Flag to control Update execution
    private bool isUpdatePaused;

    
    private void Awake()
    {
        // Initialize the flags
        isGameOver = false;
        isUpdatePaused = false;

        // Get the player position
        playerSafePosition = player.transform.position;
        UpdatePointsUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdatePaused) return;

        //pointsText.text = numberOfPoints.ToString();
        if(isGameOver)
        {
            // Pause the update
            isUpdatePaused = true;

            ShowGameOverPanel();
        }
    }

    public void AddPoints(int points)
    {
        numberOfPoints += points;
        UpdatePointsUI();
    }

    private void UpdatePointsUI()
    {
        pointsText.text = numberOfPoints.ToString();
    }

    private void ShowGameOverPanel()
    {
        // Disable the player
        player.SetActive(false);

        // Display the game over panel
        gameOverPanel.SetActive(true);

        // Display the title and game over text
        panelTitleText.text = "OOPS!";
        gameOverText.text = "Game Over\nYou have been killed!\n \nKeep going using a KEY?";
        quitButtonRightText.text = "Quit";
        keepPlayingButtonLeftText.text = "Keep Playing";

        // Wait for button click
        quitButtonRight.onClick.AddListener(QuitGame);
        keepPlayingButtonLeft.onClick.AddListener(KeepPlaying);
    }

    private void QuitGame()
    {
        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(QuitGame);
        keepPlayingButtonLeft.onClick.RemoveListener(KeepPlaying);
        
        // Load the Main Menu
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene("MainMenu");

        // Resume the update
        isUpdatePaused = false;
    }

    private void KeepPlaying()
    {
        // Get the number of keys from the player preferences
        numberOfKeys = PlayerPrefs.GetInt("revivalKeys", 0);
        Debug.Log("Number of keys before revival: " + numberOfKeys);

        // Reduce the no. of revivalKeys
        numberOfKeys--;
        numberOfKeys = Mathf.Max(0, numberOfKeys);
        PlayerPrefs.SetInt("revivalKeys", numberOfKeys);
        PlayerPrefs.Save();
        Debug.Log("Number of keys after revival: " + PlayerPrefs.GetInt("revivalKeys", 0));

        // Enable the player
        player.SetActive(true);
        isGameOver = false;

        // Restore player health
        HealthManager.health = 3;

        // Respwan the player
        player.transform.position = playerSafePosition;

        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(QuitGame);
        keepPlayingButtonLeft.onClick.RemoveListener(KeepPlaying);

        // Disable the game over panel
        gameOverPanel.SetActive(false);

        // Resume the update
        isUpdatePaused = false;
    }
}
