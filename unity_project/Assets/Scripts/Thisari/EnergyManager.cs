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

    // Flags to pause the energy manager
    public static bool isPausedEM;

    // Start is called before the first frame update
    void Start()
    {
        // Initiate the flags
        isPausedEM = false;

        // Debug.Log("Energy Manager Started");

        // Get the last fetched time and power consumption from the player prefs
        lastFetchTime = PlayerPrefs.GetString("LastFetchTime", "");
        lastPowerConsumption = PlayerPrefs.GetFloat("LastPowerConsumption", 0.0f);
        lastPowerConsumptionGap = PlayerPrefs.GetFloat("LastPowerConsumptionGap", 0.0f);

        energyDataFetch = FindObjectOfType<EnergyDataFetch>();
        treasureManager = FindObjectOfType<TreasureManager>();

        // Compare the power data when initially starting the game
        // Wait until CompareInitialWithInactivePowerConsumption is over
        StartCoroutine(InitializeAndStartFetching());
    }

    // Coroutine to initialize and start fetching power consumption
    private IEnumerator InitializeAndStartFetching()
    {
        int initialPowerChange;
        yield return StartCoroutine(CompareInitialWithInactivePowerConsumption(value => initialPowerChange = value));

        StartCoroutine(FetchCurrentPowerConsumption());
    }

    // Routine to fetch the current power consumption 
    private IEnumerator FetchCurrentPowerConsumption()
    {
        while (true)
        {
            Debug.Log("Energy Manager Paused.");
            // Wait until not paused
            yield return new WaitWhile(() => isPausedEM);
            Debug.Log("Energy Manager Resumed.");

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

            CompareCurrentPowerConsumption();

            // Wait for the next fetch
            yield return new WaitForSeconds(repeatRate);
        }
    }

    // Method to compare the power consumption
    private void CompareCurrentPowerConsumption()
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

    private IEnumerator CompareInitialWithInactivePowerConsumption(Action<int> result)
    {   
        int initialPowerChange = 5;
        
        string lastTime = lastFetchTime;
        DateTime lastDateTime;
        if (DateTime.TryParse(lastTime, out lastDateTime))
        {
            DateTime currentDateTime = DateTime.Now;
            
            // If currentTime and lastTime are on the same day
            // No change in Power Consumption considered
            // Randomly generate a bool 
            if (lastDateTime.Date == currentDateTime.Date)
            { 
                Debug.Log("Same day comparison => No change");
                initialPowerChange = -1;

                bool isFetchCompleted = false;

                // Fetch the current power consumption
                energyDataFetch.GetCurrentPowerConsumption((float powerConsumption) =>
                {
                    lastPowerConsumption = powerConsumption;
                    lastFetchTime = System.DateTime.Now.ToString();
                    isFetchCompleted = true;
                });

                // Wait until the fetch is completed
                yield return new WaitUntil(() => isFetchCompleted);
            }

            // Set the initial treasure count according to the results
            TreasureManager.isTreasureCountInitialized = true;
            Debug.Log("Treasure count initialized");
            treasureManager.SetInitialTreasureCount(initialPowerChange);

            // Wait for the first recurrent fetch
            yield return new WaitForSeconds(repeatRate);
        }

        // Return the result 
        result(initialPowerChange);
    }
}