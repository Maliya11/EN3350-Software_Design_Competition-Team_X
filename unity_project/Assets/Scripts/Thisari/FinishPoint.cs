using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishPoint : MonoBehaviour
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


    private void Start()
    {
        // Enable the buttons
        quitButtonRight.interactable = true;
        restartButtonLeft.interactable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached the finish point!");
            PlayerManager.numberOfPoints += 100;
            PlayerPrefs.SetInt("CGY1_Points", PlayerManager.numberOfPoints);
            Debug.Log("Points: " + PlayerManager.numberOfPoints);
            
            // Display the game over panel
            gameOverPanel.SetActive(true);

            // Display the title and game over text
            panelTitleText.text = "YAYYY!";
            gameOverText.text = "Congratulations!\nYou have reached the finish point!\n";
            quitButtonRightText.text = "Return";
            restartButtonLeftText.text = "Play Again";

            // Disable the player
            collision.gameObject.SetActive(false);

            // Add listeners to the buttons
            quitButtonRight.onClick.AddListener(ReturnToMainMenu);
            restartButtonLeft.onClick.AddListener(PlayAgain);
        }
    }

    private void ReturnToMainMenu()
    {
        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(ReturnToMainMenu);
        restartButtonLeft.onClick.RemoveListener(PlayAgain);

        // Load the Main Menu
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene(1);
    }

    private void PlayAgain()
    {
        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(ReturnToMainMenu);
        restartButtonLeft.onClick.RemoveListener(PlayAgain);

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
