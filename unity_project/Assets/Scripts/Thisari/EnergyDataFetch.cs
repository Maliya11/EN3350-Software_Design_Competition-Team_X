using UnityEngine;
using System.Collections;
using SimpleJSON;

public class EnergyDataFetch : MonoBehaviour 
{
    // Reference to the RequestManager
    private RequestManager requestManager;
    private bool includeToken = true;
    // Reference to the ErrorNotifications
    public ErrorNotifications errorNotifications;

    // URL related to the energy information
    // URL to view yearly power consumption
    private string yearlyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/yearly/view";

    // URL to view power consumption by specific month
    private string monthlyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/month/view";

    // URL to view power consumption by current month
    private string currentMonthPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/current-month/view";

    // URL to view daily power consumption by specific month
    private string dailyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/month/daily/view";

    // URL to view daily power consumption by current month
    private string currentMonthDailyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/current-month/daily/view";
    private JSONNode currentMonthDailyPowerConsumption;

    // URL to view all power consumption
    private string allPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/all/view";

    // URL to view current power consumption
    private string currentPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/current/view";
    private float currentPowerConsumption;

    // Method to fetch energy data
    private string viewMethod = "GET";
    
    // Method to get current power consumption 
    public void GetCurrentPowerConsumption(System.Action<float> callback)
    {
        StartCoroutine(FetchCurrentPowerConsumption(callback));
    }

    // Returns the power consumption in Wh upto fetching time from 12am of current day
    // Coroutine to fetch current power consumption
    private IEnumerator FetchCurrentPowerConsumption(System.Action<float> callback)
    {
        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        JSONNode response = null;

        // Send the request to fetch the current power consumption
        requestManager.SendRequest(currentPowerConsumptionURL, viewMethod, null, this, includeToken, null);
        yield return StartCoroutine(WaitForRequestCompletion((JSONNode res) => {
            response = res;
        }));

        if (response != null)
        {
            currentPowerConsumption = response["currentConsumption"].AsFloat;
            Debug.Log("Power consumption from EnergyDataFetch: " + currentPowerConsumption);
        }

        // Invoke the callback with the current power consumption
        callback(currentPowerConsumption);
    }

    // Method to get current month power consumption 
    public void GetCurrentMonthPowerConsumption(System.Action<JSONNode> callback)
    {
        StartCoroutine(FetchCurrentMonthPowerConsumption(callback));
    }

    // Returns the power consumption in units of all the days of the current month
    // Coroutine to fetch current month power consumption
    private IEnumerator FetchCurrentMonthPowerConsumption(System.Action<JSONNode> callback)
    {
        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        JSONNode response = null;

        // Send the request to fetch the current month power consumption
        requestManager.SendRequest(currentMonthDailyPowerConsumptionURL, viewMethod, null, this, includeToken, null);
        yield return StartCoroutine(WaitForRequestCompletion((JSONNode res) => {
            response = res;
        }));

        if (response != null)
        {
            currentMonthDailyPowerConsumption = response["dailyPowerConsumptionView"]["dailyUnits"];
        }

        // Invoke the callback with the response
        callback(currentMonthDailyPowerConsumption);
    }


    // Coroutine to wait for the request completion
    private IEnumerator WaitForRequestCompletion(System.Action<JSONNode> callback)
    {
        // Wait until the request is completed
        while (!requestManager.isRequestCompleted)
        {
            yield return null;
        }

        // Check if the request is successful
        if (requestManager.isRequestSuccessful)
        {
            // Get the current power consumption from the response
            Debug.Log("Power Consumption request successful");

            // Assign the request response to response
            JSONNode response = requestManager.jsonResponse;
            Debug.Log("Response from EnergyDataFetch: " + response.ToString());

            // Pass the response to the callback
            callback(response);
        }
        else
        {
            // Show the error notification
            errorNotifications.DisplayErrorMessage(requestManager);
            callback(null);
        }
    }
}
