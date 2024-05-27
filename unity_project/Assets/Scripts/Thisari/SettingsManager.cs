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
    public GameObject player;


    private void Start()
    {
        // Enable the buttons
        resumeButtonRight.interactable = true;
        saveButtonLeft.interactable = true;
    }

    public void DisplaySettingsPanel()
    {
        // Enable the settings panel
        settingsPanel.SetActive(true);
        settingsPanelContent.SetActive(true);

        // Disable the settings panel message
        settingsPanelMessage.SetActive(false);

        // Set the settings panel content
        settingsPanelTitle.text = "Settings";
        resumeButtonRightText.text = "Resume";
        saveButtonLeftText.text = "Save";
        resumeButtonRight.onClick.AddListener(ResumeGame);
        saveButtonLeft.onClick.AddListener(SaveSettings);
    }

    private void ResumeGame()
    {
        // Disable the settings panel
        settingsPanelContent.SetActive(false);
        settingsPanel.SetActive(false);

        // Enable the settings panel message
        settingsPanelMessage.SetActive(true);

        // Remove the listeners
        resumeButtonRight.onClick.RemoveListener(ResumeGame);
        saveButtonLeft.onClick.RemoveListener(SaveSettings);
    }

    private void SaveSettings()
    {
        // Save the settings
        Debug.Log("Settings saved!");
        
        // Disable the settings panel
        settingsPanelContent.SetActive(false);
        settingsPanel.SetActive(false);

        // Enable the settings panel message
        settingsPanelMessage.SetActive(true);

        // Remove the listeners
        resumeButtonRight.onClick.RemoveListener(ResumeGame);
        saveButtonLeft.onClick.RemoveListener(SaveSettings);
    }
}
