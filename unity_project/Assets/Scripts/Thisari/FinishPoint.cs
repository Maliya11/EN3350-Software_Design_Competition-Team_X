using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishPoint : MonoBehaviour
{
    // UI Elements
    public GameObject gameOverPanel;
    public GameObject keepPlayingButton;
    public GameObject restartButton;
    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI gameOverText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached the finish point!");

            // Display the game over panel
            gameOverPanel.SetActive(true);

            // Display the title and game over text
            panelTitleText.text = "YAYYY!";
            gameOverText.text = "Congratulations!\nYou have reached the finish point!\n \nKeep going using a KEY?";

            // Disable the player
            collision.gameObject.SetActive(false);

            // Enable the restart button
            restartButton.SetActive(true);

            // Enable the keep playing button
            keepPlayingButton.SetActive(true);
        }
    }
}
