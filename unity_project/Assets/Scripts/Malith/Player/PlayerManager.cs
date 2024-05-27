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
    public TextMeshProUGUI potionText;
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

    // Number of potions
    private int numberOfPotions;

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
        UpdatePotionUI();

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

    private void UpdatePotionUI()
    {
        potionText.text = PlayerPrefs.GetInt("revivalPotions", 0).ToString();
    }

    private void ShowGameOverPanel()
    {
        // Display the game over panel
        gameOverPanel.SetActive(true);

        // Display the title and game over text
        panelTitleText.text = "OOPS!";
        gameOverText.text = "Game Over\nYou have been killed!\n \nKeep going using a Revival Potion?";
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

        // Restore the physics of the player
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Resume the update
        isUpdatePaused = false;
    }

    private void KeepPlaying()
    {
        // Restore the player
        RestorePlayer();

        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(QuitGame);
        keepPlayingButtonLeft.onClick.RemoveListener(KeepPlaying);

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
        isGameOver = false;

        // Restore player health
        HealthManager.health = 3;

        // Respwan the player
        player.transform.position = playerSafePosition;

        // Restore the physics of the player
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
