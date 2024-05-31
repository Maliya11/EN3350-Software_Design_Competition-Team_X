using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class OutOfBounds : Singleton<OutOfBounds>
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
            // Display the game over panel
            gameOverPanel.SetActive(true);
            
            // Display the title and game over text
            panelTitleText.text = "OOPS!";
            gameOverText.text = "Game Over\nYou went out of bounds!\n \nKeep going using a KEY?";
            quitButtonRightText.text = "Quit";
            keepPlayingButtonLeftText.text = "Keep Playing";

            // Reduce the player health
            HealthManager.health = 0;

            // Remove the physics from the player
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            // Flag the player as dead
            PlayerManager.isPlayerDead = true;
        }
    }
}
