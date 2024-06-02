using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager instance;

    // UI panels for map selection, character selection, and level selection
    public GameObject mapSelectionPanel;
    public GameObject[] levelSelectionPanels;

    [Header("Our STAR UI")]
    public int stars;  // Number of stars the player has collected
    public TextMeshProUGUI startText;  // Text element to display the star count  
    public MapSelection[] mapSelections; // Array of MapSelection objects

    // Arrays of Text elements for displaying quest stars, locked stars, and unlocked stars
    public TextMeshProUGUI[] questStarsTexts;
    public TextMeshProUGUI[] lockedStarsTexts;
    public TextMeshProUGUI[] unlockStarsTexts;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
    }

    private void Update()
    {
        // Update the star UI elements
        UpdateStarUI();
        UpdateLockedStarUI();
        UpdateUnLockedStarUI();
    }

    // Update the UI for locked stars
    private void UpdateLockedStarUI()
    {
        for(int i = 0; i < mapSelections.Length; i++)
        {
            questStarsTexts[i].text = mapSelections[i].questNum.ToString(); // Update the quest stars text

            if (mapSelections[i].isUnlock == false) // If the map is locked, update the locked stars text
            {
                lockedStarsTexts[i].text = stars.ToString() + "/" + mapSelections[i].endLevel * 3;
            }
        }
    }

    // Update the UI for unlocked stars
    private void UpdateUnLockedStarUI()
    {
        for(int i = 0; i < mapSelections.Length; i++)
        {
            // Update the unlocked stars text
            unlockStarsTexts[i].text = stars.ToString() + "/" + mapSelections[i].endLevel * 3;

            // Specific updates based on map index
            switch(i)
            {
                case 0:// MAP 01
                    unlockStarsTexts[i].text = (PlayerPrefs.GetInt("HighStar_Level_" + 3) + PlayerPrefs.GetInt("HighStar_Level_" + 4) + PlayerPrefs.GetInt("HighStar_Level_" + 5)) + "/" + (mapSelections[i].endLevel - mapSelections[i].startLevel + 1) * 3;
                    break;
                case 1:// MAP 02
                    unlockStarsTexts[i].text = (PlayerPrefs.GetInt("HighStar_Level_" + 6) + PlayerPrefs.GetInt("HighStar_Level_" + 7) + PlayerPrefs.GetInt("HighStar_Level_" + 8)) + "/" + (mapSelections[i].endLevel - mapSelections[i].startLevel + 1) * 3;
                    break;
            }
        }
    }

    //Update the Stars UI on the top left connor
    private void UpdateStarUI()
    {
        stars = PlayerPrefs.GetInt("HighStar_Level_" + 3) + PlayerPrefs.GetInt("HighStar_Level_" + 4) + PlayerPrefs.GetInt("HighStar_Level_" + 5) + PlayerPrefs.GetInt("HighStar_Level_" + 6)
         + PlayerPrefs.GetInt("HighStar_Level_" + 7) + PlayerPrefs.GetInt("HighStar_Level_" + 8);
        startText.text = stars.ToString();
    }

    public void PressMapButton(int _mapIndex)
    {
        if(mapSelections[_mapIndex].isUnlock == true)
        {
            // Show the level selection panel for the selected map and hide the map selection panel
            levelSelectionPanels[_mapIndex].gameObject.SetActive(true);
            mapSelectionPanel.gameObject.SetActive(false);
        }
        else
        {
            // Log a message if the map is locked
            Debug.Log("You cannot open this map now. Please work hard to collect more stars");
        }
    }

    public void BackButton()
    {
        // Show the map selection panel and hide all level selection panels
        mapSelectionPanel.gameObject.SetActive(true);
        for(int i = 0; i < mapSelections.Length; i++)
        {
            levelSelectionPanels[i].gameObject.SetActive(false);
        }
    }

    // Hide all UI panels
    public void HideAllUIPanels()
    {
        mapSelectionPanel.gameObject.SetActive(false);
        for (int i = 0; i < levelSelectionPanels.Length; i++)
        {
            levelSelectionPanels[i].gameObject.SetActive(false);
        }
    }

    // Load the main menu scene
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    
}
