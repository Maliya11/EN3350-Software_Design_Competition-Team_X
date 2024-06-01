using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    // Reference to the LoadingScene
    private LoadingScene loadingScene;
    
    public bool isUnlocked = false;
    public Image lockImage;//LOCK IMAGE
    public Image[] starsImages;//THREE STAR IMAGE
    public Sprite[] starsSprites; // Sprites for the stars
    public TextMeshProUGUI Score; // Text element to display the score
    public int index; // Index of the current level

    private void Update()
    {
        UpdateHighScoreUI(); // Update the high score and level button UI elements
        UpdateLevelButton();
        UnlockLevel(); // Check if the level can be unlocked
    }

    // Update the high score UI element
    private void UpdateHighScoreUI()
    {
        string highScoreKey = "HighScore_Level_" + index;  // Construct the key for the high score of the current level
        int highestPoints = PlayerPrefs.GetInt(highScoreKey, 0);  // Get the highest points from PlayerPrefs
        Score.text = highestPoints.ToString();  // Update the score text with the highest points
    }

    private void UnlockLevel()
    {
        int previousLvIndex = index - 1;// PlayerPrefs.GetInt("Lv" + gameObject.name) - 1;
        if(PlayerPrefs.GetInt("HighStar_Level_" + previousLvIndex) > 0)//At least get one stars in previous level
        {
            isUnlocked = true;//can unlock the next level
        }
    }

    // Unlock the level if the previous level has been completed
    private void UpdateLevelButton()
    {
        if(isUnlocked)
        {
            lockImage.gameObject.SetActive(false); // Hide the lock image if the level is unlocked

            // Show the star images
            for(int i = 0; i < starsImages.Length; i++)
            {
                starsImages[i].gameObject.SetActive(true);
            }

            // Update the stars based on the high score
            for(int i = 0; i < PlayerPrefs.GetInt("HighStar_Level_" + index); i++)
            {
                starsImages[i].sprite = starsSprites[i];
            }
        }
        else
        {
            lockImage.gameObject.SetActive(true);  // Show the lock image if the level is locked

            // Hide the star images
            for (int i = 0; i < starsImages.Length; i++)
            {
                starsImages[i].gameObject.SetActive(false);
            }
        }
    }

    // Transition to the specified scene if the level is unlocked
    public void SceneTransition(string sceneName)
    {
        if(isUnlocked)
        {
            UIManager.instance.HideAllUIPanels();
            
            // Load the Main Menu
            loadingScene = FindObjectOfType<LoadingScene>();
            loadingScene.LoadScene(sceneName);
        }
    }
}
