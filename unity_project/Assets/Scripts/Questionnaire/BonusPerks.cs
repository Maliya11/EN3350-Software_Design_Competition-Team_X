using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusPerks : Singleton<BonusPerks>
{
    /*
    This script is used to display the bonus perks for the player.
    */

    // Get bonus perks for the player
    private int revivalPotions;

    // UI Elements
    public TextMeshProUGUI bonusPerksPanelText;

    private void Start()
    {
        // Get the bonus perks from the player preferences
        revivalPotions = PlayerPrefs.GetInt("revivalPotions");
        Debug.Log("Bonus Perks: " + revivalPotions);

        // Display the bonus perks in the UI
        bonusPerksPanelText.text = "Revival Potions: " + revivalPotions;
        bonusPerksPanelText.fontStyle = FontStyles.Bold;
    }
}
