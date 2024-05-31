using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] skins;
    public int selectedCharacter;

    private void Awake(){
        LoadSelectedCharacter();
    }

    public void LoadSelectedCharacter()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach(GameObject player in skins){
            player.SetActive(false);
        }
        skins[selectedCharacter].SetActive(true);
    }

    public void ChangeNext(){
        skins[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if(selectedCharacter == skins.Length){
            selectedCharacter = 0;
        }
        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();
        Debug.Log("Changed to Next Character Index: " + selectedCharacter);
    }

    public void ChangePrevious(){
        skins[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if(selectedCharacter == - 1){
            selectedCharacter = skins.Length - 1;
        }
        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();
        Debug.Log("Changed to Previous Character Index: " + selectedCharacter);
    }

    public void ShowCharacterSelection()
    {
        LoadSelectedCharacter();
    }
}
