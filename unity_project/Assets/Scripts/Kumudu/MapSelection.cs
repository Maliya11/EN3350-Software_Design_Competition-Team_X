using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelection : MonoBehaviour
{
    public bool isUnlock = false; //indicate if the map is unlocked

    // GameObjects for lock and unlock states
    public GameObject lockGo;
    public GameObject unlockGo;

    public int mapIndex;//the index of this map
    public int questNum;// Number of stars required to unlock this map

    // Start and end levels for this map
    public int startLevel;
    public int endLevel;

    private void Update()
    {   
        // Update the map status and unlock status
        UpdateMapStatus();
        UnlockMap();
    }

    private void UpdateMapStatus()
    {
        if(isUnlock)
        {
            // Show the unlocked UI elements and hide the locked ones
            unlockGo.gameObject.SetActive(true);
            lockGo.gameObject.SetActive(false);
        }
        else
        {
            // Show the locked UI elements and hide the unlocked ones
            unlockGo.gameObject.SetActive(false);
            lockGo.gameObject.SetActive(true);
        }
    }

    // Check if the map can be unlocked based on the number of stars collected
    private void UnlockMap() 
    {
        // Check if the player has enough stars to unlock the map
        if (UIManager.instance.stars >= questNum)
        {
            isUnlock = true;
        }
        else
        {
            isUnlock = false;
        }
    }

}
