using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuitGameLevelManager : MonoBehaviour
{
    // Reference to the LoadingScene
    private LoadingScene loadingScene;


    // UI Elements
    public GameObject quitPanel;
    public TextMeshProUGUI quitPanelTitle;
    public TextMeshProUGUI quitPanelText;
    public Button quitButtonRight;
    public TextMeshProUGUI quitButtonRightText;
    public Button resumeButtonLeft;
    public TextMeshProUGUI resumeButtonLeftText;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        // Enable the buttons
        quitButtonRight.interactable = true;
        resumeButtonLeft.interactable = true;
    }

    public void DisplayQuitPanel()
    {
        // Disable the player
        player.SetActive(false);

        // Enable the quit panel
        quitPanel.SetActive(true);

        // Set the quit panel content
        quitPanelTitle.text = "Quit?";
        quitPanelText.text = "Are you sure you want to quit? \nYou will lose all progress.";
        quitButtonRightText.text = "Quit";
        resumeButtonLeftText.text = "Resume";

        // Add listeners to the buttons
        quitButtonRight.onClick.AddListener(QuitGame);
        resumeButtonLeft.onClick.AddListener(ResumeGame);
    }
    
    private void QuitGame()
    {
        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(QuitGame);
        resumeButtonLeft.onClick.RemoveListener(ResumeGame);
        
        // Load the Main Menu
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene(1);
    }

    private void ResumeGame()
    {
        // Enable the player
        player.SetActive(true);

        // Disable the quit panel
        quitPanel.SetActive(false);

        // Remove the listeners
        quitButtonRight.onClick.RemoveListener(QuitGame);
        resumeButtonLeft.onClick.RemoveListener(ResumeGame);
    }
}
