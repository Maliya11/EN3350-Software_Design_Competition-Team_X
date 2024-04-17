using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusPerks : Singleton<BonusPerks>
{
    // Get bonus perks for the player
    private int bonusPerks;

    // UI Elements
    public TextMeshProUGUI bonusPerksPanelText;

    private void Start()
    {
        // Get the bonus perks from the player preferences
        bonusPerks = PlayerPrefs.GetInt("questionnaireBonus");
        Debug.Log("Bonus Perks: " + bonusPerks);

        // Display the bonus perks in the UI
        bonusPerksPanelText.text = "Questionnaire Perks: " + bonusPerks;
        bonusPerksPanelText.fontStyle = FontStyles.Bold;
    }
}
