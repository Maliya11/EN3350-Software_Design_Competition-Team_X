using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusPerks : MonoBehaviour
{
    // Get bonus perks for the player
    private int bonusPerks;

    // UI elements
    public TextMeshProUGUI bonusPerksPanelText;

    private void Start()
    {
        bonusPerks = PlayerPrefs.GetInt("questionnaireBonus");
        Debug.Log("Bonus Perks: " + bonusPerks);
        bonusPerksPanelText.text = "Questionnaire Perks: " + bonusPerks;
        bonusPerksPanelText.fontStyle = FontStyles.Bold;
    }
}
