using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public bool isUnlocked = false;
    public Image lockImage;//LOCK IMAGE
    public Image[] starsImages;//THREE STAR IMAGE
    public Sprite[] starsSprites;

    private void Update()
    {
        UpdateLevelButton();//TODO Remove later
        UnlockLevel();
    }

    private void UnlockLevel()
    {
        int previousLvIndex = int.Parse(gameObject.name) - 1;// PlayerPrefs.GetInt("Lv" + gameObject.name) - 1;
        if(PlayerPrefs.GetInt("Lv" + previousLvIndex) > 0)//At least get one stars in previous level
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

            for(int i = 0; i < PlayerPrefs.GetInt("Lv" + gameObject.name); i++)
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
    public void SceneTransition(int index)
    {
        if(isUnlocked)
        {
            UIManager.instance.HideAllUIPanels();
            SceneManager.LoadScene(index);    
        }
    }
}
