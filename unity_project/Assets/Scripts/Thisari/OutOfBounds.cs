using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class OutOfBounds : MonoBehaviour
{
    // Reference to the LoadingScene
    private LoadingScene loadingScene;


    // UI Elements
    public GameObject gameOverPanel;
    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI gameOverText;
    public Button quitButtonRight; 
    public TextMeshProUGUI quitButtonRightText;
    public Button keepPlayingButtonLeft; 
    public TextMeshProUGUI keepPlayingButtonLeftText;
    public GameObject player;


    private void Start()
    {
        // Enable the buttons
        quitButtonRight.interactable = true;
        keepPlayingButtonLeft.interactable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Display the title and game over text
            panelTitleText.text = "OOPS!";
            gameOverText.text = "Game Over\nYou went out of bounds!\n \nKeep going using a KEY?";
            quitButtonRightText.text = "Quit";
            keepPlayingButtonLeftText.text = "Keep Playing";

            // Wait for button click
            quitButtonRight.onClick.AddListener(QuitGame);
            keepPlayingButtonLeft.onClick.AddListener(KeepPlaying);
            
            // Display the game over panel
            gameOverPanel.SetActive(true);

            // Disable the player
            player.SetActive(false);
        }
    }

    public void QuitGame()
    {
        // Load the Main Menu
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene(1);
    }

    public void KeepPlaying()
    {
        // Enable the player
        player.SetActive(true);

        // Disable the game over panel
        gameOverPanel.SetActive(false);

        // Respwan the player using the respawn method in player controller 
        player.GetComponent<PlayerController>().Respawn();

    }
}
