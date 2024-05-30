using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleLevel : MonoBehaviour
{
    private int levelStarsNum = 0;
    public int levelIndex;

    public void PressStarButton(int _starsNum)//MARKER This method will be trigger when you press one of the button on the game level
    {
        levelStarsNum = _starsNum;

        //MARKER ONLY Your stars number is greater than the record, you can save the new record
        //MARKER PlayerPrefs.GetInt("Lv" + levelIndex) his default value is 0
        if(levelStarsNum > PlayerPrefs.GetInt("Lv" + levelIndex))//KEY: Lv1; Value: Stars Number
        {
            PlayerPrefs.SetInt("Lv" + levelIndex, levelStarsNum);
        }

        Debug.Log("Saving Data is " + PlayerPrefs.GetInt("Lv" + levelIndex));
        UIManager.instance.BackMapSelection();
    }

    public void LoadLevelScene(string sceneName)
    {
        // Hide UI panels before loading the scene
        UIManager.instance.HideAllUIPanels();
        SceneManager.LoadScene(sceneName);
    }
}
