using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuitGameLevelManager : Singleton<QuitGameLevelManager>
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
    // public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Enable the buttons
        quitButtonRight.interactable = true;
        resumeButtonLeft.interactable = true;
    }

    public void DisplayQuitPanel()
    {
        // Enable the quit panel
        quitPanel.SetActive(true);

        // Set the quit panel content
        quitPanelTitle.text = "Quit?";
        quitPanelText.text = "Are you sure you want to quit? \nYou will lose all progress.";
        quitButtonRightText.text = "Quit";
        resumeButtonLeftText.text = "Resume";

        // Remove all the listeners
        quitButtonRight.onClick.RemoveAllListeners();
        resumeButtonLeft.onClick.RemoveAllListeners();

        // Add listeners to the buttons
        quitButtonRight.onClick.AddListener(QuitGame);
        resumeButtonLeft.onClick.AddListener(ResumeGame);
    }
    
    private void QuitGame()
    {
        // Remove all the listeners
        quitButtonRight.onClick.RemoveAllListeners();
        resumeButtonLeft.onClick.RemoveAllListeners();

        // Hide the canvas details from method in PlayerManager 
        FindObjectOfType<PlayerManager>().HideCanvasDetails();
        
        // Load the Main Menu
        loadingScene = FindObjectOfType<LoadingScene>();
        loadingScene.LoadScene("MainMenu");
    }

    private void ResumeGame()
    {
        // Remove all the listeners
        quitButtonRight.onClick.RemoveAllListeners();
        resumeButtonLeft.onClick.RemoveAllListeners();

        // Disable the quit panel
        quitPanel.SetActive(false);        
    }
}
