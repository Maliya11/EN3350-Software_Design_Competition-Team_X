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
    public static int numberOfPoints;
    public static Vector3 playerSafePosition;

    
    private void Awake()
    {
        numberOfPoints = PlayerPrefs.GetInt("NumberOfPoints", 0); //default value is 0
        isGameOver = false;
        // Get the player position
        playerSafePosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = numberOfPoints.ToString();
        if(isGameOver)
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
    }

    private void QuitGame()
    {
        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(QuitGame);
        keepPlayingButtonLeft.onClick.RemoveListener(KeepPlaying);
        
        // Load the Main Menu
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene(1);
    }

    private void KeepPlaying()
    {
        // Enable the player
        player.SetActive(true);
        isGameOver = false;

        // Restore player health
        HealthManager.health = 3;

        // Respwan the player
        player.transform.position = playerSafePosition;

        // Disable the game over panel
        gameOverPanel.SetActive(false);

        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(QuitGame);
        keepPlayingButtonLeft.onClick.RemoveListener(KeepPlaying);
    }

    
}
