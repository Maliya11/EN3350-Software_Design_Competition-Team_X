using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : Singleton<SettingsManager>
{
    // UI Elements
    public GameObject settingsPanel;
    public GameObject settingsPanelContent;
    public GameObject settingsPanelMessage;
    public TextMeshProUGUI settingsPanelTitle;
    public Button resumeButtonRight;
    public TextMeshProUGUI resumeButtonRightText;
    public Button saveButtonLeft;
    public TextMeshProUGUI saveButtonLeftText;
    // public GameObject player;

    private void Start()
    {
        // Enable the buttons
        resumeButtonRight.interactable = true;
        saveButtonLeft.interactable = true;
    }

    public void DisplaySettingsPanel()
    {
        // Pause the Treasure Manager and Energy Manager
        TreasureManager.isPausedTM = true;
        EnergyManager.isPausedEM = true;

        // Enable the settings panel
        settingsPanel.SetActive(true);
        settingsPanelContent.SetActive(true);

        // Disable the settings panel message
        settingsPanelMessage.SetActive(false);

        // Set the settings panel content
        settingsPanelTitle.text = "Settings";
        resumeButtonRightText.text = "Resume";
        saveButtonLeftText.text = "Save";

        // Remove all the listeners
        resumeButtonRight.onClick.RemoveAllListeners();
        saveButtonLeft.onClick.RemoveAllListeners();

        // Add listeners to the buttons
        resumeButtonRight.onClick.AddListener(ResumeGame);
        saveButtonLeft.onClick.AddListener(SaveSettings);
    }

    private void ResumeGame()
    {
        // Unpause the treasure manager
        TreasureManager.isPausedTM = false;

        // Remove all the listeners
        resumeButtonRight.onClick.RemoveAllListeners();
        saveButtonLeft.onClick.RemoveAllListeners();

        // Disable the settings panel
        settingsPanelContent.SetActive(false);
        settingsPanel.SetActive(false);

        // Enable the settings panel message
        settingsPanelMessage.SetActive(true);
    }

    private void SaveSettings()
    {
        // Resume the Treasure Manager and Energy Manager
        TreasureManager.isPausedTM = false;
        EnergyManager.isPausedEM = false;

        // Save the settings
        Debug.Log("Settings saved!");

        // Remove all the listeners
        resumeButtonRight.onClick.RemoveAllListeners();
        saveButtonLeft.onClick.RemoveAllListeners();
        
        // Disable the settings panel
        settingsPanelContent.SetActive(false);
        settingsPanel.SetActive(false);

        // Enable the settings panel message
        settingsPanelMessage.SetActive(true);
    }
}
