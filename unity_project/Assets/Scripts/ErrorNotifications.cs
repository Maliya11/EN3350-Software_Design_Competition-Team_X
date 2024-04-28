using System.Collections;
using UnityEngine;
using TMPro;

public class ErrorNotifications : Singleton<ErrorNotifications>
{
    // UI Elements
    public GameObject errorPanel;
    public TextMeshProUGUI errorText;

    private string errorMessage;

    // Method to display the error message
    public void DisplayErrorMessage(RequestBase request)
    {
        // Display the error panel
        errorPanel.SetActive(true);

        // Get the error message from the request
        if (request.jsonResponse["message"])
        {
            // If any message is attached with the json response of the request, get that as the error message
            errorMessage = request.jsonResponse["message"];
        }
        else
        {
            // If no message is attached with the json response of the request, get an error message corresponding to the error code
            errorMessage = GetErrorMessageForCode(request.errorCode);
        }

        // Display the error message
        errorText.text = errorMessage;
    }

    // Method to close the error panel
    public void CloseErrorPanel()
    {
        // Close the error panel
        errorPanel.SetActive(false);
    }

    // Method to get the error message for a given error code
    private string GetErrorMessageForCode(int errorCode)
    {
        switch (errorCode)
        {
            case 0:
                return "Network connection lost.\n Please check your internet connection.";
            case 400:
                return "Bad request.\n Please check your input.";
            case 401:
                return "Unauthorized.\n Please log in.";
            case 403:
                return "Access denied.\n You don't have permission.";
            case 404:
                return "Not found.\n The requested resource doesn't exist.";
            case 405:
                return "Action not allowed.\n Try a different method.";
            case 429:
                return "Too many requests.\n Slow down.";
            case 500:
                return "Server error.\n Please try again later.";
            case 502:
                return "Bad gateway.\n Please try again later.";
            case 503:
                return "Service unavailable.\n Please try again later.";
            case 504:
                return "Gateway timeout.\n Please try again later.";
            default:
                return "An unknown error occurred.\n Please try again later.";
        }
    }
}
