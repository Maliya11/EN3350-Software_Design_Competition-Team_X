using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusPerks : Singleton<BonusPerks>
{
    // Get bonus perks for the player
    private int revivalKeys;

    // UI Elements
    public TextMeshProUGUI bonusPerksPanelText;

    private void Start()
    {
        // Get the bonus perks from the player preferences
        revivalKeys = PlayerPrefs.GetInt("revivalKeys");
        Debug.Log("Bonus Perks: " + revivalKeys);

        // Display the bonus perks in the UI
        bonusPerksPanelText.text = "Revival Keys: " + revivalKeys;
        bonusPerksPanelText.fontStyle = FontStyles.Bold;
    }
}
