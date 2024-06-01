using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] skins;  // Array to hold different character skins
    public int selectedCharacter;  // Index of the currently selected character

    // Called when the script instance is being loaded
    private void Awake(){
        LoadSelectedCharacter();  // Load the previously selected character
    }

    // Load the previously selected character from PlayerPrefs
    public void LoadSelectedCharacter()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);  // Get the selected character index from PlayerPrefs, default to 0 if not found
        foreach(GameObject player in skins){
            player.SetActive(false);
        }
        skins[selectedCharacter].SetActive(true);  // Activate the currently selected character skin
    }

    // Change to the next character in the array
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

    // Change to the previous character in the array
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

    // Public method to show the character selection UI
    public void ShowCharacterSelection()
    {
        LoadSelectedCharacter();
    }
}
