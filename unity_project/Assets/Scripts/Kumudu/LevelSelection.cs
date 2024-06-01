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
    public Sprite[] starsSprites;
    public TextMeshProUGUI Score;
    public int index;

    private void Update()
    {
        UpdateHighScoreUI();
        UpdateLevelButton();//TODO Remove later
        UnlockLevel();
    }

    private void UpdateHighScoreUI()
    {
        string highScoreKey = "HighScore_Level_" + index;
        int highestPoints = PlayerPrefs.GetInt(highScoreKey, 0);
        Score.text = highestPoints.ToString();
    }

    private void UnlockLevel()
    {
        int previousLvIndex = index - 1;// PlayerPrefs.GetInt("Lv" + gameObject.name) - 1;
        if(PlayerPrefs.GetInt("HighStar_Level_" + previousLvIndex) > 0)//At least get one stars in previous level
        {
            isUnlocked = true;//can unlock the next level
        }
    }

    private void UpdateLevelButton()
    {
        if(isUnlocked)//MARKER We can play this level
        {
            lockImage.gameObject.SetActive(false);//we dont want to see the lock image
            for(int i = 0; i < starsImages.Length; i++)
            {
                starsImages[i].gameObject.SetActive(true);
            }

            for(int i = 0; i < PlayerPrefs.GetInt("HighStar_Level_" + index); i++)
            {
                starsImages[i].sprite = starsSprites[i];
            }
        }
        else
        {
            lockImage.gameObject.SetActive(true);
            for (int i = 0; i < starsImages.Length; i++)
            {
                starsImages[i].gameObject.SetActive(false);
            }
        }
    }

    //MARKER We have remvoed this method from UIMANAGER for easy to read and operator
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
