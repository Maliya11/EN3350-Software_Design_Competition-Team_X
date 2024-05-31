using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject mapSelectionPanel;
    public GameObject characterSelectionPanel;
    public GameObject[] levelSelectionPanels;

    [Header("Our STAR UI")]
    public int stars;
    public TextMeshProUGUI startText;
    public MapSelection[] mapSelections;
    public TextMeshProUGUI[] questStarsTexts;
    public TextMeshProUGUI[] lockedStarsTexts;
    public TextMeshProUGUI[] unlockStarsTexts;

    
    //new edit
    // public CharacterSelect characterSelectionPanel;
  
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()//TODO REmove this method because we don't want to call these methods each frame
    {
        UpdateStarUI();
        UpdateLockedStarUI();
        UpdateUnLockedStarUI();
    }

    private void UpdateLockedStarUI()
    {
        for(int i = 0; i < mapSelections.Length; i++)
        {
            questStarsTexts[i].text = mapSelections[i].questNum.ToString();

            if (mapSelections[i].isUnlock == false)//If one of the Map is locked
            {
                lockedStarsTexts[i].text = stars.ToString() + "/" + mapSelections[i].endLevel * 3;
            }
        }
    }

    private void UpdateUnLockedStarUI()//TODO FIXED LATER this method
    {
        for(int i = 0; i < mapSelections.Length; i++)
        {
            unlockStarsTexts[i].text = stars.ToString() + "/" + mapSelections[i].endLevel * 3;

            switch(i)
            {
                case 0://MARKER MAP 01
                    unlockStarsTexts[i].text = (PlayerPrefs.GetInt("Lv" + 1) + PlayerPrefs.GetInt("Lv" + 2) + PlayerPrefs.GetInt("Lv" + 3)) + "/" + (mapSelections[i].endLevel - mapSelections[i].startLevel + 1) * 3;
                    break;
                case 1://MARKER MAP 02
                    unlockStarsTexts[i].text = (PlayerPrefs.GetInt("Lv" + 4) + PlayerPrefs.GetInt("Lv" + 5) + PlayerPrefs.GetInt("Lv" + 6)) + "/" + (mapSelections[i].endLevel - mapSelections[i].startLevel + 1) * 3;
                    break;
            }
        }
    }

    //MARKER Update OUR Stars UI on the top left connor
    private void UpdateStarUI()
    {
        stars = PlayerPrefs.GetInt("Lv" + 1) + PlayerPrefs.GetInt("Lv" + 2) + PlayerPrefs.GetInt("Lv" + 3) + PlayerPrefs.GetInt("Lv" + 4)
         + PlayerPrefs.GetInt("Lv" + 5) + PlayerPrefs.GetInt("Lv" + 6);
        startText.text = stars.ToString();
    }

    public void PressMapButton(int _mapIndex)//MARKER This method will be triggered when we press the (FOUR) map button
    {
        if(mapSelections[_mapIndex].isUnlock == true)//You can open this map
        {
            levelSelectionPanels[_mapIndex].gameObject.SetActive(true);
            mapSelectionPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("You cannot open this map now. Please work hard to collect more stars");
        }
    }

    public void BackButton()
    {
        mapSelectionPanel.gameObject.SetActive(true);
        for(int i = 0; i < mapSelections.Length; i++)
        {
            levelSelectionPanels[i].gameObject.SetActive(false);
        }
    }

    public void HideAllUIPanels()
    {
        mapSelectionPanel.gameObject.SetActive(false);
        for (int i = 0; i < levelSelectionPanels.Length; i++)
        {
            levelSelectionPanels[i].gameObject.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    
}
