using System.Collections;
using UnityEngine;
using TMPro;

public class ErrorNotifications : Singleton<ErrorNotifications>
{
    // UI Elements
    public GameObject errorPanel;
    public TextMeshProUGUI errorText;

    // Method to display the error message
    public void DisplayErrorMessage(int errorCode, string message)
    {
        // Display the error panel
        errorPanel.SetActive(true);

        // Display the error message
        errorText.text = message;
    }

    // Method to close the error panel
    public void CloseErrorPanel()
    {
        // Close the error panel
        errorPanel.SetActive(false);
    }
}
