using System;
using System.Collections;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    // Reference to the EnergyDataFetch
    private EnergyDataFetch energyDataFetch;
    // Reference to the TreasureManager
    private TreasureManager treasureManager;

    // Variables
    private float repeatRate = 15.0f;
    private string lastFetchTime = "";
    private string currentFetchTime = "";
    private float lastPowerConsumption = 0.0f;
    private float currentPowerConsumption = 0.0f;
    private float lastPowerConsumptionGap = 0.0f;
    private float currentPowerConsumptionGap = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Energy Manager Started");

        // Get the last fetched time and power consumption from the player prefs
        lastFetchTime = PlayerPrefs.GetString("LastFetchTime", "");
        lastPowerConsumption = PlayerPrefs.GetFloat("LastPowerConsumption", 0.0f);
        lastPowerConsumptionGap = PlayerPrefs.GetFloat("LastPowerConsumptionGap", 0.0f);

        energyDataFetch = FindObjectOfType<EnergyDataFetch>();
        treasureManager = FindObjectOfType<TreasureManager>();

        // Compare the power data when initially starting the game
        // GetInitialInactiveTimeInterval();

        // StartCoroutine(FetchCurrentPowerConsumption());
    }

    // Routine to fetch the current power consumption 
    private IEnumerator FetchCurrentPowerConsumption()
    {
        while (true)
        {
            Debug.Log("Fetching Current Power Consumption");
            Debug.Log("Last Fetch Time: " + lastFetchTime);
            Debug.Log("Last Power Consumption: " + lastPowerConsumption);
            Debug.Log("Last Power Consumption Gap: " + lastPowerConsumptionGap);

            bool isFetchCompleted = false;

            // Fetch the current power consumption
            energyDataFetch.GetCurrentPowerConsumption((float powerConsumption) =>
            {
                currentPowerConsumption = powerConsumption;
                currentFetchTime = System.DateTime.Now.ToString();
                currentPowerConsumptionGap = currentPowerConsumption - lastPowerConsumption;
                isFetchCompleted = true;
            });

            // Wait until the fetch is completed
            yield return new WaitUntil(() => isFetchCompleted);

            Debug.Log("Current Fetch Time: " + currentFetchTime);
            Debug.Log("Current Power Consumption: " + currentPowerConsumption);
            Debug.Log("Current Power Consumption Gap: " + currentPowerConsumptionGap);

            ComparePowerConsumption();

            // Wait for the next fetch
            yield return new WaitForSeconds(repeatRate);
        }
    }

    // Method to compare the power consumption
    private void ComparePowerConsumption()
    {
        // Compare the current power consumption with the last power consumption
        if (currentPowerConsumptionGap > lastPowerConsumptionGap)
        {
            Debug.Log("Energy Consumption Increased");

            // Decrease the number of visible treasures
            treasureManager.DecreaseVisibleTreasures();

        }
        else if (currentPowerConsumptionGap < lastPowerConsumptionGap)
        {
            Debug.Log("Energy Consumption Decreased");
            // Increase the number of visible treasures
            treasureManager.IncreaseVisibleTreasures();
        }

        // Update the last power consumption and fetch time
        lastPowerConsumption = currentPowerConsumption;
        lastFetchTime = currentFetchTime;
        lastPowerConsumptionGap = currentPowerConsumptionGap;

        // Save the last power consumption and fetch time to the player prefs
        PlayerPrefs.SetFloat("LastPowerConsumption", lastPowerConsumption);
        PlayerPrefs.SetString("LastFetchTime", lastFetchTime);
        PlayerPrefs.SetFloat("LastPowerConsumptionGap", lastPowerConsumptionGap);
    }

    /* // Method to get the time in between last fetched time and current time
    private void GetInitialInactiveTimeInterval()
    {
        string initiatingTime = System.DateTime.Now.ToString();
        // string lastTime = "1/30/2024 9:26:45 AM";
        Debug.Log("Current time: " + initiatingTime);
        Debug.Log("Last Fetched time: " + lastFetchTime);

        // Parse the last fetch time to DateTime
        DateTime lastFetchDateTime;
        if (DateTime.TryParse(lastFetchTime, out lastFetchDateTime))
        {
            // Get the current time
            DateTime currentDateTime = DateTime.Now;

            // Calculate the difference
            TimeSpan timeDifference = currentDateTime - lastFetchDateTime;

            // Extract the difference into years, months, days, hours, minutes, and seconds
            int years = currentDateTime.Year - lastFetchDateTime.Year;
            int months = currentDateTime.Month - lastFetchDateTime.Month;
            int days = currentDateTime.Day - lastFetchDateTime.Day;
            int hours = currentDateTime.Hour - lastFetchDateTime.Hour;
            int minutes = currentDateTime.Minute - lastFetchDateTime.Minute;
            int seconds = currentDateTime.Second - lastFetchDateTime.Second;

            // Adjust for negative values
            if (seconds < 0)
            {
                seconds += 60;
                minutes--;
            }
            if (minutes < 0)
            {
                minutes += 60;
                hours--;
            }
            if (hours < 0)
            {
                hours += 24;
                days--;
            }
            if (days < 0)
            {
                days += DateTime.DaysInMonth(lastFetchDateTime.Year, lastFetchDateTime.Month);
                months--;
            }
            if (months < 0)
            {
                months += 12;
                years--;
            }

            // Log the inactive time
            Debug.Log("Inactive time interval: [" + years + ", " + months + ", " + days + ", " + hours + ", " + minutes + ", " + seconds + "]");

            // Add the time interval to a list
            List<int> inactiveTimeInterval = new List<int>();
            inactiveTimeInterval.Add(years);
            inactiveTimeInterval.Add(months);
            inactiveTimeInterval.Add(days);
            inactiveTimeInterval.Add(hours);
            inactiveTimeInterval.Add(minutes);
            inactiveTimeInterval.Add(seconds);

            // Get the initial power gap
            GetInitialPowerGap(initiatingTime, lastFetchTime, inactiveTimeInterval);

        }
        else
        {
            Debug.LogError("Failed to parse last fetch time.");
        }
    }

    private void GetInitialPowerGap(string initiatingTime, string lastTime, List<int> inactiveTimeInterval)
    {
        string consumptionInInactiveTime = "0.0";
        string consumptionBeforeInactiveTime = "0.0";

        // Unpack the inactive time interval
        int years = inactiveTimeInterval[0];
        int months = inactiveTimeInterval[1];
        int days = inactiveTimeInterval[2];
        int hours = inactiveTimeInterval[3];
        int minutes = inactiveTimeInterval[4];
        int seconds = inactiveTimeInterval[5];

        // Check if initiating time and last time are in the same day by the string
        if (initiatingTime.Substring(0, 10) == lastTime.Substring(0, 10))
        {
            
        }
        else
        {
            
        }
    }

    // Method to get same day power gap
    private void GetSameDayPowerGap(string initiatingTime, string lastTime)
    {
        string consumptionInInactiveTime = "0.0";
        string consumptionBeforeInactiveTime = "0.0";

        // Get the power consumption in the inactive time
        energyDataFetch.GetCurrentPowerConsumption()
    }
 */
}