using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class OutOfBounds : MonoBehaviour
{
    // UI Elements
    public GameObject gameOverPanel;
    public GameObject keepPlayingButton;
    public GameObject restartButton;
    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI gameOverText;
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Display the game over panel
            gameOverPanel.SetActive(true);

            // Display the title and game over text
            panelTitleText.text = "OOPS!";
            gameOverText.text = "Game Over\nYou went out of bounds!\n \nKeep going using a KEY?";

            // Disable the player
            collision.gameObject.SetActive(false);

            // Enable the restart button
            restartButton.SetActive(true);

            // Enable the keep playing button
            keepPlayingButton.SetActive(true);            
        }
    }

    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KeepPlaying()
    {
        // Enable the player
        player.SetActive(true);

        // Disable the game over panel
        gameOverPanel.SetActive(false);
    }
}
