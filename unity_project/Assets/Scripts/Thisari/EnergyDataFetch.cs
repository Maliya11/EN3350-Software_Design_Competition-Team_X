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

    /* // URL to view power consumption by specific month
    private string monthlyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/month/view";

    // URL to view power consumption by current month
    private string currentMonthPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/current-month/view"; */

    // URL to view daily power consumption by specific month
    private string specificMonthDailyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/month/daily/view";
    private JSONNode specificMonthDailyPowerConsumption;

    // URL to view daily power consumption by current month
    private string currentMonthDailyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/current-month/daily/view";
    private JSONNode currentMonthDailyPowerConsumption;

    /* // URL to view all power consumption
    private string allPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/all/view"; */

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

    // Method to get yearly power consumption taking year as a parameter
    public void GetYearlyPowerConsumption(string year, System.Action<JSONNode> callback)
    {
        StartCoroutine(FetchYearlyPowerConsumption(year, callback));
    }

    // Returns the power consumption in units of all the months of the year
    // Coroutine to fetch yearly power consumption
    private IEnumerator FetchYearlyPowerConsumption(string year, System.Action<JSONNode> callback)
    {
        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        JSONNode response = null;

        // Modify the URL to include the year
        string yearlyPowerConsumptionURL_withYear =  yearlyPowerConsumptionURL + "?year=" + year;

        // Send the request to fetch the yearly power consumption
        requestManager.SendRequest(yearlyPowerConsumptionURL_withYear, viewMethod, null, this, includeToken, null);
        yield return StartCoroutine(WaitForRequestCompletion((JSONNode res) => {
            response = res;
        }));

        if (response != null)
        {
            // Invoke the callback with the response
            callback(response["units"]);
        }
        else
        {
            callback(null);
        }
    }

    // Method to get power consumption by specific month taking year and month as parameters
    public void GetSpecificMonthPowerConsumption(string year, string month, System.Action<JSONNode> callback)
    {
        StartCoroutine(FetchSpecificMonthPowerConsumption(year, month, callback));
    }

    // Returns the power consumption in units of all the days of the month
    // Coroutine to fetch power consumption by specific month
    private IEnumerator FetchSpecificMonthPowerConsumption(string year, string month, System.Action<JSONNode> callback)
    {
        // Create a new instance of the RequestManager
        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        JSONNode response = null;

        // Modify the URL to include the year and month
        string specificMonthDailyPowerConsumptionURL_withYearAndMonth = specificMonthDailyPowerConsumptionURL + "?year=" + year + "&month=" + month;

        // Send the request to fetch the power consumption by specific month
        requestManager.SendRequest(specificMonthDailyPowerConsumptionURL_withYearAndMonth, viewMethod, null, this, includeToken, null);
        yield return StartCoroutine(WaitForRequestCompletion((JSONNode res) => {
            response = res;
        }));

        if (response != null)
        {
            // Invoke the callback with the response
            callback(response["dailyPowerConsumptionView"]["dailyUnits"]);  
        }
        else
        {
            callback(null);
        }
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
            // Invoke the callback with the response
            callback(response["dailyPowerConsumptionView"]["dailyUnits"]);
        }
        else
        {
            callback(null);
        }
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
